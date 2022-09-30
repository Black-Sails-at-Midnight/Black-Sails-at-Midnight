using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IWithdraw
{
    public bool Withdraw(int value)
    {
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<EconomySystem>().Withdraw(value))
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Cannot withdraw less than 0");
        }
        return true;
    }
}

