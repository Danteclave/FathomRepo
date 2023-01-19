using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingTextOnHud : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI mainText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDisplayingMiddleText(string text)
    {
        mainText.SetText(text);
        if (fadeMainText != null)
        {
            StopCoroutine(fadeMainText);
        }
        fadeMainText = StartCoroutine(FadeEnvelope(mainText));
    }

    private Coroutine fadeMainText;

    private IEnumerator FadeEnvelope(TextMeshProUGUI tx)
    {
        mainText.enabled = true;
        tx.color = SetAlphaRet(tx.color, 0.0f);
        float t = 0.00f;
        float s = 0.05f;
        while ((tx.color = SetAlphaRet(tx.color, t+=s)).a < 1.0f)
        {
            yield return new WaitForSeconds(0.01f);
        }

        tx.color = SetAlphaRet(tx.color, 1.0f);
        yield return new WaitForSeconds(3.0f);

        float f = 0.01f;
        while ((tx.color = SetAlphaRet(tx.color, t -= f)).a > 1.0f)
        {
            yield return new WaitForSeconds(0.01f);
        }
        tx.color = SetAlphaRet(tx.color, 0.0f);
        mainText.enabled = false;
    }

    private static Color SetAlphaRet(Color c, float a)
    {
        c.a = a;
        return c;
    }
}
