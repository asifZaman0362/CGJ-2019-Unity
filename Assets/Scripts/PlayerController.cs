using UnityEngine;
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
	[Header("Plane Shifting")]
	[SerializeField] private float smoothSpeed = 0.1f;
	[SerializeField] private LayerMask whatIsGround;
	
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

	private Vector3 velocity = Vector3.zero;
	private Vector2 input = Vector2.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
		targetPos = Floor(transform.position.z);
		//rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {

		zPos = transform.position.z;
		isMoveRestricted = Physics.OverlapBox(transform.position, transform.localScale / extentSize - Vector3.up * 0.2f, Quaternion.identity, whatIsGround).Length > 0;

    	timer += Time.deltaTime;
    	
		input.x = Input.GetAxis("Horizontal") * moveSpeed / Time.timeScale;
		input.y = (Input.GetAxisRaw("Vertical")) * zTraverseSpeed / Time.timeScale;
		float inputZ = Input.GetAxisRaw("Vertical");
        float move = input.y;

		if (Abs(inputZ) <= 0) {
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
			Time.timeScale = Clamp(Log10(slowmofactor), 0.1f, 1);
			Time.fixedDeltaTime = 0.2f * Time.timeScale;
		}
		else if(Time.timeScale != 1) {
			slowmofactor += smoothSpeed;
			slowmofactor = Clamp(slowmofactor, 1, 10);
			Time.timeScale = Clamp(Log10(slowmofactor), 0.1f, 1);
			Time.fixedDeltaTime = 0.2f * Time.timeScale;
		}
        if (isMoveRestricted && !isGrounded) input.x = input.y = 0;
        
		if ((isGrounded || Input.GetButton("Jump")) && velocity.y >= 0) gravityMultiplier = gravity;
		else gravityMultiplier = gravityScale;

        if (Input.GetButtonDown("Jump")) {
        	timer = 0f;
			if (isGrounded) { jump = true; jumpRequested = false; }
			else jumpRequested = true;
        }
        else if (timer < jumpRememberLimit && jumpRequested && isGrounded) { jump = true; jumpRequested = false; }
        else jump = false;
        
        velocity = new Vector3(input.x, (jump && isGrounded) ? jumpSpeed : isGrounded ? 0 : velocity.y, move);
        velocity += !isGrounded ? Physics.gravity * gravityMultiplier * Time.deltaTime : Vector3.zero;

        CollisionFlags flags = controller.Move(velocity * Time.deltaTime);
		isGrounded = (flags & CollisionFlags.Below) == CollisionFlags.Below;
		//isMoveRestricted = (flags & CollisionFlags.Sides) == CollisionFlags.Sides;
        
    }

	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawCube(transform.position, transform.localScale * 1.01f);
		Gizmos.color = Color.white;
	}

	private void OnTriggerEnter(Collider coll) {
		if (coll.gameObject.layer == 13) {Debug.Log("Laevel"); Destroy(coll.gameObject);}
	}
    
}
