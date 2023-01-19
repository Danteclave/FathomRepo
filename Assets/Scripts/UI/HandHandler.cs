using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandHandler : MonoBehaviour
{
    public static Image image;
    public GameObject imageHolder;
    public static bool HandVisible;
    private Camera cam;
    void Start()
    {
        HandVisible = false;
        image = imageHolder.GetComponent<Image>();
        image.enabled = false;
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Physics.Raycast(ray, out RaycastHit hit, 15, ~LayerMask.GetMask("Player"));
        if (hit.collider != null && hit.collider.gameObject.GetComponent<InteractionScript>() != null && hit.distance <= 10.0f)
        {
            EnableHand();
        }
        else
        {
            DisableHand();
        }
    }
    public static void EnableHand()
    {
        image.enabled = true;
        HandVisible = true;
    }

    public static void DisableHand()
    {
        image.enabled = false;
        HandVisible = false;
    }
}
