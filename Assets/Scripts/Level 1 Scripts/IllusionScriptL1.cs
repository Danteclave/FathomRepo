using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionScriptL1 : EnemyAI
{
    public GameObject goal1;
    public GameObject goal2;
    public GameObject door;

    public int phase = 0;
    protected override void Start()
    {
        base.Start();
        agent.SetDestination(goal1.transform.position);
        state = State.Script;
    }
    protected override void Script()
    {
        base.Script();
        if (agent.remainingDistance <= 1.5f)
        {
            if (phase == 0)
            {
                agent.SetDestination(goal2.transform.position);
                door.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                door.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0.1f, -10f), ForceMode.Impulse);
                door.GetComponent<SpicySoundSource>().Play("doorbashopen");
                door.GetComponent<InteractionScript>().Holdable = true;
                phase++;
            }
        }
        if (phase == 1 && agent.remainingDistance <= 0.2)
        {
            Destroy(gameObject);
        }
    }
}
