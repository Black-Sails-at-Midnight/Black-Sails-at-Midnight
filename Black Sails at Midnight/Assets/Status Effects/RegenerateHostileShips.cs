using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateHostileShips : MonoBehaviour
{
    [SerializeField]
    float Health;

    [SerializeField]
    float TimeDelay;

    [SerializeField]
    List<GameObject> ShipsInRange;

    bool isRegenerating = false;

    private void Update()
    {
       if (!isRegenerating)
       {
            StartCoroutine(RegenerateShipsHealth());
       }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            ShipsInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            ShipsInRange.Remove(other.gameObject);
        }
    }

    IEnumerator RegenerateShipsHealth()
    {
        isRegenerating = true;
        foreach (var item in ShipsInRange)
        {
            item.GetComponent<ShipHealth>().Heal(Health);
        }
        yield return new WaitForSeconds(TimeDelay);
        isRegenerating = false;
    }
}
