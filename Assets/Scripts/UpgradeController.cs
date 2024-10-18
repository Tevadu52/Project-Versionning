using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject upgradePanel;
    [Header("Textes")]
    [SerializeField] private Text FishCounter;
    [SerializeField] private Text nameFish;
    [SerializeField] private Text moneyDisplayer;
    [SerializeField] private List<Button> buttons;
    [Header("Hook")]
    [SerializeField] private Hook hook;
    [Header("Prices")]
    [SerializeField] private int firstPrice;
    [SerializeField] private int secondPrice;
    [SerializeField] private int thirdPrice;

    private int amountOfMoney;
    private int amountOfFish;

    private void Start()
    {
        amountOfMoney = 0;
        SetGameplay(false);
        InitiateButtons();
        DisplayMoney();
        updateButtons();
    }
    public void SellAll()
    {
        amountOfMoney = Mathf.Min(amountOfMoney + PoissonsStock.Instance.PoissonsPrice, 999);
        PoissonsStock.Instance.PoissonsClear();
        DisplayMoney();
        amountOfFish = 0;
    }

    public void Buy(int price)
    {
        if (amountOfMoney - price >= 0)
        {
            amountOfMoney -= price;
            DisplayMoney();
            updateButtons();
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
        nameFish.text =  "Vous avez attrapé un " + PoissonsManager.Instance.GetPoisson(id).Name + "!\n" + PoissonsManager.Instance.GetPoisson(id).Description;
        StartCoroutine(WaitAndSupress());
    }

    public void IncrementCounter()
    {
        amountOfFish++;
        FishCounter.text = amountOfFish + " poissons pêchés";
    }
    private void DisplayMoney()
    {
        moneyDisplayer.text =  "Argent :" + amountOfMoney.ToString() + "€";
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
        buttons[0].GetComponentInChildren<Text>().text = "Hameçon de niveau 2 :" + firstPrice.ToString() + "€.";
        buttons[1].GetComponentInChildren<Text>().text = "Hameçon de niveau 3 :" + secondPrice.ToString() + "€.";
        buttons[2].GetComponentInChildren<Text>().text = "Hameçon de niveau 4 :" + thirdPrice.ToString() + "€.";
    }

    private IEnumerator WaitAndSupress()
    {
        yield return new WaitForSeconds(1f);
        nameFish.text = "";
        StopAllCoroutines();
    }

    private IEnumerator WaitAndDisplay()
    {
        yield return new WaitForSeconds(0.5f);
        StopAllCoroutines();
    }
}
