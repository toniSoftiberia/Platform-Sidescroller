using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    public float speed = 2f;

	void Update () {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
	}
}
