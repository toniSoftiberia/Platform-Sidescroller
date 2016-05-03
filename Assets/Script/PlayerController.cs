using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PlayerPhisycs))]
public class PlayerController : MonoBehaviour {

    // Player handling
    public float gravity = 20;
    public float walkSpeed = 6;
    public float runSpeed = 12;
    public float acceleration = 30;
    public float jumpHeight = 12;

    private float animationSpeed;

    private static float currentSpeed;
    private float targetSpeed;
    private Vector2 amountToMove;

    private PlayerPhisycs playerPhisics;
    private Animator animator;

    void Start () {
        playerPhisics = GetComponent<PlayerPhisycs>();
        animator = GetComponentInChildren<Animator>();

    }

	void Update () {
        // Reset acceleration upon collision
        if (playerPhisics.movementStopped) {
            targetSpeed = 0;
            currentSpeed = 0;
        }

        // If player is touching the ground
        if (playerPhisics.grounded) {
            amountToMove.y = 0;

            // Jump
            if (Input.GetButtonDown("Jump")) {
                amountToMove.y = jumpHeight;
            }
        }

        // Animate it
        animationSpeed = IncrementTowards(animationSpeed, Mathf.Abs(targetSpeed),acceleration);
        animator.SetFloat("Speed", Mathf.Abs(animationSpeed));


        // Input
        float speed = (Input.GetButton("Run") ? runSpeed : walkSpeed);
        targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
        currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

        // Set amount to move
        amountToMove.x = currentSpeed;
        amountToMove.y -= gravity * Time.deltaTime;
        playerPhisics.Move(amountToMove * Time.deltaTime);

        // Face Direction
        float moveDir = Input.GetAxisRaw("Horizontal");

        if(moveDir != 0)
            transform.eulerAngles = (moveDir > 0) ? Vector3.up * 90 : Vector3.up * -90;
	}


    // increments a toward target by e
    public float IncrementTowards(float x, float target, float e) {
        if (x == target)
            return x;
        else {
            float dir = Mathf.Sign(target - x); // must x be incremented or decremented to get closer to target
            x += e * Time.deltaTime * dir;
            return (dir == Mathf.Sign(target - x)) ? x : target;
        }
    }

    public static bool IsRunning() {
        return (currentSpeed > 6);
    }
}
