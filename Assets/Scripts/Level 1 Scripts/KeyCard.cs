using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Scanner")
        {
            GameEventSystem.Instance.KeyCardScanned();
        }

        if (collision.gameObject.tag == "LockedDoor")
        {
            Destroy(collision.gameObject);
        }
    }
}
