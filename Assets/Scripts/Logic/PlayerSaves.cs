using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSaves
{
    public string FileName = "PlayerSaves.txt";
    public PlayerData GameData;
    private DataManager _DataManager;

    [System.Serializable]
    public struct SaveStruct
    {
        public PlayerData GameData;
    }

    public PlayerSaves(DataManager dataManager)
    {
        _DataManager = dataManager;
        GameData = new PlayerData();
        GameData.RunTime = -1;
    }

    public void SetDataManager(DataManager dataManager)
    {
        _DataManager = dataManager;
    }

    public PlayerData GetPlayerData()
    {
        return GameData;
    }

    public void LoadHighScore(string json)
    {
        var _data = JsonUtility.FromJson<SaveStruct>(json);
        GameData = _data.GameData;
    }

    public void SaveHighScore(float time)
    {
        PlayerData _dataToSave = new PlayerData();
        _dataToSave.RunTime = time;

        SaveStruct _save = new SaveStruct() { GameData = _dataToSave };
        string _jsonString = JsonUtility.ToJson(_save, true);
        _DataManager.Save(FileName, _jsonString);
    }
}
