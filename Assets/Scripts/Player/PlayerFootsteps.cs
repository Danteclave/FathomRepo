using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    private AudioSource footstepSound;

    [SerializeField]
    private AudioClip[] footstepClip;

    private CharacterController characterController;

    [HideInInspector]
    public float volumeMin;
    [HideInInspector]
    public float volumeMax;

    private float accumulatedDistance;

    [HideInInspector]
    public float stepDistance;

    void Awake()
    {
        footstepSound = GetComponent<AudioSource>();
        characterController = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(footstepClip.Length != 0)
            CheckToPlay();
    }

    void CheckToPlay()
    {
        if (characterController.isGrounded && characterController.velocity.sqrMagnitude > 0)
        {
            accumulatedDistance += Time.deltaTime;
            if (accumulatedDistance > stepDistance)
            {
                footstepSound.volume = Random.Range(volumeMin, volumeMax);
                footstepSound.clip = footstepClip[Random.Range(0, footstepClip.Length)];
                footstepSound.Play();
                accumulatedDistance -= stepDistance;
            }
        }
        else
        {
            accumulatedDistance = 0f;
        }
    }
}
