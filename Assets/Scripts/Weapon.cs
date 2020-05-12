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
        reloading = false;//these lines prevent the gun from locking up if switched before reloading
        animator.SetBool("Reloading", false);
    }

    void Update()
    {
        if(currentAmmo == 0 && !reloading)//auto reloading
        {
            StartCoroutine(reload());
        }
    }

    public IEnumerator reload()
    { //reload coroutine so it can be paused
        reloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);// wait for reload minus an offset time for the animation to complete
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);//wait for animationt to complete


        currentAmmo = magSize;//reset ammo count
        reloading = false;
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        currentAmmo -= 1;

        nextFireTime = Time.time + 1f / fireRate;//fire rate control
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();

            if(target != null)//chechk that whatever the raycast hit was a target
            {
                target.Damage(damage);//damages the target
            }

            if(hit.rigidbody != null)//if object has rigid body 
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);//add hitforce
            }
            
            GameObject impactGO = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));//create hit particles
            Destroy(impactGO, 1f);
        }
    }

    public void switchFireMode()
    {
        if (fireModeSwitch)//if weapon has ability to switch fire modes
        {
            fullAuto = !fullAuto;
        }
    }

    public bool canFire()//checks if the weapon can fire
    {
        return Time.time >= nextFireTime && currentAmmo > 0 && !reloading;
    }
}
