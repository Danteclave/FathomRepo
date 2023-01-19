using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoorCtxClickable : ContextClickable
{
    // Start is called before the first frame update
    void Start()
    {
        eventOnClick = "lockedtraindoor";
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        InUpdate();
    }
}
