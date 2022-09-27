using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public void Hit(GameObject source, Attack attack);
    public void Hit(float damage);
}
