using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportWzium : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField]
    private Vector3 spot;
    [SerializeField]
    private Quaternion rot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var x = other.GetComponent<CharacterController>();
        if(x != null)
            GoodProgramming(x);
    }

    public void GoodProgramming(CharacterController cc)
    {
        cc.enabled = false;
        cc.gameObject.transform.position = spot + new Vector3(0, 0.25f, 0);
        cc.gameObject.transform.rotation = rot;
        cc.GetComponent<CharacterController>().enabled = true;
        SceneManager.LoadScene(sceneName);
    }
}
