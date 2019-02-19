
using UnityEngine;

public class Target : MonoBehaviour {
    public GameObject okVer;
    public GameObject destroyedVer;
    BoxCollider bc;
    public float health = 50f;

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
            //Instantiate(destroyedVer, transform.position, transform.rotation);
        }
        Destroy(gameObject, 8);
    }
}

