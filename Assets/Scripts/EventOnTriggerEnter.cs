using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnTriggerEnter : MonoBehaviour
{
    public string eventOnEnter;
    public string tagFilter = "Player";
    private GameEventSystem gesystem;
    // Start is called before the first frame update
    void Start()
    {
        gesystem = GameEventSystem.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool spicy = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(tagFilter))
        {
            gesystem.callEvent(eventOnEnter);
            if(spicy)
                Destroy(gameObject);
        }
    }
}
