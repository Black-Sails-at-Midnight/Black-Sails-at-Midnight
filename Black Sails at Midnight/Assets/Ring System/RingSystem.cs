using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
    public int ShipCapacity;
    [SerializeField]
    public List<ShipNavigationAI> Ships;
    [SerializeField]
    List<ShipNavigationAI> ShipsToUpdate;

    [SerializeField]
    public bool disableMarker = false;
    
    public bool DisableMarker 
    {
        set {
            disableMarker = value;
            GetComponentInChildren<RingMarkerHandler>().gameObject.SetActive(!disableMarker);
        }
        get {
            return disableMarker;
        }
    }

    bool isCheckingForNullShips = false;

    bool isSyncing = false;

    private void Start()
    {

    }

    public void SetupShipList(int capacity)
    {
        ShipCapacity = capacity;
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

    public bool AddShip(ShipNavigationAI ship)
    {
        if (Ships.Capacity == Ships.Count)
        {
            return false;
        }

        foreach (var item in Ships)
        {
            if (item == ship)
            {
                return false;
            }
        }
        Ships.Add(ship);
        StartCoroutine(CalculateSyncSpeed());
        return true;
    }

    public bool isFull()
    {
        return Ships.Capacity == Ships.Count;
    }

    public void RemoveFromList(ShipNavigationAI ship)
    {
        if (ship != null)
        {
            return;
        }
        Ships.Remove(ship);
        StartCoroutine(CalculateSyncSpeed());
    }

    public IEnumerator CheckForNullShips()
    {
        if (Ships.RemoveAll(x => x == null) > 0)
        {
            StartCoroutine(CalculateSyncSpeed());
        }
        yield return new WaitForSeconds(30);
    }

    public void ForceSync()
    {
        StartCoroutine(CalculateSyncSpeed());
    }

    void UpdateShipNavigation(ShipNavigationAI ship)
    {

        if (isSyncing)
        {
            ship.ResetAgentSpeed();
            ship.AccurateNavigation(false);
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
                break;
        }
    }

    public void ClockWise(ShipNavigationAI ship)
    {
        if (Ring.Count - 1 <= ship.CurrentPosition)
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
            ship.CurrentPosition = Ring.Count - 1;
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

    public IEnumerator CalculateSyncSpeed()
    {
        if (Ships.Count == 0)
        {
            yield return new WaitForEndOfFrame();
        } else {
            float segment = (Ring.Count - 1) / Ships.Count;

            int closestPoint = 0;
            float shortestDistance = Vector3.Distance(gameObject.transform.position, GetNextPosition(0));
            for (int i = 0; i < Ring.Count - 1; i++)
            {
                float distance = Vector3.Distance(gameObject.transform.position, GetNextPosition(i));

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPoint = i;
                }
            }
            Ships[Ships.Count - 1].CurrentPosition = closestPoint;

            Ships.Sort(CompareShipLocation);

            float longestDistance = 0f;
            for (int i = 0; i < Ships.Count; i++)
            {
                Ships[i].SetDestination(GetNextPosition(i * (int)segment));
                Ships[i].CurrentPosition =  i * (int)segment;
                Ships[i].AccurateNavigation(true);

                while(Ships[i].agent.pathPending)
                {
                    yield return new WaitForEndOfFrame();
                }

                if (Ships[i].agent.remainingDistance > longestDistance)
                {
                    longestDistance = Ships[i].agent.remainingDistance;
                }
            }

            foreach (var item in Ships)
            {
                float speed = Mathf.Clamp(MapValue(item.agent.remainingDistance, 0f, longestDistance, 0, item.baseSpeed * 2), 0, item.baseSpeed * 2);
                item.SetAgentSpeed(speed);
            }

            isSyncing = true;
        }
    }

    public int CompareShipLocation(ShipNavigationAI a, ShipNavigationAI b)
    {
        if(a.CurrentPosition > b.CurrentPosition)
        {
            return 1;
        }
        else if(a.CurrentPosition < b.CurrentPosition)
        {
            return -1;
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

    public int GetClosestPoint(Vector3 other)
    {
        int closestPoint = 0;
        float currentClosestDistance = Mathf.Infinity;

        for (int i = 0; i < Ring.Count; i++)
        {
            if(Vector3.Distance(this.GetNextPosition(i),other) < currentClosestDistance)
            {
                closestPoint = i;
            }
        }

        if (closestPoint == 0)
        {
            closestPoint = Ring.Count - 1;
        }

        return closestPoint--;
    }
}