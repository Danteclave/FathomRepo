using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private Transform playerRoot;

    [SerializeField]
    private Transform lookRoot;

    [SerializeField]
    private bool invert;

    [SerializeField]
    private bool canUnlock = true;

    [SerializeField]
    private float sensitivity = 5f;

    [SerializeField]
    private int smoothSteps = 10;

    [SerializeField]
    private float smoothWeight = 0.4f;

    [SerializeField]
    private float rollAngle = 3.0f; //10f

    [SerializeField]
    private float rollSpeed = 0.1f; //3f

    [SerializeField]
    private Vector2 defaultLookLimits = new Vector2(-70f, 80f);

    private Vector2 lookAngles;

    private Vector2 currentMouseLook;

    private Vector2 smoothMove;

    private float currentRollAngle = 0.0f;

    private int lastLookFrame;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        checkForInteractable();
        //LockAndUnlockCursor();
        if (!firstFrame && Cursor.lockState == CursorLockMode.Locked)
        {
            LookAround();
        }
        firstFrame = false;
    }

    bool firstFrame = true;

    void LockAndUnlockCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void LookAround()
    {
        if (!interactionLock)
        {
            currentMouseLook = new Vector2(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));

            lookAngles.x += currentMouseLook.x * sensitivity * (invert ? 1f : -1);
            lookAngles.y += currentMouseLook.y * sensitivity;

            lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimits.x, defaultLookLimits.y);

            currentRollAngle = Mathf.Lerp(currentRollAngle, Input.GetAxisRaw("Mouse X") * rollAngle, Time.deltaTime * rollSpeed);

            lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);
            playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
        }
    }

    public bool interactionLock = false;
    public bool interactionPossible = false;
    Interactable inter;

    public Vector3 startedGrabbing;

    public float grabCooldown = 0.1f;
    private bool canGrab = true;
    public IEnumerator GrabCooldown(bool thrown = false)
    {
        yield return new WaitForSeconds(grabCooldown * (thrown ? 8 : 1));
        canGrab = true;
    }

    private void checkForInteractable()
    {
        interactionPossible = false;
        if (inter == null ||  !inter.beingInteracted)
        {
            Vector3 fwd = transform.TransformDirection(Vector3.forward);
            if (Physics.Raycast(transform.position, fwd, out RaycastHit hit, 10))
            {
                var obj = hit.collider.gameObject;
                interactionPossible = obj.GetComponent<Interactable>() != null;
                if ((inter = obj.GetComponent<Interactable>()) != null)
                    startedGrabbing = hit.point;
            }
        }
        

        if(interactionPossible && inter != null && Input.GetButton("Fire1") && canGrab)
        {
            inter.grabLocation = startedGrabbing;
            inter.beingInteracted = true;
            interactionLock = inter.shouldLockScreen;

        }
        else if(inter != null && (Vector3.Distance(inter.transform.position, playerRoot.transform.position) > 12 || !Input.GetButton("Fire1")))
        {
            interactionLock = inter.beingInteracted = false;
            inter = null;
            canGrab = false;
            StartCoroutine(GrabCooldown());
        }
        else if(inter != null && Input.GetButton("Fire2"))
        {
            interactionLock = inter.beingInteracted = false;

            //inter.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward) * 60f;
            inter.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward) * inter.throwforce, ForceMode.Impulse);

            inter = null;
            canGrab = false;
            StartCoroutine(GrabCooldown(true));
        }
        hand.enabled = interactionPossible && !interactionLock;
    }

    [SerializeField]
    private Image hand;
}
