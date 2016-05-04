using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BoxCollider))]
public class BoxGizmos : MonoBehaviour {

    public Color gizmosColor;

    void OnDrawGizmos() {
        Gizmos.color = gizmosColor;
        Gizmos.DrawCube(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);
    }
}
