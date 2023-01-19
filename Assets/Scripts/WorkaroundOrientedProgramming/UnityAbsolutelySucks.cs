using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityAbsolutelySucks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetTrigger("DoorToOpen");
        GetComponent<Animator>().ResetTrigger("DoorToClose");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
