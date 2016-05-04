using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

    public float trackSpeed = 10;
    public float offset = 0.1f;

    private Transform target;

    public void setTarget(Transform t) {
        target = t;
        transform.position = new Vector3(t.position.x, t.position.y, transform.position.z);
    }
    
    void Start() {
        transform.position = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);
    }

    void LateUpdate() {
        if(target) {
            float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
            float y = IncrementTowards(transform.position.y - offset, target.position.y, trackSpeed);

            transform.position = new Vector3(x, y + offset, transform.position.z);
        }
    }

    public float IncrementTowards(float x, float target, float e) {
        if (x == target)
            return x;
        else {
            float dir = Mathf.Sign(target - x); // must x be incremented or decremented to get closer to target
            x += e * Time.deltaTime * dir;
            return (dir == Mathf.Sign(target - x)) ? x : target;
        }
    }

}
