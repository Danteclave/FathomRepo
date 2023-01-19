using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseScriptedControler : MonoBehaviour
{
    protected GameEventSystem gesystem;
    protected SoundManager snManager;

    protected void Setup()
    {
        gesystem = GameEventSystem.Instance;
        snManager = FindObjectOfType<SoundManager>();
    }
}
