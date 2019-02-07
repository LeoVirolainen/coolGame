
using UnityEngine;

public class gunScript : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash; //pyssyn muzzle flash
    public GameObject impactEffect; //osuman part. effect

    private float nextTimeToFire = 0f; //for full-auto

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) //poista "getButtonDown":ista "Down" -nii ampuu full-auto
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

    }

    void Shoot()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce); //täräyttää targetia
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); //liittyy impactin particleseihin
            Destroy(impactGO, 2f); //tuhoo "impact" -assetit 2 sek. päästä
        }
    }
}
