using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class JsonLoader 
{
    private string fileName = "/Mistakes.json";
    private static JsonLoader _instance; 
    
    public static JsonLoader GetInstance()
    {
        if (_instance == null)
            _instance = new JsonLoader();
        return _instance;
    }
    
    public void Save(List<Mistake> mistakes)
    {
        File.WriteAllText(Application.streamingAssetsPath + fileName,JsonHelper.ToJson(mistakes.ToArray(), true));
    }
    
    public List<Mistake> Load()
    {
        var jsonText = File.ReadAllText(Application.streamingAssetsPath + fileName);
        var mistake = JsonHelper.FromJson<Mistake>(jsonText);
        
        return mistake.ToList();
    }
}