using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour {

    private NavMeshAgent robot;
    private SphereCollider los;
    public CharacterController player;
    private bool plrInSight;

    void Start() {
        robot = GetComponent<NavMeshAgent>();
        los = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other == player) {
            print("found player!");
            plrInSight = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other == player) {
            print("no player in sight...");
            plrInSight = false;
        }
    }


    void Update() {
        if (plrInSight == true) {
            robot.destination = player.transform.position;
        }
    }
}
