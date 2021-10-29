using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    PlayerSaves SaveFile;
    public DataManager DataManager;

    public float Load()
    {
        SaveFile = new PlayerSaves(DataManager);
        
        var _json = DataManager.Load(SaveFile.FileName);
        if (string.IsNullOrWhiteSpace(_json))
        {
            SaveFile.SaveHighScore(-1);
        }
        else
        {
            SaveFile.LoadHighScore(_json);
        }
        return SaveFile.GameData.RunTime;
    }

    public void Save()
    {
        SaveFile.SaveHighScore(GameSettings.GameLogic.RunTime);
    }
}
