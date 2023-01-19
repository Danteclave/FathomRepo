using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbBlock : MonoBehaviour
{
    public bool unlock = false;
    public Interactable interactable;
    private bool stopChecking = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stopChecking)
        {
            OnInteracte();
        }
    }

    public void OnInteracte ()
    {
        if (interactable.dumbInteracted)
        {
            stopChecking = false;
            unlock = true;
        }
    }

        

    
    
}
