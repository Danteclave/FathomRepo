using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBobbing : MonoBehaviour
{

    public float walkingBobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    private CharacterController cc;
    private PlayerController pc;
    private PlayerMovement pm;

    float defaultPosY = 0;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        cc = pc.cc;
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.walking != 0)
        {
   
            timer += Time.deltaTime * walkingBobbingSpeed * pm.walking;
            transform.localPosition = new Vector3(transform.localPosition.x,
                defaultPosY + Mathf.Sin(timer) * bobbingAmount,
                transform.localPosition.z);
        }
        else
        {
            timer = 0;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed),
                transform.localPosition.z);
        }
    }
}
