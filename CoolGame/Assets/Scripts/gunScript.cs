using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class gunScript : MonoBehaviour {

    public AudioSource fire;
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash; //pyssyn muzzle flash
    public GameObject impactEffect; //osuman part. effect

    private float nextTimeToFire = 0f; //for full-auto

    public Animator animator;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    private void OnEnable() //kutsutaan aina ku objekti enablataan
    {
        isReloading = false;                        //Jotta aseiden vaihtelu kesken reloadin
        animator.SetBool("Reloading", false);       //ei sotke kaikkea.
    }

    private void Awake() {
        fire = GetComponent<AudioSource>();
    }

    void Update() {

        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire) //poista "getButtonDown":ista "Down" -nii ampuu full-auto
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f); //*
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);  // *ettei voi ampua heti latauksen jälkeen, ku pysy vielä nousee.

        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot() {
        fire.Play();
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(-hit.normal * impactForce); //täräyttää targetia
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)); //liittyy impactin particleseihin
            Destroy(impactGO, 2f); //tuhoo "impact" -assetit 2 sek. päästä
        }
    }
}
