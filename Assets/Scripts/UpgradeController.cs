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
    [SerializeField] private Text nameFish;
    [SerializeField] private Text moneyDisplayer;
    [SerializeField] private List<Button> buttons;

    private int amountOfMoney;

    private void Start()
    {
        amountOfMoney = 0;
        SetGameplay(false);
    }
    public void SellAll()
    {
        amountOfMoney += PoissonsStock.Instance.PoissonsPrice;
        PoissonsStock.Instance.PoissonsClear();
        DisplayMoney();
    }

    public void Buy(int price)
    {
        amountOfMoney -= price;
        //ligne pour augmenter le niveau de l'hameçon
        DisplayMoney();
        updateButtons();
    }

    private void DisplayMoney()
    {
        moneyDisplayer.text = amountOfMoney.ToString();
    }

    private void updateButtons()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].interactable = false;
        }
        // buttons[nomDuNiveauDeLaCanneAPeche + 1].interactable = true;
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
        nameFish.text = PoissonsManager.Instance.GetPoisson(id).Name;
        StartCoroutine(WaitAndDisable());
    }

    private IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(1f);
        nameFish.text = "";
        StopAllCoroutines();
    }
}
