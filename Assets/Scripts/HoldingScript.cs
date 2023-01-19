using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingScript : MonoBehaviour
{
    public GameObject holdArea;
    private InteractionScript iscript;
    private bool canGrab;
    private GameObject heldObj;
    private Rigidbody heldRb;
    private Camera cam;
    //raycasthit for finding an interactable object
    private RaycastHit hit;
    //raycasthit for detecting a collision while holding an object
    private RaycastHit hit2;
    private GameObject player;
    private LayerMask originalLayer;
    public float distanceFromCamera = 5;
    public float minDistancePrecentage = 0.5f;

    void Start()
    {
        cam = Camera.main;
        canGrab = true;
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    void Update()
    {
        //positioning the holding area transform
        holdArea.transform.position = cam.transform.position + cam.transform.forward * distanceFromCamera;

        if (heldObj is null)
        {
            Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
            Physics.Raycast(ray, out hit, 15, ~LayerMask.GetMask("Player"));
            InteractionScript iscript = null;

            if (hit.collider is not null)
            {
                iscript = hit.collider.GetComponent<InteractionScript>();
            }

            if (hit.collider is not null && iscript is not null && iscript.Holdable && hit.distance < 10.0f)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && canGrab)
                {
                    pickupObject(hit.collider.gameObject);
                }
            }
        }
        if (heldObj is not null)
        {
            moveObject();
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                dropObject();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                throwObject();
            }
        }
    }

    private void moveObject()
    {
        if (Physics.Linecast(cam.transform.position, holdArea.transform.position, out hit2, ~LayerMask.GetMask("HeldObject", "Player")))
        {
            float dis = Vector3.Distance(cam.transform.position, hit2.point);
            //stopping the object when colliding with an another object
            var targetPosition = hit2.point - (hit2.point - cam.transform.position).normalized * (distanceFromCamera * 2);
            
            //not moving the collision interaction objects behind the player
            if (heldObj.GetComponent<InteractionScript>().IsInteractionObject)
            {
                targetPosition = hit2.point;
            }

            if (dis / distanceFromCamera < minDistancePrecentage)
            {
                targetPosition = Vector3.Lerp(cam.transform.position, holdArea.transform.position, minDistancePrecentage);
            }

            heldObj.transform.position = Vector3.Lerp(heldObj.transform.position, targetPosition, Time.deltaTime * 10f);
        }
        else
        {
            heldObj.transform.position = Vector3.Lerp(heldObj.transform.position, holdArea.transform.position, Time.deltaTime * 10f);
        }

        heldObj.transform.rotation = cam.transform.rotation;
    }

    private void pickupObject(GameObject gameObject)
    {
        heldObj = gameObject;
        heldObj.transform.position = gameObject.transform.position;
        heldRb = gameObject.GetComponent<Rigidbody>();
        heldRb.useGravity = false;
        if (heldObj.gameObject.TryGetComponent<DistanceFromCameraComponent>(out var component))
        {
            distanceFromCamera = component.overrideDefaultDistance;
        }
        else
        {
            distanceFromCamera = 5;
        }
        originalLayer = heldObj.layer;
        heldObj.layer = 10;
        //Preventing flying on objects by holding them underneath the player
        Physics.IgnoreCollision(player.GetComponent<CharacterController>(), heldObj.GetComponent<Collider>(), ignore: true);
    }

    public void dropObject()
    {
        if (heldRb == null) return;
        heldRb.useGravity = true;
        heldObj.layer = originalLayer;
        Physics.IgnoreCollision(player.GetComponent<CharacterController>(), heldObj.GetComponent<Collider>(), ignore: false);
        heldRb = null;
        heldObj = null;
        StartCoroutine(grabCooldown());
    }

    private void throwObject()
    {
        heldRb.useGravity = true;
        heldObj.layer = originalLayer;
        Physics.IgnoreCollision(player.GetComponent<CharacterController>(), heldObj.GetComponent<Collider>(), ignore: false);
        heldRb.AddForce(cam.transform.forward * 50, ForceMode.Impulse);
        heldRb = null;
        heldObj = null;
        StartCoroutine(grabCooldown());
    }

    //simple coroutine to prevent flying on objects like in half-life
    private IEnumerator grabCooldown()
    {
        canGrab = false;
        yield return new WaitForSeconds(0.5f);
        canGrab = true;
    }
}
