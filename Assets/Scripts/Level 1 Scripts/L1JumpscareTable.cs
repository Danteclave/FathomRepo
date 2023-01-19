using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1JumpscareTable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float timestamp = -1;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
            timestamp = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Time.time - timestamp > 1f)
            {
                GameEventSystem.Instance.callEvent("cleanupScare");
            }
        }
    }
}
