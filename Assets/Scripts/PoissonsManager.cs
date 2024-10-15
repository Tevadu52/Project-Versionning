using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PoissonsManager", order = 0)]
public class PoissonsManager : ScriptableObject
{
    [SerializeField]
    private Poisson[] Poissons = new Poisson[0];

    [SerializeField]
    private Zone[] Zones = new Zone[0];
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
}

[System.Serializable]
public class Poisson
{
    [HideInInspector]
    public int Id;
    public string Name;
    public string Description;
    public int Price;
    public int Level;
    public GameObject Prefab;
}

[System.Serializable]
public struct Zone
{
    public float minY;
    public float maxY;
}