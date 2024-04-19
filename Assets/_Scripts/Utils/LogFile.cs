using System;
using System.IO;
using UnityEngine;

public class LogFile : MonoBehaviour
{
    private string _path;

    void Start()
    {
        _path = Application.dataPath + "/log.txt";
        Debug.Log(_path);
    }

    public void WriteToLog(string message)
    {
        try
        {
            using StreamWriter sw = File.AppendText(_path);
            sw.WriteLine($"{DateTime.Now}: {message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error Writing To Log File: {e.Message}");
            throw;
        }
    }
}
