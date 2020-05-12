using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 5f;
    public float range = 100f;
    public float hitForce = 50f;
    public float fireRate = 2f;
    public float magSize;
    public float reloadTime = 2f;
    public bool fireModeSwitch;
    public bool fullAuto; // Defines what state the gun starts in (full or semi auto)

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;
    public Animator animator;

    private float nextFireTime = 0;
    [HideInInspector]
    public float currentAmmo;
    [HideInInspector]
    public bool reloading = false;

    void Start()
    {
        currentAmmo = magSize;
    }

    void OnEnable()
    {
        reloading = false;
        animator.SetBool("Reloading", false);
    }

    void Update()
    {
        if(currentAmmo == 0 && !reloading)
        {
            StartCoroutine(reload());
        }
    }

    public IEnumerator reload()
    {
        reloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);


        currentAmmo = magSize;
        reloading = false;
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo -= 1;

        nextFireTime = Time.time + 1f / fireRate;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.Damage(damage);
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
            
            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }

    public void switchFireMode()
    {
        if (fireModeSwitch)
        {
            fullAuto = !fullAuto;
        }
    }

    public bool canFire()//TODO include reloading in the checks 
    {
        return Time.time >= nextFireTime && currentAmmo > 0 && !reloading;
    }
}
