using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[Serializable]
public struct Coordinates
{
    public Coordinates(float X, float Y)
    {
        x = X;
        y = Y;
    }
    public float x;
    public float y;
}

public class RingSystem : MonoBehaviour
{
    [SerializeField]
    public float Radius = 3;
    [SerializeField]
    public Coordinates Origin;
    [SerializeField]
    [Range(0, 90)]
    public int QuarterOfPathingPoints = 6;

    [SerializeField]
    public List<Coordinates> Ring;

    [SerializeField]
    GameObject Point;

    public void GenerateRing()
    {
        Ring = new List<Coordinates>();

        Radius = Mathf.Sqrt(Radius);

        for (int index = 0; index < QuarterOfPathingPoints + 1; index++)
        {
            float x = Origin.x + (Radius / QuarterOfPathingPoints * index);


            if (Origin.x == x)
            {
                Ring.Add(new Coordinates(x, Radius));
                Ring.Add(new Coordinates(x, x - Radius));
            }
            else if (x == Radius)
            {
                Ring.Add(new Coordinates(x, Origin.y));
                Ring.Add(new Coordinates(x - Radius * 2, Origin.y));
            }

            float y = Mathf.Sqrt(Mathf.Pow(Radius, 2) - Mathf.Pow(x, 2));
            y -= Mathf.Pow(Origin.x, 2);
            y += Mathf.Pow(Origin.y, 2);

            Ring.Add(new Coordinates(x, y));
            Ring.Add(new Coordinates(Origin.x - x, y));

            Ring.Add(new Coordinates(Origin.x - x, Origin.y - y));
            Ring.Add(new Coordinates(x, Origin.y - y));
        }

        foreach (Coordinates PathPoint in Ring)
        {
            Instantiate(Point, new Vector3(PathPoint.x, 0, PathPoint.y), transform.rotation, transform);
        }
    }

    public int GetNumberOfCoordinates()
    {
        return Ring.Count;
    }

    public Vector3 GetNextPosition(int index)
    {
        return new Vector3(Ring[index].x, 0, Ring[index].y);
    }
}


