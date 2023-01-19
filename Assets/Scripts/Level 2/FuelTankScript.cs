using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTankScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CanisterPosition;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Incdefs.IsOfType<FuelTank>(collision.gameObject))
        {
            StartCoroutine(addingCoroutine(collision.gameObject));
        }
    }

    private IEnumerator addingCoroutine(GameObject gameobj)
    {
        gameobj.GetComponent<Interactable>().enabled = false;
        gameobj.GetComponent<Rigidbody>().isKinematic = true;
        gameobj.transform.position = CanisterPosition.transform.position;
        gameobj.transform.rotation = CanisterPosition.transform.rotation;
        yield return new WaitForSeconds(5f);
        GameEventSystem.Instance.FuelAdded();
        Destroy(gameobj);
    }
}
