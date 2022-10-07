using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    Attack startWeapon;

    [Header("Weapon Cycling")]
    [SerializeField]
    public KeyCode nextWeapon;

    [SerializeField]
    public KeyCode previousWeapon;

    
    public Attack ActiveWeapon {get; private set;}
    public List<Attack> AllWeapons {get; private set;}
    

    // Monobehaviour Methods
    public void Start() 
    {
        AllWeapons = new(/*GetComponentsInChildren<Attack>()*/);

        foreach(Transform child in transform)
        {
            AllWeapons.Add(child.GetComponent<Attack>());
            child.gameObject.SetActive(false);
        }
        AllWeapons.Reverse();

        if (startWeapon != null)
            ActiveWeapon = AllWeapons[GetWeaponIndex(startWeapon)];
        else
            ActiveWeapon = AllWeapons[0];
        
        ActiveWeapon.gameObject.SetActive(true);
    }

    public void Update() 
    {
        if (Input.GetKeyDown(nextWeapon))
        {
            NextWeapon();
        }

        if (Input.GetKeyDown(previousWeapon))
        {
            PreviousWeapon();
        }
    }

    // Public Methods
    public void NextWeapon()
    {
        int activeIndex = GetWeaponIndex(ActiveWeapon);

        if (activeIndex + 1 > AllWeapons.Count - 1)
        {
            ActiveWeapon = AllWeapons[0];
        } else {
            ActiveWeapon = AllWeapons[activeIndex + 1];
        }

        EnableWeapons();
    }

    public void PreviousWeapon()
    {
        int activeIndex = GetWeaponIndex(ActiveWeapon);

        if (activeIndex - 1 < 0)
        {
            ActiveWeapon = AllWeapons[AllWeapons.Count - 1];
        } else {
            ActiveWeapon = AllWeapons[activeIndex - 1];
        }

        EnableWeapons();
    }

    // Private Methods
    private int GetWeaponIndex(Attack weapon)
    {
        return AllWeapons.IndexOf(weapon);
    }

    private void EnableWeapons()
    {
        foreach (Attack weapon in AllWeapons)
        {
            if (weapon == ActiveWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
        }
    }
}
