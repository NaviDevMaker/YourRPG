using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface ISavedData
{
    void Export(SaveData saveData);
    void Import(SaveData saveData);
}

public class DataController
{
    const string fileName = "save.json";
    string pass => Path.Combine(Application.persistentDataPath, fileName);
    public void SaveData(List<ISavedData> savedDatas)
    {
        SaveData saveData = null;
        if (!File.Exists(pass)) saveData = new SaveData();
        else
        {
            var json = File.ReadAllText(pass);
            saveData = JsonUtility.FromJson<SaveData>(json);
        }
        savedDatas.ForEach(data => data.Export(saveData));
        var newJson = JsonUtility.ToJson(saveData);
        File.WriteAllText(pass, newJson);
    }
    public async UniTask LoadData(List<ISavedData> savedDatas)
    {
        //ここにきて今まで誤解してた。YieldはLateUpdateの後あたりで実行されるやつで１フレーム後とは違うらしい
        await UniTask.NextFrame();
        if (!File.Exists(pass)) return;
        var json = File.ReadAllText(pass);
        var savedData = JsonUtility.FromJson<SaveData>(json);
        savedDatas.ForEach(data => data.Import(savedData));
    }
}
