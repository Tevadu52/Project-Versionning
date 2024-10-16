using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PoissonsManager", order = 0)]
public class PoissonsManager : MonoBehaviour
{
    [SerializeField]
    private Poisson[] Poissons = new Poisson[0];
    [SerializeField]
    private GameObject PoissonsHolder;

    [SerializeField]
    private Zone[] Zones = new Zone[0];
    [SerializeField]
    private int Xmax;
    public static PoissonsManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }
    public Poisson GetPoisson(int index) { return Poissons[index]; }
    public Zone GetZone(int index) { return Zones[index]; }
    public int GetXmax() {return Xmax;}

    public void SpawnPoissons()
    {
        foreach (Transform currentPosition in PoissonsHolder.transform)
        {
            Destroy(currentPosition.gameObject);
        }
        int id = 0;
        foreach (Poisson poisson in Poissons)
        {
            for (int i = 0; i < poisson.NumberByLevel; i++)
            {
                GameObject poissonPrefab = Instantiate(poisson.Prefab);
                poissonPrefab.GetComponent<PoissonController>().Id = i;
            }
            id += 1;
        }
    }
}

[System.Serializable]
public class Poisson
{
    public string Name;
    public string Description;
    public float Speed;
    public int Price;
    public int Level;
    public int NumberByLevel;
    public GameObject Prefab;
}

[System.Serializable]
public struct Zone
{
    public float minY;
    public float maxY;
}