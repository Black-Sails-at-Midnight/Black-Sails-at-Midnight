using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomySystem : MonoBehaviour
{
    [SerializeField]
    public int StartingGold = 10;
    [SerializeField]
    private int Gold {get; set;}


    private void Start()
    {
        Gold = StartingGold;
    }

    /// <summary>
    /// Withdraw Gold from the player's bank.
    /// </summary>
    /// <param name="amount">Amount of gold to withdraw.</param>
    /// <returns></returns>
    public bool Withdraw(int amount)
    {
        if (Gold < amount)
        {
            return false;
        }

        Gold -= amount;
        return true;
    }

    /// <summary>
    /// Deposit Gold into the player's bank.
    /// </summary>
    /// <param name="amount">Amount of gold to deposit.</param>
    /// <returns></returns>
    public bool Deposit(int amount)
    {
        if (amount < 0)
        {
            return false;
        }
        Gold += amount;
        return true;
    }
}

class Vault
{
    public bool Deposit(int value)
    {
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<EconomySystem>().Deposit(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot deposit less than 0!");
        }
        return true;
    }

    public bool Withdraw(int value)
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<EconomySystem>().Withdraw(value);
    }
}
