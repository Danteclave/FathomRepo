using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorsScript : MonoBehaviour
{
    public ButtonScript[] buttons = new ButtonScript[4]; 
    private void Start()
    {
        ResetButtons();
        GameEventSystem.Instance.OnGeneratorWrongOrder += ResetButtons;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            foreach(ButtonScript b in buttons)
            {
                Debug.Log($"name: {b.name}, activation state: {b.Enabled}");
            }
        }
    }

    private void ResetButtons()
    {
        StartCoroutine(ResetCoroutine());
    }
    private IEnumerator ResetCoroutine()
    {
        foreach (ButtonScript b in buttons)
        {
            b.Enabled = false;
        }
        yield return new WaitForSeconds(2f);
        buttons[1].Enabled = true; //here set your first option to enabled
    }
}
