using System;
using System.Diagnostics;
using UnityEngine;

public class GetCPUTime : MonoBehaviour
{


    private void Start()
    {
        TimeSpan cpuTime = GetCurrentCPUTime();
        
    }




    public static TimeSpan GetCurrentCPUTime()
    {
        Process currentProcess = Process.GetCurrentProcess();
        TimeSpan cpuTime = currentProcess.TotalProcessorTime;
        return cpuTime;
    }


    string Generate()
    {
        string uuid = Guid.NewGuid().ToString().Substring(0, 5);
        string date = DateTime.Now.ToString("ddMMyyyy");
        return $"result_{uuid}_{date}.csv";
    }



}
