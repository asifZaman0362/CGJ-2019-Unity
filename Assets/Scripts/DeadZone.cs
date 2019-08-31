using UnityEngine;

public class DeadZone : MonoBehaviour {
    private Vector3 playerPos;
    private void Start() {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.CompareTag("Player")) {
            playerPos.z = coll.gameObject.transform.position.z;
            coll.gameObject.transform.position = playerPos;
        }
    }
}