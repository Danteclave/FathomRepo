using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonTeleport : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private Vector3 spot;
    [SerializeField]
    private Quaternion rot;
    public GameObject other;
    Camera cam;
    RaycastHit hit;
    public bool canInteract;
    public bool isScanned;
    // Start is called before the first frame update
    void Start()
    {
        canInteract = false;
        cam = Camera.main;
        GameEventSystem.Instance.OnKeyCardScanned += ActivateButton;
        GameEventSystem.Instance.OnLightsTurnOn += EnableButton;
    }

    private void EnableButton()
    {
        isScanned = true;
        canInteract = true;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        Physics.Raycast(ray, out hit);
        //todo display a hand when in reach
        ButtonTeleport bt = null;
        if (hit.collider is not null)
        {
            bt = hit.collider.gameObject.GetComponent<ButtonTeleport>();
        }
        if (hit.collider is not null && bt is not null && hit.distance < 5.0)
        {
            Debug.Log($"{bt.canInteract}, {bt.isScanned}");
        }
        if(hit.collider is not null && bt is not null && hit.distance<5.0 && bt.canInteract && bt.isScanned)
        {
            Debug.Log("hot");
            if (Input.GetMouseButtonDown(0))
            {
                teleporte();
            }
        }

    }

    private void ActivateButton()
    {
        isScanned = true;
    }


    private void teleporte()
    {
        other.GetComponent<CharacterController>().enabled = false;
        other.gameObject.transform.position = spot;
        other.gameObject.transform.rotation = rot;
        other.GetComponent<CharacterController>().enabled = true;
        SceneManager.LoadScene(sceneName);
    }
}
