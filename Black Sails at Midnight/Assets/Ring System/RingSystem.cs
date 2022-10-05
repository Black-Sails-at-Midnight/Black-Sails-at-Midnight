using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

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

    [Header("Ship Settings")]
    [SerializeField]
    int ShipCapacity;
    [SerializeField]
    List<ShipNavigationAI> Ships;
    [SerializeField]
    List<ShipNavigationAI> ShipsToUpdate;
    bool isCheckingForNullShips = false;

    bool isSyncing = false;

    private void Start()
    {
        Ships = new List<ShipNavigationAI>(ShipCapacity);
        ShipsToUpdate = new List<ShipNavigationAI>();
    }

    private void Update()
    {
        if (ShipsToUpdate.Count != 0)
        {
            foreach (var item in ShipsToUpdate)
            {
                UpdateShipNavigation(item);
            }
            ShipsToUpdate.Clear();
        }

        if (!isCheckingForNullShips)
        {
            StartCoroutine(CheckForNullShips());
        }
    }

    public void GenerateRing()
    {
        Ring = new List<Coordinates>();

        List<Coordinates> TopLeft, BottomLeft, BottomRight, TopRight;
        TopLeft = new List<Coordinates>();
        BottomLeft = new List<Coordinates>();
        BottomRight = new List<Coordinates>();
        TopRight = new List<Coordinates>();

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

    public void AddShip(ShipNavigationAI ship)
    {
        foreach (var item in Ships)
        {
            if (item == ship)
            {
                return;
            }
        }
        Ships.Add(ship);
        CalculateSyncSpeed();
    }

    public void RemoveFromList(ShipNavigationAI ship)
    {
        if (ship != null)
        {
            return;
        }
        Ships.Remove(ship);
        CalculateSyncSpeed();
    }

    public IEnumerator CheckForNullShips()
    {
        if (Ships.RemoveAll(x => x == null) > 0)
        {
            CalculateSyncSpeed();
        }
        yield return new WaitForSeconds(30);
    }

    void UpdateShipNavigation(ShipNavigationAI ship)
    {

        if (isSyncing)
        {
            ship.ResetAgentSpeed();
        }
        switch (ship.direction)
        {
            case ShipNavigationAI.Direction.ClockWise:
                ClockWise(ship);
                break;
            case ShipNavigationAI.Direction.Counter_Clockwise:
                CounterClockWise(ship);
                break;
            default:
                Debug.Log("Invalid value in direction!");
                break;
        }
    }

    public void ClockWise(ShipNavigationAI ship)
    {
        if (Ring.Count <= ship.CurrentPosition)
        {
            ship.CurrentPosition = 0;
        }
        else
        {
            ship.CurrentPosition++;
        }

        ship.destination = GetNextPosition(ship.CurrentPosition);
        ship.agent.SetDestination(ship.destination);

    }

    public void CounterClockWise(ShipNavigationAI ship)
    {
        if (ship.CurrentPosition <= 0)
        {
            ship.CurrentPosition = Ring.Count;
        }
        else
        {
            ship.CurrentPosition--;
        }

        ship.destination = GetNextPosition(ship.CurrentPosition);
        ship.agent.SetDestination(ship.destination);
    }

    public void TriggerDestinationUpdate(ShipNavigationAI ship)
    {
        ShipsToUpdate.Add(ship);
    }

    public bool CheckIfWaitingForUpdate(ShipNavigationAI ship)
    {
         return ShipsToUpdate.Find(x => x == ship) != null;
    }

    public void CalculateSyncSpeed()
    {
        float segment = Ring.Count / Ships.Count;

        Ships.Sort(CompareShipLocation);

        foreach (ShipNavigationAI item in Ships)
        {
            Debug.Log("Point: " + item.CurrentPosition);
        }
        float AverageDistance = 0f;

        for (int i = 0; i < Ships.Count; i++)
        {
            Ships[i].SetDestination(GetNextPosition(i * (int)segment));
            Ships[i].CurrentPosition =  i * (int)segment;

            AverageDistance += Ships[i].agent.remainingDistance;
        }

        AverageDistance /= Ships.Count;

        foreach (var item in Ships)
        {
            item.SetAgentSpeed(MapValue(item.agent.remainingDistance, 0f, AverageDistance, item.baseSpeed * 0.5f, item.baseSpeed * 3));
        }
    }

    public int CompareShipLocation(ShipNavigationAI a, ShipNavigationAI b)
    {
        if(a.CurrentPosition > b.CurrentPosition)
        {
            return -1;
        }
        else if(a.CurrentPosition < b.CurrentPosition)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
    }
}


