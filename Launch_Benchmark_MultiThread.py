import subprocess
import time
import os
from multiprocessing import Pool
import csv
import psutil
import uuid
from datetime import datetime
import xml.etree.ElementTree as ET


# Chemins vers les éditeurs Unity
UNITY_PATHS = [

    r"C:\\Program Files\\Unity\\Hub\\Editor\\6000.0.62f1\\Editor\\Unity.exe"
]

# Chemins vers les projets Unity
PROJECT_PATHS = [

    r"C:\\Users\\glongfil\\Documents\\GitHub\\UltraInstinctVR\\CodeBase"

]

REPEAT_COUNT = 1
TIMEOUT = 300


# =========================
# Utils CPU
# =========================
def get_total_cpu_time(proc: psutil.Process):
    """CPU total du process + enfants."""
    total = 0.0

    try:
        cpu = proc.cpu_times()
        total += cpu.user + cpu.system
    except Exception:
        pass

    try:
        for child in proc.children(recursive=True):
            try:
                c = child.cpu_times()
                total += c.user + c.system
            except Exception:
                pass
    except Exception:
        pass

    return total


# =========================
# CSV Path helper
# =========================
def create_csv_for_project(project_path):
    cpu_dir = os.path.join(project_path, "Logs", "cpu_time")
    os.makedirs(cpu_dir, exist_ok=True)

    short_uuid = str(uuid.uuid4())[:6]
    csv_path = os.path.join(cpu_dir, f"cpu_time_{short_uuid}.csv")

    return csv_path


# =========================
# CSV Writer
# =========================
def write_csv_row(csv_path, project_index, iteration, success, cpu_time, start_time, end_time):
    file_exists = os.path.exists(csv_path)

    with open(csv_path, "a", newline="", encoding="utf-8") as f:
        writer = csv.writer(f)

        if not file_exists:
            writer.writerow([
                "project",
                "iteration",
                "success",
                "cpu_time_sec",
                "start_time",
                "end_time"
            ])

        writer.writerow([
            project_index + 1,
            iteration + 1,
            success,
            round(cpu_time, 3),
            start_time,
            end_time
        ])


# =========================
# Wait for ACTION_DONE
# =========================
def wait_for_action_done(log_file_path, timeout=TIMEOUT, process=None):
    start_time = time.time()

    while time.time() - start_time < timeout:
        if os.path.exists(log_file_path):
            try:
                with open(log_file_path, "r", encoding="utf-8") as f:
                    if "ACTION_DONE" in f.read():
                        print("✅ ACTION_DONE détecté.")
                        time.sleep(100)
                        if process:
                            process.terminate()
                        return True
            except Exception:
                pass

        time.sleep(1)

    print("⚠️ Timeout atteint sans ACTION_DONE.")
    if process:
        process.terminate()
    return False


# =========================
# Run Unity once
# =========================
def run_unity_once(project_path, unity_path, index, iteration):
    print(f"\n🚀 Lancement Unity - Projet {index+1} | Itération {iteration+1}")

    # ⏱️ start time réel
    start_dt = datetime.utcnow().isoformat()

    log_file_path = os.path.join(project_path, "Assets", "Scripts", "game_logs.txt")

    if os.path.exists(log_file_path):
        os.remove(log_file_path)

    command = [
        unity_path,
        "-projectPath", project_path,
        "-runTests",
        "-testPlatform", "Playmode",
        "-enableCodeCoverage",
        "-coverageOptions", "generateAdditionalMetrics;generateHtmlReport;assemblyFilters:+Assembly-CSharp"

    ]

    process = subprocess.Popen(
        command,
        stdout=subprocess.DEVNULL,
        stderr=subprocess.DEVNULL
    )

    ps_proc = psutil.Process(process.pid)

    # CPU début
    start_cpu = get_total_cpu_time(ps_proc)

    success = wait_for_action_done(
        log_file_path=log_file_path,
        process=process
    )


    export_coverage_to_csv(project_path)

    # CPU fin (avant disparition)
    end_cpu = get_total_cpu_time(ps_proc)

    process.wait()

    cpu_used = max(0.0, end_cpu - start_cpu)

    end_dt = datetime.utcnow().isoformat()

    return (index, iteration, success, cpu_used, start_dt, end_dt)


# =========================
# Iterations par projet
# =========================
def run_project_iterations(project_index):
    project_path = PROJECT_PATHS[project_index]
    unity_path = UNITY_PATHS[project_index]

    csv_path = create_csv_for_project(project_path)

    results = []

    for i in range(REPEAT_COUNT):
        try:
            index, iteration, success, cpu_used, start_dt, end_dt = run_unity_once(
                project_path,
                unity_path,
                project_index,
                i
            )

            write_csv_row(
                csv_path,
                index,
                iteration,
                success,
                cpu_used,
                start_dt,
                end_dt
            )

            if success:
                print(f"✔️ Projet {index+1} | Itération {iteration+1} réussie.")
            else:
                print(f"❌ Projet {index+1} | Itération {iteration+1} échouée.")

            print(f"🧠 CPU utilisé: {cpu_used:.3f} sec")

            results.append(success)

        except Exception as e:
            print(f"💥 Erreur durant l'itération {i+1} du projet {project_index+1} : {e}")
            results.append(False)

        time.sleep(3)

    return results


# =========================
# Parallel runner
# =========================

def chunk_list(lst, size):
    """Split list into chunks of given size."""
    for i in range(0, len(lst), size):
        yield list(range(i, min(i + size, len(lst))))



def run_projects_in_parallel():
    if len(PROJECT_PATHS) != len(UNITY_PATHS):
        print("⚠️ Le nombre de projets et d'éditeurs ne correspond pas.")
        return

    BATCH_SIZE = 3

    all_results = [None] * len(PROJECT_PATHS)

    for batch_indices in chunk_list(PROJECT_PATHS, BATCH_SIZE):
        print(f"\n🚀 Lancement batch: {batch_indices}")

        with Pool(len(batch_indices)) as pool:
            results = pool.map(run_project_iterations, batch_indices)

        # Store results in correct positions
        for idx, res in zip(batch_indices, results):
            all_results[idx] = res

    # Final summary
    for idx, results in enumerate(all_results):
        success_count = results.count(True)
        fail_count = results.count(False)
        print(f"\n📊 Résumé du projet {idx+1} : {success_count} succès / {REPEAT_COUNT} | {fail_count} échecs")



#=========================
# Collect coverage metric
#=========================

def export_coverage_to_csv(project_path):
    xml_path = os.path.join(project_path, "CodeCoverage", "Report", "Summary.xml")

    folder_path = os.path.join(project_path, "Logs", "coverage")
    os.makedirs(folder_path, exist_ok=True)

    filename = f"coverage_{str(uuid.uuid4())[:6]}.csv"
    csv_path = os.path.join(folder_path, filename)

    if not os.path.exists(xml_path):
        print(f"❌ Coverage Summary not found at: {xml_path}")
        return

    try:
        tree = ET.parse(xml_path)
        root = tree.getroot()

        summary = root.find(".//Summary")

        if summary is None:
            print("❌ Summary node not found in coverage report.")
            return

        total_methods = summary.findtext("Totalmethods", default="0")
        covered_methods = summary.findtext("Coveredmethods", default="0")
        method_coverage = summary.findtext("Methodcoverage", default="0")
        line_coverage = summary.findtext("Linecoverage", default="0")
        coverage_branch = summary.findtext("Coveredbranches",default="0")
        total_branch = summary.findtext("Totalbranches",default="0")


        with open(csv_path, "w", newline="", encoding="utf-8") as f:
            writer = csv.writer(f)

            writer.writerow([
                "MethodCoverage",
                "TotalMethods",
                "CoveredMethods",
                "LineCoverage",
                "coverage_branch",
                "total_branch"
            ])

            writer.writerow([
                method_coverage,
                total_methods,
                covered_methods,
                line_coverage,
                coverage_branch,
                total_branch
            ])

        print(f"📄 Coverage CSV generated at: {csv_path}")

    except Exception as e:
        print(f"💥 Error parsing coverage XML: {e}")



# =========================
# Main
# =========================
if __name__ == "__main__":
    run_projects_in_parallel()