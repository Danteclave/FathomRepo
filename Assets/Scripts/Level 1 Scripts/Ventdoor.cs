using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ventdoor : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.layer == 10)
        {
            Debug.Log("co");
        }
        if (collision.gameObject.tag == "Screwdriver")
        {
            Debug.Log("usuwanko");
           Destroy(gameObject);
        }
    }
}
