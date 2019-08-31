using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameEnd : MonoBehaviour {

    public bool yea;
    public UnityEvent mEvent;
    public int next = 0;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator LevelCompleteUI;
    [SerializeField] private float zoomOutSpeed = 0.1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private Transform camera;
    [SerializeField] private Transform target;
    [SerializeField] private Transform world;
    [SerializeField] private Animator animator;
    public bool isGoodEnd = true;

    private MeshRenderer renderer;

    void Start() {
        renderer = GetComponent<MeshRenderer>();
    }

    void OnTriggerEnter(Collider coll) {
        if (isGoodEnd) {
            animator.enabled = true;
            SceneManager.LoadSceneAsync(10);
            return;
        }
        if (coll.gameObject.CompareTag("Player")) {
            coll.gameObject.GetComponent<PlayerController>().canMove = false;
            renderer.enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            if (isGoodEnd) StartCoroutine(MoveCamera());
            mEvent.Invoke();
        }
    }

    public IEnumerator MoveCamera() {
        camera.gameObject.GetComponent<CameraControllerMain>().enabled = false;
        while ((camera.transform.position-target.transform.position).magnitude >= 1) {
            camera.transform.position = Vector3.MoveTowards(camera.position, target.position, 5);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void Finish() {
        StartCoroutine(OnLevelComplete());
    }

    private IEnumerator OnLevelComplete() {
    	LevelCompleteUI.Play("Finish");
        float size = 1;
        while (size >= 1) {
            size = mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 0.5f, zoomOutSpeed);
            mainCamera.gameObject.transform.Rotate(Vector3.forward * rotationSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
        PlayerPrefs.SetInt("level", 0);
        SceneManager.LoadScene(next);
    }
}