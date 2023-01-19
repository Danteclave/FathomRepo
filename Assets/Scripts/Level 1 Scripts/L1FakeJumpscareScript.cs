using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class L1FakeJumpscareScript : BaseScriptedControler
{
    // Start is called before the first frame update
    void Start()
    {
        Setup();
        gesystem.registerCallback("startFakeJumpscareLevel1", d => startJumpScare());
        gesystem.registerCallback("continueScare1", d => continueScare());
        gesystem.registerCallback("cleanupScare", d => cleanupScare());

        //snManager.soundObjects.Add(new())
    }

    private void OnDestroy()
    {
        gesystem.removeCallback("startFakeJumpscareLevel1");
        gesystem.removeCallback("continueScare1");
        gesystem.removeCallback("cleanupScare");
    }

    public GameObject ventToBlock;
    public GameObject keycard;
    public GameObject doorToLock;
    public GameObject[] planks;

    //private short stage = 0;
    public short stage = 0;

    // Update is called once per frame
    void Update()
    {
        // this really doesn't need to be made with the event system
        // but I wanna see if it works
        if (stage == 0)
        {
            if (keycard.GetComponent<Rigidbody>().velocity.magnitude > 0.01f && Time.timeSinceLevelLoad > 17f)
            {
                gesystem.callEvent("startFakeJumpscareLevel1");
            }
        }
    }
    public void startJumpScare()
    {
        stage++;
        ventToBlock.SetActive(true);
    }

    public void continueScare()
    {
        StartCoroutine(continueScareCoroutine());
        if(stage == 0)
            FindObjectOfType<FloatingTextOnHud>().StartDisplayingMiddleText("The door seems to be blocked.");
    }

    public IEnumerator continueScareCoroutine()
    {
        if (stage == 1)
        {
            snManager.PlayMusic("fakeoutscare");
            stage++;
            doorToLock.GetComponent<SpicySoundSource>().Play("first");
            yield return new WaitForSeconds(2);
            doorToLock.GetComponent<SpicySoundSource>().Play("second");
            yield return new WaitForSeconds(2);
            doorToLock.GetComponent<SpicySoundSource>().Play("third");
            yield return new WaitForSeconds(2);
            stage++;
        }
    }

    public void cleanupScare()
    {
        if (stage == 3)
        {
            stage++;
            ventToBlock.SetActive(false);

            doorToLock.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            planks[0].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            planks[1].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            doorToLock.GetComponent<Rigidbody>().AddForce(new Vector3(15, 2, 5), ForceMode.Impulse);
            planks[0].GetComponent<Rigidbody>().AddForce(new Vector3(15, 2, -10), ForceMode.Impulse);
            planks[1].GetComponent<Rigidbody>().AddForce(new Vector3(15, 2, 20), ForceMode.Impulse);

            doorToLock.GetComponent<SpicySoundSource>().Play("final");

            // todo: swap for other music or disable entirely
            snManager.PlayMusic("SampleAmbient");
        }
    }
}
