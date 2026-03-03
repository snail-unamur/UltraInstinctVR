using System;
using System.IO;
using UnityEngine;

public class LogToFile : MonoBehaviour
{
    private string logFilePath;
    private StreamWriter writer;
    private static readonly object fileLock = new object();

    void Awake()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        logFilePath = Path.Combine(projectRoot, "Logs", "game_logs.txt");

        string directoryPath = Path.GetDirectoryName(logFilePath);
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);

        // ouvre UNE SEULE FOIS le writer
        writer = new StreamWriter(
            new FileStream(
                logFilePath,
                FileMode.Append,
                FileAccess.ReadWrite,
                FileShare.ReadWrite
            )
        );

        writer.AutoFlush = true;

        writer.WriteLine("\n=== New Session ===");

        Application.logMessageReceived += LogToFileMethod;
    }

    void OnDestroy()
    {
        Application.logMessageReceived -= LogToFileMethod;

        writer?.Close();
        writer?.Dispose();
    }

    private void LogToFileMethod(string logString, string stackTrace, LogType type)
    {
        lock (fileLock)
        {
            if (writer == null) return;

            writer.WriteLine($"[{DateTime.Now}] {type}: {logString}");

            if (type == LogType.Exception || type == LogType.Error)
            {
                writer.WriteLine(stackTrace);
            }
        }
    }
}