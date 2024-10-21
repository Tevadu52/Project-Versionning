using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;
    public GameData gameData = new GameData();

    [SerializeField]
    private UpgradeController upgradeController;

    private string playerName;
    private int numberOfFish;
    public int NumberOfFishAdd { set { numberOfFish++; } }

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }
    private void Start()
    {
        string _filePath = Application.persistentDataPath + "/Save.json";
        if (System.IO.File.Exists(_filePath))
        {
            LoadScore();
        }
    }

    public void SaveScore()
    {
        ScoreUser scoreUser = new ScoreUser();
        scoreUser.name = playerName;
        scoreUser.score = upgradeController.AmountOfMoney;
        scoreUser.fishHunted = numberOfFish;
        gameData.scores.Add(scoreUser);

        string save = JsonUtility.ToJson(gameData);
        string _filePath = Application.persistentDataPath + "/Save.json";
        System.IO.File.WriteAllText(_filePath, save);
    }

    public void LoadScore()
    {
        string _filePath = Application.persistentDataPath + "/Save.json";
        string save = System.IO.File.ReadAllText(_filePath);

        gameData = JsonUtility.FromJson<GameData>(save);

    }

    public void setPlayerName(string name)
    {
        playerName = name;
    }
}

[System.Serializable]
public class GameData
{
    public List<ScoreUser> scores = new List<ScoreUser>();
}

[System.Serializable]
public class ScoreUser
{
    public string name;
    public int score;
    public int fishHunted;
}