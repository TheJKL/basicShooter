using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 5f;
    public float range = 100f;
    public float hitForce = 50f;
    public float fireRate = 2f;
    public bool fireModeSwitch;
    public bool fullAuto; // Defines what state the gun starts in (full or semi auto)

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;

    private float nextFireTime = 0;


    public void Shoot()
    {
        muzzleFlash.Play();
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
        return Time.time >= nextFireTime;
    }
}
