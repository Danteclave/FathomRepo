using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RerouterToStart : MonoBehaviour
{
    string currentScene;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        /*if (EditorApplication.isPlaying && FindObjectOfType<PlayerController>() == null)
        {
            currentScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene("SetupScene");

            StartCoroutine(WorkaroundDispatch());
        }*/
    }

    IEnumerator WorkaroundDispatch()
    {
        yield return new WaitForSeconds(0.1f);
        var other = FindObjectOfType<PlayerController>();
        other.GetComponent<CharacterController>().enabled = false;
        other.gameObject.transform.position = transform.position + new Vector3(0, 0.25f, 0);
        other.gameObject.transform.rotation = transform.rotation;
        other.GetComponent<CharacterController>().enabled = true;

        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(currentScene);
        Destroy(gameObject);
    }
}
