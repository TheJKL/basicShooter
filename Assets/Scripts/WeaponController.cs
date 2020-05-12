using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public GameObject primary;
    public GameObject secondary;

    private Weapon primaryWeapon;
    private Weapon secondaryWeapon;

    private GameObject currentWeaponGO;
    private Weapon currentWeapon;

    void Start()
    {
        primaryWeapon = primary.GetComponent<Weapon>();
        secondaryWeapon = secondary.GetComponent<Weapon>();

        currentWeaponGO = primary;
        currentWeapon = primaryWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        if ((!currentWeapon.fullAuto && Input.GetButtonDown("Fire1") || currentWeapon.fullAuto && Input.GetButton("Fire1")) && currentWeapon.canFire())
        {
            currentWeapon.Shoot();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            currentWeapon.switchFireMode();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != primaryWeapon)
        {
            setEquip(primary, primaryWeapon);
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != secondaryWeapon)
        {
            setEquip(secondary, secondaryWeapon);
        }

        if (Input.GetKeyDown(KeyCode.R) && !currentWeapon.reloading && currentWeapon.currentAmmo < currentWeapon.magSize)
        {
            StartCoroutine(currentWeapon.reload());
        }
    }

    private void setEquip(GameObject weaponGO, Weapon weapon)
    {
        currentWeaponGO.SetActive(false);//deactivate old weapon GO
        weaponGO.SetActive(true);//activate new weapon GO

        currentWeapon = weapon;//set weapon and weaponGO variables
        currentWeaponGO = weaponGO;
    }
}
