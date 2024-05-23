using System;
using System.IO;
using UnityEngine;

public enum LogLevel
{
    Debug = 0,
    Warning = 1,
    Info = 2,
    Error = 3,
}

public class LogFile : MonoBehaviour
{
    private string _path;

    public void WriteToLog(string message, LogLevel severity)
    {
        _path = Application.persistentDataPath + "/log.txt";

        try
        {
            using StreamWriter sw = File.AppendText(_path);

            if (severity == LogLevel.Debug)
                sw.WriteLine($"{DateTime.Now} [DEBUG] -- {message}");
            else if (severity == LogLevel.Error)
                sw.WriteLine($"{DateTime.Now} [ERROR] -- {message}");
            else if (severity == LogLevel.Warning)
                sw.WriteLine($"{DateTime.Now} [WARNING] -- {message}");
            else if (severity == LogLevel.Info)
                sw.WriteLine($"{DateTime.Now} [INFO] -- {message}");

        }
        catch (Exception e)
        {
            Debug.LogError($"Error Writing To Log File: {e.Message}");
            throw;
        }
    }
}
