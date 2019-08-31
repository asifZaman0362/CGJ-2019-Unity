using UnityEngine;
using System.Collections;

public class SceneStart : MonoBehaviour {
    
    Camera mainCam;
    public float speedZoom = 1f;

    private void Start() {
        mainCam = GetComponent<Camera>();
        StartCoroutine(ZoomIn());
    }

    private IEnumerator ZoomIn() {
        float size = 1f;
        while (size < 10) {
            size += speedZoom;
            mainCam.orthographicSize = size;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}