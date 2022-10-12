using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWeaponHandler : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI weaponNameField;
    
    [SerializeField]
    public GameObject inChamberIndicator;

    private WeaponManager weaponManager;

    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponManager.ActiveWeapon as Gun != null)
        {
            weaponNameField.text = weaponManager.ActiveWeapon.name ?? "";

            if ((weaponManager.ActiveWeapon as Gun).clip > 0)
            {
                inChamberIndicator.SetActive(true);
            } else {
                inChamberIndicator.SetActive(false);
            }
        }

        
    }
}
