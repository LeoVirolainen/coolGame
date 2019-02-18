using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {
    private void Start() {
        Destroy(gameObject, 4);
    }
}