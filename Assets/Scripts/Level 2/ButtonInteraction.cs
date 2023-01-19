using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteraction : MainButtonScript
{
    Camera cam;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        getRayCast<ButtonScript>(cam, 5.0f, lightsChanger);
    }

    private void lightsChanger(ButtonScript bs)
    {
        if (bs.canInteract)
        {
            if (bs.next is null && bs.Enabled)
            {
                GameEventSystem.Instance.LightsTurnOn();
            }
            else if (!bs.Enabled)
            {
                GameEventSystem.Instance.ButtonReset();
            }
            GameEventSystem.Instance.ButtonActivated();
        }
    }
}
