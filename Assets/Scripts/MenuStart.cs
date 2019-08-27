using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MenuStart : MonoBehaviour
{

	private Camera mainCamera;
	
	[SerializeField] private Color beginColor;
	[SerializeField] private Color endColor;
	[SerializeField] private float lerpSpeed = 0.1f;
	
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        if (!mainCamera.orthographic) mainCamera.orthographic = true;
    }

    // Update is called once per frame
    void Update()
    {
    	mainCamera.backgroundColor = Color.Lerp(mainCamera.backgroundColor, endColor, lerpSpeed);
    }
}
