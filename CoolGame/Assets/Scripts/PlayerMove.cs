using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public string horizontalInputName;
    public string verticalInputName;
    public float movementSpeed;

    private CharacterController charController;

    public Animation anim;

    public AnimationCurve jumpFalloff;
    public float jumpMultiplier;
    public KeyCode jumpKey;
    private bool isMoving;

    private bool isJumping;

    private void Start() {
        anim = gameObject.GetComponentInChildren<Animation>();
    }

    private void Awake() {
        charController = GetComponent<CharacterController>();
    }

    private void Update() {
        PlayerMovement();
        if (gameObject.GetComponent<CharacterController>().velocity.z > 0) {
            isMoving = true;
        } else {
            isMoving = false;
            print("not moving");
        }

        if (gameObject.GetComponent<CharacterController>().velocity.x > 0) {
            isMoving = true;
        } else {
            isMoving = false;
            print("not moving in x");
        }

        if (isMoving == true) {
            //anim.Play("Bobbing");
            print("I'm moving. Should bob.");
        }
    }

    private void PlayerMovement() {

        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(forwardMovement + rightMovement);

        JumpInput();
    }

    private void JumpInput() {
        if (Input.GetKeyDown(jumpKey) && !isJumping) {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent() {
        charController.slopeLimit = 90.0f; //tämä estää glitchauksen seinää vasten hypätessä
        float timeInAir = 0.0f;

        do {
            float jumpForce = jumpFalloff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }
}

