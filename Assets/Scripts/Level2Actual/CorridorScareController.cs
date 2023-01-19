using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorScareController : BaseScriptedControler
{
    public SpicySoundSource speakerAboveFirst;
    // Start is called before the first frame update
    void Start()
    {
        Setup();
        gesystem.registerCallback("playfake1", e => playfake1());
        gesystem.registerCallback("spawnenemy1", e => spawnenemy1());
        gesystem.registerCallback("spawnenemy2", e => spawnenemy2());
        gesystem.registerCallback("spawnenemy3", e => spawnenemy3());
        gesystem.registerCallback("spawnenemy4", e => spawnenemy4());
    }

    private void OnDestroy()
    {
        gesystem.removeCallback("playfake1");
        gesystem.removeCallback("spawnenemy1");
        gesystem.removeCallback("spawnenemy2");
        gesystem.removeCallback("spawnenemy3");
        gesystem.removeCallback("spawnenemy4");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private bool faked1 = false;
    void playfake1()
    {
        if (!faked1)
        {
            speakerAboveFirst.Play("fakenoise");
            FindObjectOfType<FloatingTextOnHud>().StartDisplayingMiddleText("...?");
            faked1 = true;
        }
    }


    public GameObject enemy1;

    private bool spawnedenemy1 = false;
    void spawnenemy1()
    {
        if (!spawnedenemy1)
        {
            spawnedenemy1 = true;
            enemy1.SetActive(true);
        }
    }

    public GameObject enemy2;

    private bool spawnedenemy2 = false;
    void spawnenemy2()
    {
        if (!spawnedenemy2)
        {
            spawnedenemy2 = true;
            enemy2.SetActive(true);
        }
    }

    public GameObject enemy3;

    private bool spawnedenemy3 = false;
    void spawnenemy3()
    {
        if (!spawnedenemy3)
        {
            spawnedenemy3 = true;
            enemy3.SetActive(true);
        }
    }

    public GameObject enemy4;

    private bool spawnedenemy4 = false;
    void spawnenemy4()
    {
        if (!spawnedenemy4)
        {
            spawnedenemy4 = true;
            enemy4.SetActive(true);
        }
    }
}
