using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screwdriver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -50)
        {
            transform.position = new Vector3(-51.2739983f, 34.3100014f, -63.1721268f);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Ventdoor")
        {

            Destroy(collision.gameObject);
        }
    }
}
