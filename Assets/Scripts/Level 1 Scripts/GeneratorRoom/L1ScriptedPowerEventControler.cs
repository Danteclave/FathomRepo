using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class L1ScriptedPowerEventControler : BaseScriptedControler
{
    public GameObject leverspot;

    public GameObject illusionEnemy;

    public GameObject generator;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

        floattext = FindObjectOfType<FloatingTextOnHud>();

        gesystem.registerCallback("turnAllLightsOn", d => setlights(true));
        gesystem.registerCallback("turnAllLightsOff", d => setlights(false));
        gesystem.registerCallback("enemyIllusion", d => enemyillusion());
        gesystem.registerCallback("togglePower", d => togglePower());
        gesystem.registerCallback("lockedtraindoor", d => displayMessage());
        gesystem.registerCallback("allowTrain", d => allowTrain());
        gesystem.registerCallback("onLeverClick", d => onLeverClick());

        meshes.AddRange(FindObjectsOfType<FluorescentL1>().Select(e => e.gameObject.GetComponent<MeshRenderer>()).ToList());
    }

    private FloatingTextOnHud floattext;

    void displayMessage()
    {
        if(!power)
            floattext.StartDisplayingMiddleText("Maybe this door will open if I turn on the power?");
    }

    private void OnDestroy()
    {
        gesystem.removeCallback("turnAllLightsOn");
        gesystem.removeCallback("turnAllLightsOff");
        gesystem.removeCallback("enemyIllusion");
        gesystem.removeCallback("togglePower");
        gesystem.removeCallback("lockedtraindoor");
        gesystem.removeCallback("allowTrain");
    }

    void enemyillusion()
    {
        illusionEnemy.SetActive(true);
        illusionEnemy.GetComponent<IllusionScriptL1>().state = EnemyAI.State.Script;
    }

    public Animator trainAnim;

    public bool power = false;
    void togglePower()
    {
        power ^= true;
        if(power)
        {
            leverspot.transform.rotation = Quaternion.Euler(0, -30, 0);
            gesystem.callEvent("turnAllLightsOn");
            trainAnim.SetTrigger("DoorToOpen");
            generator.GetComponent<SpicySoundSource>().Play("turnOn");
            generator.GetComponent<SpicySoundSource>().Play("on");


        }
        else
        {
            leverspot.transform.rotation = Quaternion.Euler(0, 30, 0);
            gesystem.callEvent("turnAllLightsOff");
            trainAnim.SetTrigger("DoorToClose");
            generator.GetComponent<SpicySoundSource>().Stop(generator, "on");
        }
    }

    public List<MeshRenderer> meshes;
    public Material materialOn;
    public Material materialOff;

    public MeshRenderer train;
    public Material dashOn;
    public Material dashOff;

    void setlights(bool value)
    {
        foreach(var x in FindObjectsOfType<FluorescentL1>())
        {
            x.islighton = value;
        }

        foreach(var x in meshes)
        {
            var mats = x.sharedMaterials;
            for(int i =0; i<mats.Length; i++)
            {
                if(mats[i] == materialOff && value)
                {
                    mats[i] = materialOn;
                }
                else if(mats[i] == materialOn && !value)
                {
                    mats[i] = materialOff;
                }
            }
            x.sharedMaterials = mats;
        }
    }

    private bool trainEnabled = false;

    void allowTrain()
    {
        var tmats = train.sharedMaterials;
        for (int i = 0; i < tmats.Length; i++)
        {
            if (tmats[i] == dashOff)
            {
                tmats[i] = dashOn;
            }
        }
        train.sharedMaterials = tmats;
        trainEnabled = true;
    }

    void onLeverClick()
    {
        if(!trainEnabled)
        {
            floattext.StartDisplayingMiddleText("The train is not operational.");
        }
        else
        {
            teleport.GoodProgramming(FindObjectOfType<CharacterController>());
        }
    }

    public TeleportWzium teleport;

    // Update is called once per frame
    void Update()
    {
        
    }
}
