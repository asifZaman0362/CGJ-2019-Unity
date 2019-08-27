using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

	private Rigidbody rb;
	
	[Header("Movement")]
	[SerializeField] private float moveSpeed = 10;
	[SerializeField] private float zTraverseSpeed = 3;
	[SerializeField] private float jumpSpeed = 10;
	[SerializeField] private float jumpRememberLimit = 1;
	[Header("Ground check")]
	[SerializeField] private Vector3 groundCheckSize;
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private Transform groundCheckPosition;
	[Header("Other parameters")]
	[SerializeField] private float up0 = 10f;
	[SerializeField] private float up1 = 4f;
	[SerializeField] private float gravityScale = 6.5f;
	[SerializeField] private float gravity = 4;
	
	private bool jumpPressed = false;
	private bool isGrounded = false;
	private bool isMoveRestricted = false;
	private float timer = 0;
	private float gravityMultiplier = 1;

	private Vector3 axis = Vector3.right;
	private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
    
    	isGrounded = (Physics.OverlapBox(groundCheckPosition.position, groundCheckSize, Quaternion.identity, whatIsGround).Length > 0);
		//isMoveRestricted = (Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.49f, 0.5f), Quaternion.identity, whatIsGround).Length > 0);
        velocity = transform.right * Input.GetAxis("Horizontal") * moveSpeed + Vector3.up * (jumpPressed && isGrounded ? jumpSpeed : rb.velocity.y) + transform.forward * Input.GetAxis("Vertical") * zTraverseSpeed;
        //if (isMoveRestricted && !isGrounded) velocity.x = 0;
		if (Input.GetButton("Jump")) {
			if (rb.velocity.y > 0) gravityMultiplier = gravity;
			else gravityMultiplier = gravityScale;
		}
		else gravityMultiplier = gravityScale;
        if (Input.GetButtonDown("Jump")) {
        	timer = 0f;
        	jumpPressed = true;
        }
        else if (timer < jumpRememberLimit) jumpPressed = true;
        else jumpPressed = false;
        
    }
    
    void FixedUpdate() {
    
    	timer += Time.fixedDeltaTime;
    	rb.velocity = velocity;
		rb.velocity += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
    
    }
    
}
