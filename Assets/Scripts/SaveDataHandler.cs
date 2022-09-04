using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveDataHandler : MonoBehaviour
{
    public static SaveDataHandler Instance;

    public string CurrentPlayerName;
    public int CurrentPlayerScore = 0;
    public string BestPlayerName;
    public int BestPlayerScore = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayerData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScore(int addedPoints)
    {
        // Add to the current score.
        CurrentPlayerScore += addedPoints;
        // Transfer to the save data handler.
        if (CurrentPlayerScore > BestPlayerScore)
        {
            BestPlayerScore = CurrentPlayerScore;
            BestPlayerName = CurrentPlayerName;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string currentPlayerName;
        public int currentPlayerScore;
        public string bestPlayerName;
        public int bestPlayerScore;
    }

    public void SavePlayerData()
    {
        SaveData data = new SaveData();
        data.currentPlayerName = CurrentPlayerName;
        data.currentPlayerScore = CurrentPlayerScore;
        data.bestPlayerName = BestPlayerName;
        data.bestPlayerScore = BestPlayerScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(SaveFilePath(), json);
    }

    public void LoadPlayerData()
    {
        string path = SaveFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            CurrentPlayerName = data.currentPlayerName;
            CurrentPlayerScore = data.currentPlayerScore;
            BestPlayerName = data.bestPlayerName;
            BestPlayerScore = data.bestPlayerScore;
        }
    }

    public void ResetPlayerData()
    {
        string path = SaveFilePath();
        if (File.Exists(path))
        {
            // Delete the saved data.
            File.Delete(path);
        }
        CurrentPlayerName = null;
        CurrentPlayerScore = 0;
        BestPlayerName = null;
        BestPlayerScore = 0;
        SavePlayerData();
    }

    private string SaveFilePath()
    {
        return Application.persistentDataPath + "/freakout.json";
    }
}
