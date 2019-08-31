using UnityEngine;
using UnityEngine.SceneManagement;


public class EndSceneLoad : MonoBehaviour {
    public int index;
    public void OnEnable() {
        SceneManager.LoadScene(index);
    }
}