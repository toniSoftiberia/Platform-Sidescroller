using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject player;

    private GameObject currentPlayer;
    private GameCamera cam;
    private Vector3 checkpoint;

    public static int levelCount = 2;
    public static int currentLevel = 0;


    void Start () {
        cam = GetComponent<GameCamera>();

        if (GameObject.FindGameObjectWithTag("Spawn"))
            checkpoint = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        else
            checkpoint = Vector3.zero;

        SpawnPlayer(checkpoint);
    }


    private void SpawnPlayer(Vector3 spawnPos) {
        currentPlayer = (Instantiate(player, spawnPos, new Quaternion(0, 90, 0, 90)) as GameObject);
        cam.setTarget(currentPlayer.transform);
    }

    private void Update() {
        if (!currentPlayer && Input.GetButtonDown("Respawn"))
            SpawnPlayer(checkpoint);
    }

    public void SetCheckpoint(Vector3 point) {
        checkpoint = point;
    }

    public void EndLevel() {
        if(currentLevel < levelCount) {
            ++currentLevel;
            SceneManager.LoadScene("Level " + currentLevel);
        }else {
            Debug.Log("Game Over");
        }
    }
}
