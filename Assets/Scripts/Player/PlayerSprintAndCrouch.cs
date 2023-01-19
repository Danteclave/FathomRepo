using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Rendering;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 3;
    public float proneSpeed = 1;

    private Transform lookRoot;
    public GameObject raySource;

    private float standHeightScale = 2f;
    private float standHeight = 6.7f;

    private float crouchHeightScale = 1f;
    private float crouchHeight = 3.65f;

    private float proneHeightScale = 0.3f;
    private float proneHeight = 1.55f;

    private float currentHeight = 0f;

    private PlayerFootsteps playerFootsteps;

    private float sprintVolume = 0.1f;
    private float crouchVolume = 0.0035f;
    private float walkVolumeMin = 0.05f;
    private float walkVolumeMax = 0.075f;
    private float walkStepDistance = 0.4f;
    private float sprintStepDistance = 0.25f;
    private float crouchStepDistance = 0.5f;

    private RaycastHit hit;

    public enum Stance
    {
        STANDING,
        CROUCHING,
        PRONE
    }
    public Stance stance = Stance.STANDING;

    // Start is called before the first frame update
    void Start()
    {
        playerFootsteps.volumeMin = walkVolumeMin;
        playerFootsteps.volumeMax = walkVolumeMax;
        playerFootsteps.stepDistance = walkStepDistance;
    }

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        lookRoot = transform.GetChild(0);
        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(new Ray(raySource.transform.position, Vector3.up), out hit);
        //Debug.Log($"{hit.distance}, {raySource.transform.position.y} , {hit.distance <standHeight - currentHeight}");
        Sprint();
        Crouch();
        Prone();
        var scale = transform.localScale;
        switch (stance)
        {
            case Stance.STANDING:
                //lookRoot.localPosition = new Vector3(0f, Mathf.Lerp(lookRoot.localPosition.y, standHeight, 10 * Time.deltaTime));
                //transform.localScale = new Vector3(1, 2.0f, 1);
                transform.localScale = new Vector3(1f, Mathf.Lerp(transform.localScale.y, standHeightScale, 10 * Time.deltaTime));
                currentHeight = standHeight;
                break;
            case Stance.CROUCHING:
                //lookRoot.localPosition = new Vector3(0f, Mathf.Lerp(lookRoot.localPosition.y, crouchHeight, 10 * Time.deltaTime));
                //transform.localScale = new Vector3(1, 1.0f, 1);
                transform.localScale = new Vector3(1f, Mathf.Lerp(transform.localScale.y, crouchHeightScale, 10 * Time.deltaTime));
                currentHeight = crouchHeight;
                break;
            case Stance.PRONE:
                //lookRoot.localPosition = new Vector3(0f, Mathf.Lerp(lookRoot.localPosition.y, proneHeight, 10 * Time.deltaTime));
                //transform.localScale = new Vector3(1, 0.3f, 1);
                transform.localScale = new Vector3(1f, Mathf.Lerp(transform.localScale.y, proneHeightScale, 10 * Time.deltaTime));
                currentHeight = proneHeight; 
                break;
        }
        if (transform.localScale.y > scale.y)
        {
            GetComponent<CharacterController>().enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y + 1.15f * (transform.localScale.y - scale.y), transform.position.z);
            GetComponent<CharacterController>().enabled = true;
        }
    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && stance == Stance.PRONE)
        {
            if (hit.distance > crouchHeight - currentHeight || hit.collider is null)
            {
                playerMovement.speed = crouchSpeed;
                stance = Stance.CROUCHING;
                playerFootsteps.volumeMin = crouchVolume;
                playerFootsteps.volumeMax = crouchVolume;
                playerFootsteps.stepDistance = crouchStepDistance;
            }
        }
        else
        {
            if (hit.distance > standHeight - currentHeight || hit.collider is null)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && stance != Stance.PRONE)
                {

                    playerMovement.speed = sprintSpeed;
                    playerFootsteps.stepDistance = sprintStepDistance;
                    playerFootsteps.volumeMin = sprintVolume;
                    playerFootsteps.volumeMax = sprintVolume;

                    stance = Stance.STANDING;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift) && stance != Stance.PRONE)
                {
                    playerMovement.speed = moveSpeed;
                    playerFootsteps.volumeMin = walkVolumeMin;
                    playerFootsteps.volumeMax = walkVolumeMax;
                    playerFootsteps.stepDistance = walkStepDistance;
                }
            }
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Space))
        {
            if (stance == Stance.CROUCHING)
            {
                if (hit.distance > standHeight - currentHeight || hit.collider is null)
                {
                    playerMovement.speed = moveSpeed;
                    stance = Stance.STANDING;
                    playerFootsteps.volumeMin = walkVolumeMin;
                    playerFootsteps.volumeMax = walkVolumeMax;
                    playerFootsteps.stepDistance = walkStepDistance;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (!(stance == Stance.PRONE && (hit.distance < crouchHeight - currentHeight || hit.collider is null)))
                {
                    playerMovement.speed = crouchSpeed;
                    stance = Stance.CROUCHING;
                    playerFootsteps.volumeMin = crouchVolume;
                    playerFootsteps.volumeMax = crouchVolume;
                    playerFootsteps.stepDistance = crouchStepDistance;
                }
            }
        }
    }

    void Prone()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (stance == Stance.PRONE)
            {
                if (hit.distance > standHeight - currentHeight || hit.collider is null)
                {
                    stance = Stance.STANDING;
                    playerMovement.speed = moveSpeed;
                    playerFootsteps.volumeMin = walkVolumeMin;
                    playerFootsteps.volumeMax = walkVolumeMax;
                    playerFootsteps.stepDistance = walkStepDistance;
                }
            }
            else
            {
                stance = Stance.PRONE;
                playerMovement.speed = proneSpeed;
                playerFootsteps.volumeMin = 0;
                playerFootsteps.volumeMax = 0;
                playerFootsteps.stepDistance = 0;
            }
        }
    }
}
