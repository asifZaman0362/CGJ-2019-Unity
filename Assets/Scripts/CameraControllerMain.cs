using UnityEngine;

public class CameraControllerMain : MonoBehaviour {
    public Transform player;
    public float yLimit  = 10;
    public float yTarget = 10;
    private Vector3 normalPos;

    public void Start() {
        normalPos = transform.position;
    }

    void Update() {
        if (player.position.y > yLimit) {
            transform.position = Vector3.MoveTowards (transform.position, Vector3.up * yLimit, 0.4f);
        }
        else if (transform.position.y != normalPos.y) {
            transform.position = Vector3.MoveTowards (transform.position, normalPos, 0.4f);
        }
    }
}