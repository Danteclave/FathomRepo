using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    public int fuelAmount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private float tickDelay = .45f;
    public bool touchingRefuellingStation { get; private set; } = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (Incdefs.IsOfType<RefuellingStation>(collision.gameObject))
        {
            touchingRefuellingStation = true;
            StartCoroutine(LoadFuel());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (Incdefs.IsOfType<RefuellingStation>(collision.gameObject))
            touchingRefuellingStation = false;
    }

    private IEnumerator LoadFuel()
    {
        if (isLoadingFuel) yield break;
        isLoadingFuel = true;
        while(touchingRefuellingStation && fuelAmount < 100)
        {
            fuelAmount++;
            yield return new WaitForSeconds(tickDelay);
        }
        isLoadingFuel = false;
    }

    public bool isLoadingFuel { get; private set; } = false;
}
