using UnityEngine;
using UnityEngine.SceneManagement;


public class DeadZone : MonoBehaviour {
    private Vector3 playerPos;
    public bool isEnd = false;
    private void Start() {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
    private void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.CompareTag("Player")) {
            if(isEnd) {SceneManager.LoadScene(0); return; }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}