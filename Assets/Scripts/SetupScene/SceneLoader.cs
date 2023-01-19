using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        if (FindObjectOfType<RerouterToStart>() == null)
            GetComponent<TeleportWzium>().GoodProgramming(FindObjectOfType<CharacterController>());
    }
}
