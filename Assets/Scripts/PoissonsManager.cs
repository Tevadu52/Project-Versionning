using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoissonsManager : MonoBehaviour
{
    [SerializeField]
    private Poisson[] Poissons = new Poisson[0];

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

    private void Start()
    {
        Rect rect = new Rect(-Xmax -2.5f, Ymin +2.5f, Xmax * 2 + 5, -Ymin + 5);
        CamMove.Instance.BoundsRect = rect;
    }

    public Poisson GetPoisson(int index) { return Poissons[index]; }
    public Zone GetZone(int index) { return Zones[index]; }
    public float Ymin
    {
        get 
        {
            return Zones[Zones.Length - 1].maxY;
        }
    }
    public int GetXmax() {return Xmax;}

    public void SpawnPoissons()
    {
        foreach (Transform currentPosition in transform)
        {
            Destroy(currentPosition.gameObject);
        }
        int id = 0;
        foreach (Poisson poisson in Poissons)
        {
            for (int i = 0; i < poisson.NumberByLevel; i++)
            {
                GameObject poissonPrefab = Instantiate(poisson.Prefab, transform, true);
                poissonPrefab.GetComponent<PoissonController>().Id = i;
            }
            id += 1;
        }
    }

    private void OnDrawGizmos()
    {
        Rect rect = new Rect(-Xmax, Ymin, Xmax * 2, -Ymin + 5);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(rect.center, rect.size);
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