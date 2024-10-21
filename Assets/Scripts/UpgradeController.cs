using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject startPanel;
    [Header("Textes")]
    [SerializeField] private TMP_Text FishCounter;
    [SerializeField] private TMP_Text nameFish;
    [SerializeField] private TMP_Text moneyDisplayer;
    [SerializeField] private TMP_Text timer;

    [SerializeField] private List<Button> buttons;
    [Header("Hook")]
    [SerializeField] private Hook hook;
    [Header("Prices")]
    [SerializeField] private int firstPrice;
    [SerializeField] private int secondPrice;
    [SerializeField] private int thirdPrice;
    [Header("Variables")]
    [SerializeField] private int timeToWait;
    [SerializeField, Range(60f, 1200f)] private float totalPlayTime;

    public int AmountOfMoney {get {return  amountOfMoney; } }
    private int amountOfMoney ;
    private int amountOfFish;
    private bool gamehasStarted;

    private void Start()
    {
        SetGameplay(false);
        InitiateButtons();
        DisplayMoney();
        updateButtons();
    }

    private void Update()
    {
        if (gamehasStarted)
        {
            UpdateTimer();
        }
        totalPlayTime -= Time.deltaTime;
    }

    public void SellAll()
    {
        amountOfMoney = Mathf.Min(amountOfMoney + PoissonsStock.Instance.PoissonsPrice, 9999);
        PoissonsStock.Instance.PoissonsClear();
        DisplayMoney();
        amountOfFish = 0;
        updateButtons();
    }

    public void Buy(int price)
    {
        if (amountOfMoney - price >= 0)
        {
            amountOfMoney -= price;
            DisplayMoney();
            updateButtons();
            hook.UpgradeHook();
        }
    }


    public void GoBack()
    {
        PoissonsManager.Instance.SpawnPoissons();
        StartCoroutine(WaitAndDisplay());
        upgradePanel.SetActive(false);
        gameplayPanel.SetActive(true);
    }

    public void GoToShop()
    {
        upgradePanel.SetActive(true);
        gameplayPanel.SetActive(false);
    }

    public void SetGameplay(bool state)
    {
        gameplayPanel.SetActive(state);
    }
    public bool GetGameplay()
    {
        return gameplayPanel.activeSelf;
    }
    public bool GetShop()
    {
        return upgradePanel.activeSelf;
    }

    public void DisplayFishName(int id)
    {
        nameFish.text =  "Vous avez attrap� un " + PoissonsManager.Instance.GetPoisson(id).Name + "!\n" + PoissonsManager.Instance.GetPoisson(id).Description;
        StartCoroutine(WaitAndSupress());
    }

    public void IncrementCounter()
    {
        amountOfFish++;
        FishCounter.text = amountOfFish + " poissons p�ch�s";
    }

    public void StartGame()
    {
        amountOfMoney = 0;
        PoissonsManager.Instance.SpawnPoissons();
        StartCoroutine(WaitAndDisplay());
        startPanel.SetActive(false);
        upgradePanel.SetActive(false);
        gameplayPanel.SetActive(true);
        gamehasStarted = true;
    }

    private void UpdateTimer()
    {
        timer.text = ((int)totalPlayTime / 60).ToString() + ":" + ((int)totalPlayTime % 60).ToString();
        if (totalPlayTime <= 0)
        {
            startPanel.SetActive(true);
            upgradePanel.SetActive(false);
            gameplayPanel.SetActive(false);
            gamehasStarted = false;
            SaveData.Instance.SaveScore();
        }

    }

    private void DisplayMoney()
    {
        moneyDisplayer.text =  "Argent :" + amountOfMoney.ToString() + "�";
    }

    private void updateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
        if (amountOfMoney >= firstPrice && hook.Level == 0)
        {
            buttons[0].interactable = true;
        } else if (amountOfMoney >= secondPrice && hook.Level == 1)
        {
            buttons[1].interactable = true;
        } else if (amountOfMoney >= thirdPrice && hook.Level == 2)
        {
            buttons[2].interactable = true;
        }
    }

    private void InitiateButtons()
    {
        buttons[0].GetComponentInChildren<TMP_Text>().text = "Hame�on de niveau 2 :" + firstPrice.ToString() + "�.";
        buttons[1].GetComponentInChildren<TMP_Text>().text = "Hame�on de niveau 3 :" + secondPrice.ToString() + "�.";
        buttons[2].GetComponentInChildren<TMP_Text>().text = "Hame�on de niveau 4 :" + thirdPrice.ToString() + "�.";
    }

    private IEnumerator WaitAndSupress()
    {
        yield return new WaitForSeconds(timeToWait);
        nameFish.text = "";
        StopAllCoroutines();
    }

    private IEnumerator WaitAndDisplay()
    {
        yield return new WaitForSeconds(0.5f);
        StopAllCoroutines();
    }
}
