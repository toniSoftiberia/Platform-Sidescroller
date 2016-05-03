using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class PlayerPhisycs : MonoBehaviour {

    public LayerMask collisionMask;

    private BoxCollider collider;
    private Vector3 s;
    private Vector3 c;

    private Vector3 originalSize;
    private Vector3 originalCenter;
    private float colliderScale;

    private int collisionDivisionsX = 3;
    private int collisionDivisionsY = 10;

    public float groundSkin = .005f;
    public float sideSkin = .02f;

    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public bool movementStopped;

    Ray ray;
    RaycastHit hit;

    public void Start() {
        collider = GetComponent<BoxCollider>();
        collider.center = new Vector3(collider.center.x, collider.center.z + 1.16f, collider.center.z );
        colliderScale = transform.localScale.x;

        originalSize = collider.size;
        originalCenter = collider.center;
        SetCollider(originalSize, originalCenter);
    }


    public void Move(Vector2 moveAmmount) {

        float deltaY = moveAmmount.y;
        float deltaX = moveAmmount.x;
        Vector2 p = transform.position;

        grounded = false;
        for (int i = 0; i < collisionDivisionsX; ++i) {
            float dir = Mathf.Sign(deltaY);
            float x = (p.x + c.x - s.x / 2) + s.x / (collisionDivisionsX - 1) * i; // Left, centre and then rightmost point of collider
            float y = p.y + c.y + s.y / 2 * dir; // Bottom of collider

            ray = new Ray(new Vector2(x, y), new Vector2(0, dir));
            Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaY) + groundSkin, collisionMask)) {
                // Get Distance between player and ground
                float dst = Vector3.Distance(ray.origin, hit.point);

                // Stop player's downwards movement after coming within skin width of a collider
                if (dst > groundSkin) {
                    deltaY = dst * dir - groundSkin * dir;
                } else {
                    deltaY = 0;
                }

                grounded = true;

                break;
            }
        }


        //transform.Translate(moveAmmount);
        // Check collisions left and right
        //movementStopped = false;
        for (int i = 0; i < collisionDivisionsY; i++) {
            float dir = Mathf.Sign(deltaX);
            float x = p.x + c.x + s.x / 2 * dir;
            float y = p.y + c.y - s.y / 2 + s.y / (collisionDivisionsY - 1) * i;

            ray = new Ray(new Vector2(x, y), new Vector2(dir, 0));
            Debug.DrawRay(ray.origin, ray.direction);

            float skin = sideSkin;
            if (PlayerController.IsRunning())
                skin *= 10f;

            if (Physics.Raycast(ray, out hit, Mathf.Abs(deltaX) + skin, collisionMask)) {
                // Get Distance between player and ground
                float dst = Vector3.Distance(ray.origin, hit.point);

                // Stop player's downwards movement after coming within skin width of a collider
                if (dst > skin) {
                    deltaX = dst * dir - skin * dir;
                } else {
                    deltaX = 0;
                }

                movementStopped = true;
                break;

            } else {
                movementStopped = false;
            }
        }

        if (!grounded && !movementStopped) { 
            Vector3 playerDirection = new Vector3(deltaX, deltaY);
            Vector3 o = new Vector3(p.x + c.x + s.x / 2 * Mathf.Sign(deltaX), p.y + c.y + s.y / 2 * Mathf.Sign(deltaY));
            ray = new Ray(o, playerDirection.normalized);

            if (Physics.Raycast(ray, Mathf.Sqrt(deltaX * deltaX * deltaY * deltaY), collisionMask)) {
                grounded = true;
                deltaY = 0;
            }
        }

        Vector2 finalTransform = new Vector2(deltaX, deltaY);

        transform.Translate(finalTransform, Space.World);
    }

    public void SetCollider(Vector3 size, Vector3 center) {
        collider.size = size;
        collider.center = center;

        s = size * colliderScale;
        c = center * colliderScale;

    }
}
