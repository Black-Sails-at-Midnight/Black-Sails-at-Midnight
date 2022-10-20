using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    Attack startWeapon;
    
    public Attack ActiveWeapon {get; private set;}
    public List<Attack> AllWeapons {get; private set;}
    
    private bool weaponsActive = true;
    

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
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            PreviousWeapon();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            NextWeapon();
        }
    }

    // Public Methods
    public void NextWeapon()
    {
        if (IsWeaponScoped(ActiveWeapon))
            return; 

        int activeIndex = GetWeaponIndex(ActiveWeapon);

        if (activeIndex + 1 > AllWeapons.Count - 1)
        {
            ActiveWeapon = AllWeapons[0];
        } else {
            ActiveWeapon = AllWeapons[activeIndex + 1];
        }

        SetWeaponsActive();
    }

    public void PreviousWeapon()
    {
        if (IsWeaponScoped(ActiveWeapon))
            return; 

        int activeIndex = GetWeaponIndex(ActiveWeapon);

        if (activeIndex - 1 < 0)
        {
            ActiveWeapon = AllWeapons[AllWeapons.Count - 1];
        } else {
            ActiveWeapon = AllWeapons[activeIndex - 1];
        }

        SetWeaponsActive();
    }

    public void ToggleWeaponRenderers(bool active)
    {
        foreach(Attack weapon in AllWeapons)
        {
            foreach (Renderer renderer in weapon.GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = active;
                ToggleWeaponFire(active);
            }
        }
    }

    public void ToggleWeaponFire(bool active)
    {
        foreach(Attack weapon in AllWeapons)
        {
            weapon.attackSettings.canAttack = active;
        }
    }

    // Private Methods
    private int GetWeaponIndex(Attack weapon)
    {
        return AllWeapons.IndexOf(weapon);
    }

    private void SetWeaponsActive()
    {
        foreach (Attack weapon in AllWeapons)
        {
            if (weapon == ActiveWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
        }
    }
    
    private bool IsWeaponScoped(Attack attack)
    {
        AimDownSight ADS = attack.GetComponent<AimDownSight>();

        if (ADS == null)
            return false;

        return ADS.scopedIn;
    }
}