using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    private void Start()
    {
        soundSource = GetComponent<SpicySoundSource>();
    }

    void Update()
    {
        if (swoshNoise)
            StartCoroutine(playEnemySound());
    }

    private SpicySoundSource soundSource;
    private IEnumerator playEnemySound()
    {
        swoshNoise = false;
        soundSource.Play("swosh swosh");
        yield return new WaitForSeconds(Random.Range(0, 100) <= 5 ? 0.5f : Random.Range(3, 10));
        swoshNoise = true;
    }
    bool swoshNoise = true;

}
