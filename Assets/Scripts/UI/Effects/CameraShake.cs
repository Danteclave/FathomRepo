using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class CameraShake : MonoBehaviour
{
    private Vignette vignette;
    public void Start()
    {
        GameEventSystem.Instance.OnPlayerTakeDamage += () => cameraShake();
    }

    public IEnumerator cameraShake()
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < 0.1f)
        {
            float x = Random.Range(-1f, 1f) * 1.5f;
            float y = Random.Range(-1f, 1f) * 0.25f;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
