using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour, IDamageable {
    public float health;
    private Vector3 enemyDistance;

 //   private void Update() {
 //       if (enemy hit you) {}
  //          print("ow");
 //           Damage();
  //      }
    

    public void TakeDamage(float amount) {
        health -= 10.0f * Time.deltaTime;
        if (health < 1.0f) {
            Debug.LogError("YOU DIE");
        }
    }
}
