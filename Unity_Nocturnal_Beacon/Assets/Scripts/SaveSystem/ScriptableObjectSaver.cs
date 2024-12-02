using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ScriptableObjectSaver 
{
    private static string GetSavePath(string folderName, string fileName)
    {
        return Path.Combine(Application.persistentDataPath, folderName, fileName + ".json");
    }
    private static string GetSavePath(string folderName)
    {
        return Path.Combine(Application.persistentDataPath, folderName);
    }

    public static void SaveScriptableObject(ScriptableObject so, string folderName)
    {
        if (so == null)
        {
            Debug.LogError("No ScriptableObject assigned!");
            return;
        }

        string json = JsonUtility.ToJson(so, true);
        string folderPath = GetSavePath(folderName);
        string filePath = Path.Combine(folderPath, so.name + ".json");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log($"Folder created: {folderPath}");
        }
        
        File.WriteAllText(filePath, json);
        Debug.Log($"ScriptableObject saved to: {filePath}");
    }

    public static List<T> LoadScriptableObject<T>(string folderName)
    {
        string path = GetSavePath(folderName);
        List<T> objects = new List<T>();

        if (!File.Exists(path))
        {
            Debug.LogError($"No save file found in {path}.");
            return objects;
        }

        string[] jsonFiles = Directory.GetFiles(path, "*.json");

        foreach(var file in jsonFiles)
        {
            string json = File.ReadAllText(file);
            var so = JsonUtility.FromJson<T>(json);
            objects.Add(so);
        }

        Debug.Log($"ScriptableObject loaded from: {path}");
        return objects;
    }
}
