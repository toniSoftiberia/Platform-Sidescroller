using UnityEngine;
using System.Collections;

public class SoulBlade : MonoBehaviour {

    public float speed = 300;

	void Update () {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.Self);
	}

    void OnTriggerEnter(Collider other) {
        if( other.tag == "Player") {
            other.GetComponent<Entity>().TakeDamage(10);
        }
    }
}
