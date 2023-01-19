using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContextClickable : MonoBehaviour
{
    protected void Setup()
    {
        playertransform = FindObjectOfType<PlayerController>().transform;
        gesystem = GameEventSystem.Instance;
        floattext = FindObjectOfType<FloatingTextOnHud>();
    }

    protected GameEventSystem gesystem;
    protected FloatingTextOnHud floattext;
    protected Transform playertransform;

    public string eventOnHover;
    public string eventOnClick;

    protected short InUpdate()
    {
        short result = 0;
        var cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, 10, ~LayerMask.GetMask("Player")))
        {
            if (eventOnHover != "" && hit.collider.gameObject == gameObject)
            {
                gesystem.callEvent(eventOnHover);
                result |= 1;
            }
            if (eventOnClick != "" && hit.collider.gameObject == gameObject)
            {
                if (Input.GetButton("Fire1"))
                {
                    gesystem.callEvent(eventOnClick);
                    result |= 2;
                }
            }
        }
        return result;
    }
}
