﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterController : MonoBehaviour {
    public float smoothTilt = 5.0f;
    public float tiltAngle = 5.0f;
    public float speed = 5.0F;
    public float jumpSpeed = 500f;
    public float health = 500f;
    public Rigidbody rb; //for za jumping
    private bool isJumping = false;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>(); //for za jumping
    }

    private void FixedUpdate() {
        if (rb.velocity.y == 0) {
            if (isJumping == true) {
                //if (rb.transform.position.y >= 1 && rb.transform.position.y <= 2)
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }
        }
        isJumping = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && (isJumping == false)) {
            isJumping = true;
        }
        float translation = Input.GetAxis("Vertical") * speed;
        float strafe = Input.GetAxis("Horizontal") * speed;
        translation *= Time.deltaTime;
        strafe *= Time.deltaTime;

        transform.Translate(strafe, 0, translation);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
    }
}