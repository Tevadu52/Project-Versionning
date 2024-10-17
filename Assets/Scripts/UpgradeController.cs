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
        DisplayMoney();
        updateButtons();
    }
    public void SellAll()
    {
        amountOfMoney += PoissonsStock.Instance.PoissonsPrice;
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

    private void DisplayMoney()
    {
        moneyDisplayer.text = amountOfMoney.ToString() + "�";
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

    public void GoBack()
    {
        upgradePanel.SetActive(false);
        PoissonsManager.Instance.SpawnPoissons();
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

    private IEnumerator WaitAndSupress()
    {
        yield return new WaitForSeconds(1f);
        nameFish.text = "";
        StopAllCoroutines();
    }
}
