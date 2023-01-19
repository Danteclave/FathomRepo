using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1Door2ContextClickable : ContextClickable
{
    // Start is called before the first frame update
    void Start()
    {
        Setup();
        gesystem.registerCallback("generatorDoesNotOpen", d => generatorDoesNotOpen());
    }

    private void OnDestroy()
    {
        gesystem.removeCallback("generatorDoesNotOpen");
    }

    void generatorDoesNotOpen()
    {
        if(GetComponent<Rigidbody>().constraints != RigidbodyConstraints.None)
            floattext.StartDisplayingMiddleText("Does not open from this side.");
    }

    // Update is called once per frame
    void Update()
    {
        InUpdate();
    }
}
