using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [System.Serializable]
    public class AttackSettings {
        public float damage;
        public float damageMultiplier = 1;

        public bool canAttack = true;
    }

    [SerializeField]
    public AttackSettings attackSettings;
}
