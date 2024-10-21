using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public static SaveData Instance;
    public GameData gameData = new GameData();

    [SerializeField]
    private TMP_Text[] nameText, fishHuntedText, moneyText;

    [SerializeField]
    private UpgradeController upgradeController;

    private string playerName;
    private int numberOfFish;
    public int NumberOfFishAdd { set { numberOfFish += value; } }

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
        gameData.scores.Add(new ScoreUser(playerName, numberOfFish, upgradeController.AmountOfMoney));

        string save = JsonUtility.ToJson(gameData);
        string _filePath = Application.persistentDataPath + "/Save.json";
        System.IO.File.WriteAllText(_filePath, save);

        numberOfFish = 0;
        LoadScore();
    }

    public void LoadScore()
    {
        string _filePath = Application.persistentDataPath + "/Save.json";
        string save = System.IO.File.ReadAllText(_filePath);

        gameData = JsonUtility.FromJson<GameData>(save);
        gameData.scores = gameData.scores.OrderBy(f => f.score).ToList();
        gameData.scores.Reverse();

        for (int i = 0; i < 5; i++)
        {
            if(i >= gameData.scores.Count)
            {
                nameText[i].gameObject.SetActive(false);
                fishHuntedText[i].gameObject.SetActive(false);
                moneyText[i].gameObject.SetActive(false);
            }
            else
            {
                nameText[i].gameObject.SetActive(true);
                if(gameData.scores[i].name == "")
                {
                    nameText[i].text = "Inconnue";
                }
                else
                {
                    nameText[i].text = gameData.scores[i].name;
                }
                fishHuntedText[i].gameObject.SetActive(true);
                fishHuntedText[i].text = gameData.scores[i].fishHunted.ToString();
                moneyText[i].gameObject.SetActive(true);
                moneyText[i].text = gameData.scores[i].money.ToString();
            }
        }
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
public struct ScoreUser
{
    public string name;
    public int fishHunted;
    public int money;
    public int score;

    public ScoreUser(string Name, int FishHunted,int Money)
    {
        name = Name;
        fishHunted = FishHunted;
        money = Money;
        score = Money + FishHunted;
    }
}