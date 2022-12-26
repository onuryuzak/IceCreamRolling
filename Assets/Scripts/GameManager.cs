using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Facebook.Unity;
using GameAnalyticsSDK;
using System.IO;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

public class GameManager : Singleton<GameManager>
{
    [ReadOnly]
    public int currentLevel = 1;
    [ReadOnly]
    public int totalCoins = 0;
    [ReadOnly]
    public bool completedTutorial = false;

    SaveData saveData = new SaveData();

    public void GameStart()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Level" + currentLevel);       
    }

    public void Success()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Level" + currentLevel);        
    }

    public void Fail()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Level" + currentLevel);      
    }

  
    void Awake()
    {
        DontDestroyOnLoad(this);
        GameAnalytics.Initialize();
        //FB.Init();
        LoadState();
    }

    private void OnDestroy()
    {
        SaveState();
    }

    public void SaveState()
    {
        string filePath = Application.persistentDataPath + "/saveFile.art";

        saveData.currentLevel = currentLevel;
        saveData.totalCoins = totalCoins;
        saveData.completedTutorial = completedTutorial;
        byte[] bytes = SerializationUtility.SerializeValue(saveData, DataFormat.Binary);
        File.WriteAllBytes(filePath, bytes);
    }

    public void LoadState()
    {
        string filePath = Application.persistentDataPath + "/saveFile.art";
        if (!File.Exists(filePath)) return; // No state to load

        byte[] bytes = File.ReadAllBytes(filePath);
        saveData = SerializationUtility.DeserializeValue<SaveData>(bytes, DataFormat.Binary);
        currentLevel = saveData.currentLevel;
        totalCoins = saveData.totalCoins;
        completedTutorial = saveData.completedTutorial;
    }

    [Button]
    public void ClearState()
    {
        string filePath = Application.persistentDataPath + "/saveFile.art";
        if (!File.Exists(filePath)) return; // No state to clear

        File.Delete(filePath);
    }
}
