using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public GameObject player;

    private GameCamera cam;

	void Start () {
        cam = GetComponent<GameCamera>();

        SpawnPlayer();
    }


    private void SpawnPlayer() {
        GameObject go = (Instantiate(player, Vector3.zero, new Quaternion(0, 90, 0, 90)) as GameObject);
        cam.setTarget(go.transform);
    }
}
