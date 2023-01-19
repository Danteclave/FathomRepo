using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbLock : MonoBehaviour
{
    public DumbBlock dumbBlock;
    private bool stopChecking = true;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Locked";
    }

    // Update is called once per frame
    void Update()
    {
        if (stopChecking)
        {
            Unlock();
        }
    }
    private void Unlock()
    {
        if (dumbBlock.unlock)
        {
            stopChecking = false;
            gameObject.tag = "Interactable";
        }
    }

 
}
