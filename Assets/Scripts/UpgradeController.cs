using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private Text moneyDisplayer;
    [SerializeField] private List<Button> buttons;

    private int amountOfMoney;
    public void SellAll()
    {
        //amountOfMoney += PoissonStock.Instance.PoissonsTotalPrix();
        // PoissonStock.Instance.Clear();
        DisplayMoney();
    }

    public void Buy(int price)
    {
        //amountOfMoney -= price;
        //ligne pour augmenter le niveau de l'hameçon
        DisplayMoney();
    }

    public void DisplayMoney()
    {
        moneyDisplayer.text = amountOfMoney.ToString();
    }

    private void updateButtons()
    {
    }
}
