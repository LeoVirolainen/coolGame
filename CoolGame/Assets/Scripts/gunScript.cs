using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using UnityEngine.UI;

public class gunScript : MonoBehaviour {

    public AudioClip fire;
    public AudioClip reload;
    public AudioSource gun;
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

    public Text ammoCount;

    private void Start()
    {
        currentAmmo = maxAmmo;
        SetAmmoCount();
    }

    private void OnEnable() //kutsutaan aina ku objekti enablataan
    {
        isReloading = false;                        //Jotta aseiden vaihtelu kesken reloadin
        animator.SetBool("Reloading", false);       //ei sotke kaikkea.
        SetAmmoCount();
    }

    private void Awake() {
        gun = GetComponent<AudioSource>();
        //reload = GetComponent<AudioSource>();
    }

    void Update() {

        if (isReloading)
            return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && currentAmmo > 0) //poista "getButtonDown":ista "Down" -nii ampuu full-auto
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

    }

    IEnumerator Reload()
    {
        gun.PlayOneShot(reload);

        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f); //*
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(.25f);  // *ettei voi ampua heti latauksen jälkeen, ku pysy vielä nousee.

        currentAmmo = maxAmmo;
        isReloading = false;
        SetAmmoCount();
    }

    void Shoot() {
        gun.PlayOneShot(fire);
        muzzleFlash.Play();

        currentAmmo--;
        SetAmmoCount();

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

    void SetAmmoCount() //ammukset UI:ssa
    {
        ammoCount.text = currentAmmo.ToString() + "/" + maxAmmo;
    }
}
