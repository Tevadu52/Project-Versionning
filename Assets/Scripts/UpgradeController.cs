using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    [Header ("Panels")]
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private GameObject namePanel;
    [Header("Texts")]
    [SerializeField] private Text moneyDisplayer;
    [SerializeField] private Text fishName;
    [SerializeField] private List<Button> buttons;

    private int amountOfMoney;

    private void Start()
    {
        amountOfMoney = 0;
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

    public void DisplayMoney()
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
        UpgradePanel.SetActive(false);
        PoissonsManager.Instance.SpawnPoissons();
    }

    public void DisplayFishName(int id)
    {
        namePanel.SetActive(true);
        fishName.text = PoissonsManager.Instance.GetPoisson(id).Name;
        StartCoroutine(WaitAndDisable());
    }

    public IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(1f);
        namePanel.SetActive(false);
    }
}
