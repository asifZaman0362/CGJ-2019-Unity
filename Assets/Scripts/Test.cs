using UnityEngine;
using static UnityEngine.Mathf;

public class Test : MonoBehaviour {
    
    public CharacterController controller;
    public float speed = 2;
    public float speedUpToMatch = 2;
    public float extentSize = 1.5f;
    public LayerMask ground;

    private float target = 0;
    private float dir = 0;
    private float lastFrameInput = 0;
    private bool isMoveRestricted = false;

    void Update() {

        isMoveRestricted = Physics.OverlapBox(transform.position, (transform.localScale / extentSize) - Vector3.up * 0.1f, Quaternion.identity, ground).Length > 0;

        float input = Input.GetAxis("Vertical");
        float move = speed * input;

        if (Abs(input) <= 0) {
            if (Abs(lastFrameInput) > 0) {
                if (isMoveRestricted) target = RoundToInt(transform.position.z);
                else target = lastFrameInput > 0 ? Ceil(transform.position.z) : Floor(transform.position.z);
                dir = transform.position.z - target;
            }
            float delta = transform.position.z - target;
            dir = -Sign(delta);
            if (Abs(delta) < 0.1f) {
                controller.Move(-Vector3.forward * delta);
                move = 0;
            }
            else move = dir * speed * speedUpToMatch;
        }

        lastFrameInput = input;

        controller.Move(Vector3.forward * move * Time.deltaTime);

    }
}