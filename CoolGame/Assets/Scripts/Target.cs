using UnityEngine;
using UnityEngine.Audio;

public class Target : MonoBehaviour, IDamageable {
    public GameObject okVer;
    public GameObject destroyedVer;
    public AudioSource target;
    public AudioClip col;
    BoxCollider bc;
    public float health = 50f;

    private void OnCollisionEnter(Collision collision) {
        target.PlayOneShot(col);
    }
    private void OnTriggerEnter(Collider other) {
        if (other.name == ("Body")) {
            target.PlayOneShot(col);
        }
    }

    private void Start() {
        bc = GetComponent<BoxCollider>();
    }

    public void TakeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            Die();
        }
    }

    void Die() {
        if (destroyedVer != null) {
            destroyedVer.gameObject.SetActive(true);
            okVer.gameObject.SetActive(false);
            bc.enabled = false;
            Destroy(gameObject, 8);
            //Instantiate(destroyedVer, transform.position, transform.rotation);
        } else {
            Destroy(gameObject);
        }
    }
}

