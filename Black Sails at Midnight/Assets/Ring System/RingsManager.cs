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
    public int QuarterOfPathingPoints = 6;
    [SerializeField]
    [Range(0, 100000)]
    float SpaceBetweenRings = 64;
    [SerializeField]
    [Range(0, 100)]
    int NumberOfRings = 6;

    [SerializeField]
    List<GameObject> Rings;

    [SerializeField]
    GameObject Ring;

    private void Start()
    {
        Rings = new List<GameObject>();

        for (int i = 0; i < NumberOfRings; i++)
        {
            GameObject ring = Instantiate(Ring,transform);
            RingSystem temp = ring.GetComponent<RingSystem>();
            temp.Origin = Origin;
            temp.QuarterOfPathingPoints = QuarterOfPathingPoints;
            temp.Radius = Radius + SpaceBetweenRings * i;
            temp.GenerateRing();
            Rings.Add(ring);
        }
    }

    public RingSystem GetRing(int number)
    {
        if (number > Rings.Count || number < 0)
        {
            return null;
        }
        else
        {
            return Rings[number].GetComponent<RingSystem>();
        }
    }
}
