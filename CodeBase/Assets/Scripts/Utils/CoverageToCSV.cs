using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

public static class CoverageToCSV
{
    [MenuItem("Tools/Coverage/Export Summary CSV")]
    public static void Export()
    {
        string xmlPath = "CodeCoverage/Report/Summary.xml";


        string folderPath = Path.Combine(Application.dataPath, "..", "Logs", "coverage");
        string csvPath = Path.Combine(folderPath, "coverage_summary.csv");

    
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("Created folder: " + folderPath);
        }

        if (!File.Exists(xmlPath))
        {
            Debug.LogError("Coverage Summary not found at: " + xmlPath);
            return;
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(xmlPath);

        XmlNode summary = doc.SelectSingleNode("//CoverageReport/Summary");

        if (summary == null)
        {
            Debug.LogError("Summary node not found in coverage report.");
            return;
        }

        string totalMethods = summary["Totalmethods"].InnerText;
        string coveredMethods = summary["Coveredmethods"].InnerText;
        string methodCoverage = summary["Methodcoverage"].InnerText;
        string lineCoverage = summary["Linecoverage"].InnerText;

        using (StreamWriter writer = new StreamWriter(csvPath))
        {
            writer.WriteLine("MethodCoverage,TotalMethods,CoveredMethods,LineCoverage");
            writer.WriteLine($"{methodCoverage},{totalMethods},{coveredMethods},{lineCoverage}");
        }

        Debug.Log("Coverage summary CSV generated at: " + csvPath);
    }
}