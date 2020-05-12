using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 5f;
    public float range = 100f;
    public float hitForce = 50f;
    public float fireRate = 2f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impact;

    private float nextFireTime = 0;
    public bool fullAuto = false;

    void Update()
    {
        
    }

    public void Shoot()
    {
        muzzleFlash.Play();
        nextFireTime = Time.time + 1f / fireRate;
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);
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
        fullAuto = !fullAuto;
    }

    public bool canFire()
    {
        return Time.time >= nextFireTime;
    }
}
