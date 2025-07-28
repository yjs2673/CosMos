using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject PlanetPrefab;
    public GameObject CloudPrefab;
    public GameObject BlackholePrefab;

    GameObject[] planet;
    GameObject[] cloud;
    GameObject[] blackhole;

    GameObject[] targetPool;

    void Awake()
    {
        planet = new GameObject[300];
        cloud = new GameObject[300];
        blackhole = new GameObject[300];

        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < planet.Length; i++)
        {
            planet[i] = Instantiate(PlanetPrefab);
            planet[i].SetActive(false);
        }
        for (int i = 0; i < cloud.Length; i++)
        {
            cloud[i] = Instantiate(CloudPrefab);
            cloud[i].SetActive(false);
        }
        for (int i = 0; i < blackhole.Length; i++)
        {
            blackhole[i] = Instantiate(BlackholePrefab);
            blackhole[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "Planet":
                targetPool = planet;
                break;
            case "Cloud":
                targetPool = cloud;
                break;
            case "Blackhole":
                targetPool = blackhole;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "Planet":
                targetPool = planet;
                break;
            case "Cloud":
                targetPool = cloud;
                break;
            case "Blackhole":
                targetPool = blackhole;
                break;
        }
        return targetPool;
    }
}
