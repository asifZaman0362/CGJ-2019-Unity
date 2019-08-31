using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GoToNextScene : MonoBehaviour {
	
	public int nextSceneIndex = 1;
	public UnityEvent _event;
	public bool yes = false;

	void Update() {
		if (yes) GotoScene(nextSceneIndex);
	}

	public void GotoScene(int index = -1) {
		if (index != -1) SceneManager.LoadScene(index);
		else SceneManager.LoadScene(nextSceneIndex);
	}
	
	public void Continue() {
		nextSceneIndex = PlayerPrefs.GetInt("level", 0);
		if (nextSceneIndex == 0) { _event.Invoke(); return; } 
		GotoScene(nextSceneIndex);
	}
	
	public void ExitGame() {
		Application.Quit();
	}
}
