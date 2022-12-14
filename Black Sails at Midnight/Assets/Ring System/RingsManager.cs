using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingsManager : MonoBehaviour
{
    [SerializeField]
    public float Radius = 3;
    [SerializeField]
    public Coordinates Origin;
    [SerializeField]
    [Range(0, 90)]
    public int NumberOfPathingPoints = 6;
    [SerializeField]
    float SpaceBetweenRings = 64;
    [SerializeField]
    [Range(0, 100)]
    int NumberOfRings = 6;

    [SerializeField]
    int BaseShipCapacity = 4;
    [SerializeField]
    int IncreaseInShipCapacity = 4;

    [SerializeField]
    public bool isDoneGenerating;

    [SerializeField]
    List<GameObject> Rings;

    [SerializeField]
    GameObject Ring;

    private void Start()
    {
        Rings = new List<GameObject>();
        int CurrentShipCapacity = BaseShipCapacity;
        for (int i = 0; i < NumberOfRings; i++)
        {
            GameObject ring = Instantiate(Ring,transform);
            RingSystem temp = ring.GetComponent<RingSystem>();
            temp.Origin = Origin;
            temp.NumberOfPathingPoints = NumberOfPathingPoints + 2;
            temp.Radius = Radius + SpaceBetweenRings * i;
            temp.GenerateRing();
            temp.GetComponentInChildren<RingMarkerHandler>().Radius = temp.Radius;

            temp.SetupShipList(CurrentShipCapacity);
            if (i == 0 || i == NumberOfRings - 1)
            {
                temp.DisableMarker = true;
            } else {
                CurrentShipCapacity += IncreaseInShipCapacity;
            }

            Rings.Add(ring);
        }

        GameObject.Find("WaveManager").GetComponent<PrimaryWaveSystem>().enabled = true;        
    }

    public RingSystem GetRing(int index)
    {
        if (index > Rings.Count || index < 0)
        {
            return null;
        }
        else
        {
            return Rings[index].GetComponent<RingSystem>();
        }
    }

    public int GetNumberOfRings()
    {
        return Rings.Count;
    }
}
