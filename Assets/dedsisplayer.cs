using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dedsisplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public RawImage rawImage;
    public GameObject flashlight;
    // Update is called once per frame
    void Update()
    {

    }
    

    public void startInsanity()
    {
        FindObjectOfType<PlayerController>().enabled = false;
        FindObjectOfType<CharacterController>().enabled = false;
        FindObjectOfType<PlayerMovement>().enabled = false;
        StartCoroutine(FadeEnvelope());
        flashlight.SetActive(false);
        Time.timeScale = 0.0f;
    }

    private IEnumerator FadeEnvelope()
    {
        rawImage.gameObject.SetActive(true);
        rawImage.color = SetAlphaRet(rawImage.color, 0.0f);
        Debug.Log(rawImage.color);
        float t = 0.00f;
        float s = 0.05f;
        while ((rawImage.color = SetAlphaRet(rawImage.color, t += s)).a < 1.0f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            Debug.Log(rawImage.color);
        }

        rawImage.color = SetAlphaRet(rawImage.color, 1.0f);
        yield return new WaitForSecondsRealtime(5.0f);
        Debug.Log(rawImage.color);

        float f = 0.01f;
        while ((rawImage.color = SetAlphaRet(rawImage.color, t -= f)).a > 0.0f)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            Debug.Log(rawImage.color);
        }
        rawImage.color = SetAlphaRet(rawImage.color, 0.0f);
        Debug.Log(rawImage.color);
        Time.timeScale = 1.0f;
        Application.Quit();
    }

    private static Color SetAlphaRet(Color c, float a)
    {
        c.a = a;
        return c;
    }
}
