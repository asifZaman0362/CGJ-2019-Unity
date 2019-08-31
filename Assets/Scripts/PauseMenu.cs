using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
	
	public UnityEvent pauseEvents;
	public UnityEvent resumeEvents;
	
	private float timeScale;
	private float fixedDT;
	private bool isPaused = false;
	
	private void Update() {
		if (Input.GetButtonDown("Cancel")) {
			if (isPaused) Resume();
			else Pause();
		}
	}
	
	public void Pause() {
		timeScale = Time.timeScale;
		fixedDT = Time.fixedDeltaTime;
		Time.timeScale = 0;
		Time.fixedDeltaTime = 0;
		pauseEvents.Invoke();
		isPaused = true;
	}
	
	public void Resume() {
		Time.timeScale = timeScale;
		Time.fixedDeltaTime = fixedDT;
		resumeEvents.Invoke();
		isPaused = false;
	}
	
	public void Exit() {
		PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex);
		SceneManager.LoadScene(0);
	}
	
}
