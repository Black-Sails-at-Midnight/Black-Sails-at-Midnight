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

    public bool IsSame(Coordinates other)
    {
        if (other.x == this.x && other.y == this.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetDistance(Coordinates other)
    {
        float distance = (other.y - other.y) / (this.x - other.x);
        if (distance < 0)
        {
            distance *= -1;
        }
        return distance;
    }
}

public class RingSystem : MonoBehaviour
{
    [SerializeField]
    public float Radius = 3;
    [SerializeField]
    public Coordinates Origin;
    [SerializeField]
    [Range(0, 90)]
    public int NumberOfPathingPoints = 6;

    [SerializeField]
    public List<Coordinates> Ring;

    [SerializeField]
    GameObject Point;

    public void GenerateRing()
    {
        Ring = new List<Coordinates>();

        List<Coordinates> TopLeft, BottomLeft, BottomRight, TopRight;
        TopLeft = new List<Coordinates>();
        BottomLeft = new List<Coordinates>();
        BottomRight = new List<Coordinates>();
        TopRight = new List<Coordinates>();

        Radius = Mathf.Sqrt(Radius);
        NumberOfPathingPoints /= 4;

        for (int index = 0; index < NumberOfPathingPoints + 1; index++)
        {
            float x = Origin.x + (Radius / NumberOfPathingPoints * index);

            if (Origin.x == x)
            {
                TopLeft.Add(new Coordinates(x, Radius));
                BottomRight.Add(new Coordinates(x, x - Radius));
            }
            else if (x == Radius)
            {
                TopRight.Add(new Coordinates(x, Origin.y));
                BottomLeft.Add(new Coordinates(x - Radius * 2, Origin.y));
            }
            else
            {
                float y = Mathf.Sqrt(Mathf.Pow(Radius, 2) - Mathf.Pow(x, 2)) - Mathf.Pow(Origin.x, 2) + Mathf.Pow(Origin.y, 2);

                TopLeft.Add(new Coordinates(x, y));
                BottomLeft.Add(new Coordinates(Origin.x - x, y));

                BottomRight.Add(new Coordinates(Origin.x - x, Origin.y - y));
                TopRight.Add(new Coordinates(x, Origin.y - y));
            }
        }

        TopRight.Reverse();
        BottomLeft.Reverse();

        AddToList(Ring, TopLeft);
        AddToList(Ring, TopRight);
        AddToList(Ring, BottomRight);
        AddToList(Ring, BottomLeft);

        foreach (Coordinates PathPoint in Ring)
        {
            Instantiate(Point, new Vector3(PathPoint.x, 0, PathPoint.y), transform.rotation, transform);
        }
    }

    private void AddToList(List<Coordinates> destination, List<Coordinates> source)
    {
        foreach (var item in source)
        {
            destination.Add(item);
        }
        source.Clear();
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


