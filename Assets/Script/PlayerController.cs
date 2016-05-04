using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PlayerPhisycs))]
public class PlayerController : Entity {

    // Player handling
    public float gravity = 20;
    public float walkSpeed = 8;
    public float runSpeed = 12;
    public float acceleration = 30;
    public float jumpHeight = 12;
    public float slideDeceleration = 10;

    private float initiateSlideThreshold = 9;

    private float animationSpeed;

    private static float currentSpeed;
    private float targetSpeed;
    private Vector2 amountToMove;
    private float moveDirectionX;

    // States
    private bool jumping;
    private bool sliding;
    private bool wallHolding;
    private bool stopSliding;

    private PlayerPhisycs playerPhisics;
    private Animator animator;
    private GameManager manager;

    void Start () {
        playerPhisics = GetComponent<PlayerPhisycs>();
        animator = GetComponentInChildren<Animator>();
        manager = Camera.main.GetComponent<GameManager>();

        animator.SetLayerWeight(1, 1);
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

            if (wallHolding) {
                wallHolding = false;
                animator.SetBool("Wall Hold", false);
            }

            if (jumping) {
                jumping = false;
                animator.SetBool("Jumping", false);
            }

            if (sliding) {
                if(Mathf.Abs(currentSpeed) < .25f || stopSliding == true) {
                    stopSliding = false;
                    sliding = false;
                    animator.SetBool("Sliding", false);
                    playerPhisics.ResetCollide();
                }
            }

            // Slide Input
            if (Mathf.Abs(currentSpeed) > initiateSlideThreshold &&  Input.GetButtonDown("Slide")) {
                sliding = true;
                animator.SetBool("Sliding", true);
                targetSpeed = 0;
                playerPhisics.SetCollider(new Vector3(2f, 1.85f, 1f), new Vector3(0f, 0.915f, 0f));
            }
        }else if ( !wallHolding && playerPhisics.canWallHold) {
            wallHolding = true;
            animator.SetBool("Wall Hold", true);
        }

        // Jump Input
        if (Input.GetButtonDown("Jump")) {
            if (Input.GetButtonDown("Jump") && (playerPhisics.grounded || wallHolding)) {
                stopSliding = true;
                amountToMove.y = jumpHeight;
                jumping = true;
                animator.SetBool("Jumping", true);
                if (wallHolding) {
                    wallHolding = false;
                    animator.SetBool("Wall Hold", false);
                }
            }
        }

        // Animate it
        animationSpeed = IncrementTowards(animationSpeed, Mathf.Abs(targetSpeed),acceleration);
        animator.SetFloat("Speed", Mathf.Abs(animationSpeed));

        // Face Direction
        moveDirectionX = Input.GetAxisRaw("Horizontal");
        if (!sliding) {
            // Input
            float speed = (Input.GetButton("Run") ? runSpeed : walkSpeed);
            targetSpeed = moveDirectionX * speed;
            currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

            if (moveDirectionX != 0 && !wallHolding)
                transform.eulerAngles = (moveDirectionX > 0) ? Vector3.up * 90 : Vector3.up * -90;
        }else {
            currentSpeed = IncrementTowards(currentSpeed, targetSpeed, slideDeceleration);
        }

        // Set amount to move
        amountToMove.x = currentSpeed;

        if (wallHolding) {
            amountToMove.x = 0;
            if (Input.GetAxisRaw("Vertical") != -1) ;
            amountToMove.y = 0;
        }

        amountToMove.y -= gravity * Time.deltaTime;
        playerPhisics.Move(amountToMove * Time.deltaTime, moveDirectionX);
	}


    void OnTriggerEnter(Collider other) {
        if (other.tag == "CheckPoint") {
            manager.SetCheckpoint(other.transform.position);
        }
        if (other.tag == "Finish") {
            manager.EndLevel();
        }
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
