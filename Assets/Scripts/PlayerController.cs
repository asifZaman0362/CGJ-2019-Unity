﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using static UnityEngine.Mathf;


[RequireComponent(typeof(CharacterController), typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

	private CharacterController controller;
	//private Rigidbody rigidbody;
	
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 10;
	[SerializeField] private float zTraverseSpeed = 2;
	[SerializeField] private float speedUpToMatch = 1.5f;
	[SerializeField] private float jumpSpeed = 10;
	[SerializeField] private float jumpRememberLimit = 1;
	[Header("Other parameters")]
	[SerializeField] private float gravityScale = 6.5f;
	[SerializeField] private float gravity = 4;
	[SerializeField] private float extentSize = 1.5f;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip jumpAudio;
	[Header("Plane Shifting")]
	[SerializeField] private float smoothSpeed = 0.1f;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private PostProcessProfile postProcess;
	[SerializeField] private Color col2;
	[SerializeField] private bool freeMove = false;
	//[SerializeField] private PostProcessVolume volume;
	
	private bool jumpRequested = false;
	private bool jump = false;
	private bool isGrounded = false;
	private bool isMoveRestricted = false;
	private float timer = 0;
	private float gravityMultiplier = 1;
	private bool isTraversing = false;
	private float slowmofactor = 10;
	private float zPos = 0;
	private bool keepMoving = false;
	private float zDir = 0;
	private float lastFrameInputZ = 0;
	private float targetPos = 0;
	private float timeScale;
	private float fixedDeltaTime;

	private Vector3 velocity = Vector3.zero;
	private Vector2 input = Vector2.zero;
	private ChromaticAberration chromme;
	private ColorGrading colorGrading;

	[HideInInspector] public bool ScaleOut = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
		targetPos = Floor(transform.position.z);
		chromme = ScriptableObject.CreateInstance<ChromaticAberration>();
		chromme.enabled.Override(true);
		chromme.intensity.Override(0.2f);
		postProcess.AddSettings(chromme);
		colorGrading = postProcess.GetSetting<ColorGrading>();
		//rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

		if (ScaleOut) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, 0.1f);
			return;
		}

		zPos = transform.position.z;
		isMoveRestricted = Physics.OverlapBox(transform.position, transform.localScale / extentSize - Vector3.up * 0.2f, Quaternion.identity, whatIsGround).Length > 0;

    	timer += Time.deltaTime;
    	
		input.x = Input.GetAxis("Horizontal") * moveSpeed;
		input.y = (Input.GetAxisRaw("Vertical")) * zTraverseSpeed / Time.timeScale;
		float inputZ = Input.GetAxisRaw("Vertical");
        float move = input.y;

		if (Abs(inputZ) <= 0 && !freeMove) {
            if (Abs(lastFrameInputZ) > 0) {
                if (isMoveRestricted) targetPos = RoundToInt(transform.position.z);
                else targetPos = lastFrameInputZ > 0 ? Ceil(transform.position.z) : Floor(transform.position.z);
                zDir = transform.position.z - targetPos;
            }
            float delta = transform.position.z - targetPos;
            zDir = -Sign(delta);
            if (Abs(delta) < 0.15f) {
                controller.Move(-Vector3.forward * delta);
                move = 0;
            }
            else move = zDir * moveSpeed * speedUpToMatch;
        }
		lastFrameInputZ = inputZ;
		//if (isMoveRestricted) { input.y = 0; }
		
		if (Abs(controller.velocity.z) > 0 && Abs(move) > 0) {
			slowmofactor -= smoothSpeed;
			slowmofactor = Clamp(slowmofactor, 1, 10);
			Time.timeScale = Clamp(Log10(slowmofactor), 0.3f, 1);
			Time.fixedDeltaTime = 0.2f * Time.timeScale;
			chromme.intensity.value = Mathf.Clamp(1 - Time.timeScale, 0.2f, 1);
			colorGrading.colorFilter.value = Color.Lerp(col2, Color.white, Log10(slowmofactor));
		}
		else if(Time.timeScale != 1) {
			slowmofactor += smoothSpeed;
			slowmofactor = Clamp(slowmofactor, 1, 10);
			Time.timeScale = Clamp(Log10(slowmofactor), 0.3f, 1);
			Time.fixedDeltaTime = 0.2f * Time.timeScale;
			chromme.intensity.value = Mathf.Clamp(1 - Time.timeScale, 0.2f, 1);
			colorGrading.colorFilter.value = Color.Lerp(col2, Color.white, Time.timeScale);
		}
        if (isMoveRestricted && !isGrounded) input.x = input.y = 0;
        
		if ((isGrounded || Input.GetButton("Jump")) && velocity.y >= 0) gravityMultiplier = gravity;
		else gravityMultiplier = gravityScale;

        if (Input.GetButtonDown("Jump")) {
        	timer = 0f;
			if (isGrounded) { jump = true; jumpRequested = false; audioSource.clip = jumpAudio; audioSource.Play(); }
			else jumpRequested = true;
        }
        else if (timer < jumpRememberLimit && jumpRequested && isGrounded) { jump = true; jumpRequested = false; audioSource.clip = jumpAudio; audioSource.Play(); }
        else jump = false;
        
        velocity = new Vector3(input.x, (jump && isGrounded) ? jumpSpeed : isGrounded ? 0 : velocity.y, move);
        velocity += !isGrounded ? Physics.gravity * gravityMultiplier * Time.deltaTime : Vector3.zero;

        CollisionFlags flags = controller.Move(velocity * Time.deltaTime);
		isGrounded = (flags & CollisionFlags.Below) == CollisionFlags.Below;
		//isMoveRestricted = (flags & CollisionFlags.Sides) == CollisionFlags.Sides;
        
    }

	private void OnDestroy() {
		postProcess.RemoveSettings<ChromaticAberration>();
	}

	private void OnTriggerEnter(Collider coll) {
		//if (coll.gameObject.layer == 13) {Debug.Log("Laevel"); Destroy(coll.gameObject);}
	}
    
}
