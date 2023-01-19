using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMovement : MonoBehaviour
{
    private bool _isGrounded;
    public bool isGrounded
    {
        get { return _isGrounded; }
        set
        {
            if(_isGrounded != value)
            {
                _isGrounded = value;
                if(!value) ledgeForgiveness = Time.time;
            }
            
        }
    }


    private PlayerSprintAndCrouch playerSprintAndCrounch;

    private CharacterController characterController;

    private Vector3 moveDirection;

    public float speed = 5f;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float jumpForce = 5f;
    private float verticalVelocity;

    private SoundManager sn;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerSprintAndCrounch = GetComponent<PlayerSprintAndCrouch>();
        sn = FindObjectOfType<SoundManager>();
        gravity = -Physics.gravity.y; //minus because I cannot be bothered
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * 5.0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        Move();
    }

    public float ledgeForgiveness = 0.0f;

    void Move()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        walking = 0;
        if (moveDirection.magnitude > 0.01f && characterController.isGrounded)
        {
            if (playerSprintAndCrounch.stance == PlayerSprintAndCrouch.Stance.STANDING)
            {
                walking = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
            }
        }

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed * Time.deltaTime;
        ApplyGravity();
        characterController.Move(moveDirection);
    }

    public int walking = 0;

    void ApplyGravity()
    {
        var yeag = false;
        if (playerSprintAndCrounch.stance != PlayerSprintAndCrouch.Stance.PRONE)
        {
            yeag = Jump();
        }
        verticalVelocity -= gravity * Time.deltaTime;

        if (characterController.isGrounded && !yeag)
        {
            verticalVelocity = -0.2f;
        }

        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    bool Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || Time.time - ledgeForgiveness <= 0.1)
            {
                verticalVelocity = jumpForce;

                sn.Play(gameObject, "jump");
                return true;
            }
        }
        return false;
    }
}
