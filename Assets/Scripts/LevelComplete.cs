using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Events;

public class LevelComplete : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private Animator LevelCompleteUI;
    [SerializeField] private PostProcessProfile profile;
    [SerializeField] private PostProcessEffectSettings effectSettings;
    [SerializeField] private float zoomOutSpeed = 0.1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private UnityEvent _event;

    private void OnEnable() {
        if (mainCamera == null) {
            mainCamera = GameObject.FindObjectOfType<AudioListener>().GetComponent<Camera>();
        }
    }

    private void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.CompareTag("Player")) {
            StartCoroutine(OnLevelComplete());
        }
    }

    private IEnumerator OnLevelComplete() {
    	LevelCompleteUI.Play("Finish");
    	if (_event != null) _event.Invoke();
        float size = 1;
        while (size >= 1) {
            size = mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 0.5f, zoomOutSpeed);
            mainCamera.gameObject.transform.Rotate(Vector3.forward * rotationSpeed);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (SceneManager.sceneCount > index)
            SceneManager.LoadScene(index);
            Destroy(gameObject);
    }

}
