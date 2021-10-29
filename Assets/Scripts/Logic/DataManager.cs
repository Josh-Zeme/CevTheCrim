using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DataManager : MonoBehaviour
{
    private string _Path = null;
    public void Save(string fileName, string data)
    {
        System.IO.File.WriteAllText(GetFilePath(fileName), data);
    }

    public string Load(string fileName)
    {
        string _json = ReadFromFile(fileName);
        return _json;
    }

    public string GetFilePath(string fileName)
    {
        _Path = @$"{Application.persistentDataPath}/{fileName}";
        return _Path;
    }

    private string ReadFromFile(string fileName)
    {
        string _path = GetFilePath(fileName);
        if (File.Exists(_path))
        {
            using (StreamReader reader = new StreamReader(_path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not Found");
            return "";
        }
    }


}
