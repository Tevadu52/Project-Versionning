using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonsStock : MonoBehaviour
{
    private List<Poisson> poissons = new List<Poisson>();
    public int PoissonsPrice 
    { 
        get 
        {
            int finalPrice = 0;
            foreach (Poisson poisson in poissons)
            {
                finalPrice += poisson.Price;
            }
            return finalPrice;
        }
    }
    public void PoissonsClear() { poissons.Clear(); }
    public void PoissonsAdd(Poisson poisson) { poissons.Add(poisson); }
    public static PoissonsStock Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
}