using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FuseStuffControllerScript : BaseScriptedControler
{
    public GameObject leftFuseSpot;
    public GameObject rightFuseSpot;

    public GameObject leftFuse;
    public GameObject rightFuse;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        gesystem.registerCallback("leftFuseTurnOn", e => fuseLeft());
        gesystem.registerCallback("rightFuseTurnOn", e => fuseRight());
        gesystem.registerCallback("leftCableConnect", e => cableLeft());
        gesystem.registerCallback("rightCableConnect", e => cableRight());
        gesystem.registerCallback("teleportToEnd", e => tele());
    }

    private void OnDestroy()
    {
        gesystem.removeCallback("leftFuseTurnOn");
        gesystem.removeCallback("rightFuseTurnOn");
        gesystem.removeCallback("leftCableConnect");
        gesystem.removeCallback("rightCableConnect");
        gesystem.removeCallback("teleportToEnd");
    }

    public void tele()
    {
        GetComponent<TeleportWzium>().GoodProgramming(FindObjectOfType<CharacterController>());
    }

    public GameObject leftCableSpot;
    public GameObject rightCableSpot;

    public GameObject leftCable;
    public GameObject rightCable;

    // could make this into functions with parameters but I do not care
    void cableLeft()
    {
        FindObjectOfType<HoldingScript>().dropObject();
        Destroy(leftCableCol.GetComponent<EventOnTriggerEnter>());
        leftCable.transform.position = leftCableSpot.transform.position;
        leftCable.transform.rotation = leftCableSpot.transform.rotation;
        Destroy(leftCable.GetComponent<InteractionScript>());
        Destroy(leftCable.GetComponent<Rigidbody>());
        cableleft = true;
    }

    public GameObject leftCableCol;
    public GameObject rightCableCol;

    void cableRight()
    {
        FindObjectOfType<HoldingScript>().dropObject();
        Destroy(rightCableCol.GetComponent<EventOnTriggerEnter>());
        rightCable.transform.position = rightCableSpot.transform.position;
        rightCable.transform.rotation = rightCableSpot.transform.rotation;
        Destroy(rightCable.GetComponent<InteractionScript>());
        Destroy(rightCable.GetComponent<Rigidbody>());
        cableright = true;
    }

    void fuseLeft()
    {
        if (leftFuse.activeSelf) return;
        leftFuse.SetActive(true);
        fuseGoodProgramming(leftFuseSpot);
        fuseleft = true;
    }

    void fuseRight()
    {
        if (rightFuse.activeSelf) return;
        rightFuse.SetActive(true);
        fuseGoodProgramming(rightFuseSpot);
        fuseright = true;
    }

    void fuseGoodProgramming(GameObject obj)
    {
        var l = GameObject.FindGameObjectsWithTag("Fuse");
        l = l.Where(e => e != leftFuse && e != rightFuse).ToArray();
        var closest = l[0];
        foreach(var x in l)
        {
            if((closest.transform.position - obj.transform.position).sqrMagnitude > (x.transform.position - obj.transform.position).sqrMagnitude)
            {
                closest = x;
            }
        }
        FindObjectOfType<HoldingScript>().dropObject();
        Destroy(closest);

    }

    bool opened = false;
    // Update is called once per frame
    void Update()
    {
        if(cableleft && cableright && fuseleft && fuseright)
        {
            if (!opened)
            {
                opened = true;
                manholecover.GetComponent<Animator>().SetTrigger("Open");
            }

        }
    }

    public GameObject manholecover;

    bool cableleft = false;
    bool cableright = false;
    bool fuseleft = false;
    bool fuseright = false;
}
