using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IDeposit
{
    public bool Payout(int value)
    {
        if(!GameObject.FindGameObjectWithTag("Player").GetComponent<EconomySystem>().Deposit(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot deposit less than 0!");
        }
        return true;
    }
}