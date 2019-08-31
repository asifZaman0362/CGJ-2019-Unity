using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using TMPro;

public class TextDisplay : MonoBehaviour {

    public string[] text;
    public float speed = 0.05f;
    public TextMeshProUGUI textMeshProUGUI;
    public UnityEvent eventOnFinish;

    private int line = 0;
    private bool isSpeaking = false;
    private bool skip = false;
    private bool finished = false;

    private void Start() {
        StartCoroutine(Speak());
    }

    private void Update() {
        if (finished) return;
        if (Input.GetKeyDown(KeyCode.A)) {
            if (isSpeaking) {
                skip = true;
            } else StartCoroutine(Speak());
        }
    }

    private IEnumerator Speak() {
        textMeshProUGUI.text = "";
        if (line < text.Length) {
            string str = "";
            isSpeaking = true;
            foreach (char c in text[line].ToCharArray()) {
                if (skip) {
                    textMeshProUGUI.text = text[line];
                    skip = false;
                    break;
                }
                str += c;
                textMeshProUGUI.text = str;
                yield return new WaitForSeconds(speed);
            }
            isSpeaking = false;
            line++;
        } else {
            textMeshProUGUI.gameObject.transform.parent.gameObject.SetActive(false);
            if (eventOnFinish != null) eventOnFinish.Invoke();
            finished = true;
        }
        yield return null;
    }
    
}
