using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public bool Enabled;
    public ButtonScript next = null;
    public Light childLight;
    public bool canInteract;
    // Start is called before the first frame update
    void Start()
    {
        canInteract = false;
        childLight = transform.GetChild(0).gameObject.GetComponent<Light>();
        GameEventSystem.Instance.OnGeneratorButtonPressed += CheckIfCorrectPressed;
        GameEventSystem.Instance.OnLightsTurnOn += TurnOnTheLight;
        GameEventSystem.Instance.OnGeneratorWrongOrder += ResetButton;
        GameEventSystem.Instance.OnFuelAdded += EnableButtons;
        childLight.enabled = false;
    }

    private void EnableButtons()
    {
        canInteract = true;
    }

    private void ResetButton()
    {
        childLight.enabled = false;
    }

    private void TurnOnTheLight()
    {
        childLight.enabled = false;
        childLight.color = Color.green;
        childLight.enabled = true;
    }

    private void CheckIfCorrectPressed()
    {
        if (Enabled)
        {
            if (next is null)
            {
                return;
            }
            if(next.childLight.enabled)
            {
                return;
            }
            StartCoroutine(EnableNextButtonCoroutine());
        }
    }

    private IEnumerator EnableNextButtonCoroutine()
    {
        childLight.color = Color.yellow;
        childLight.enabled = true;
        yield return new WaitForSeconds(1f);
        next.Enabled = true;
    }
}
