
using UnityEngine;

public class Target : MonoBehaviour {
    public GameObject destroyedVer;
    public float health = 50f;

    public void TakeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            Die();
        }
    }

    void Die() {
        Instantiate(destroyedVer, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}

