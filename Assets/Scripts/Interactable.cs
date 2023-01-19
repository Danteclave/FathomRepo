using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [SerializeField]
    private List<Transform> grabPoints;

    public Transform[] axisPoints;
    public bool shouldLockScreen
    {
        get => axisPoints.Length != 0;
    }

    public bool _beingInteracted = false;
    public bool dumbInteracted = false;
    public bool beingInteracted
    {
        get => _beingInteracted;
        set
        {
            if (_beingInteracted == value) return;
            if (_beingInteracted = value)
            {
                dumbInteracted = true;
                // TODO: fix if I ever need to bullshit my way that I did something
                grabPoints.Sort((a, b) => Vector3.Distance(grabLocation, a.transform.position).CompareTo(Vector3.Distance(grabLocation, b.transform.position)));
                totalDeltaX = mouseDeltaX = totalDeltaY = mouseDeltaY = 0;
                if (!amogus || lastpunkcior == null)
                {
                    punkcior = grabPoints[0].position;
                    amogus = true;
                }
                else
                {
                    punkcior = (Vector3)lastpunkcior;
                }
            }
            else
            {
                lastpunkcior = punkcior;
            }

            if (!shouldLockScreen)
            {
                Physics.IgnoreCollision(FindObjectOfType<PlayerController>().capCollider, GetComponent<Collider>(), !value);
                rb.useGravity = !value;
            }
        }
    }

    private Rigidbody rb;

    public float[] AngleConstraints = new float[2]; //only used in spinny bois
    public Vector3[] PunkciorSpinConstraints = new Vector3[2]; //n-th point is used as the fallback for n-th constraint
    public Vector3[] PosConstraints = new Vector3[2]; //only used in slidy bois
    public Vector3[] PunkciorSlidyConstraints = new Vector3[2]; //same except for slidy bois


    public Vector3 punkcior;
    public Vector3? lastpunkcior = null;
    private bool amogus = false;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = FindObjectOfType<MouseLook>().transform;
        rb = GetComponent<Rigidbody>();
    }


    public GameObject debugCube;
    public GameObject debugCumbe;

    public int Quadrant;

    public float throwforce = 60f;

    // Update is called once per frame
    void Update()
    {
        if (beingInteracted)
        {
            mouseDeltaX = mouseDeltaY = 0;
            mouseDeltaX = Input.GetAxis("Mouse X");
            mouseDeltaY = Input.GetAxis("Mouse Y");
            if (axisPoints.Length == 2)
            {
                //todo: add angular velocity instead of 1:1 mapping
                var tdir = playerTransform.TransformDirection(new Vector3(mouseDeltaX * 0.2f, 0, mouseDeltaY * 0.2f));
                tdir.y = 0;
                //Debug.Log(tdir);
                punkcior += tdir;
                punkcior = (punkcior - transform.position).normalized;
                punkcior += transform.position;

                if (debugCube != null && debugCumbe != null)
                {
                    debugCube.transform.position = punkcior + Vector3.back;
                    debugCumbe.transform.position = transform.position;
                }

                var angle = Vector3.SignedAngle(transform.TransformDirection(Vector3.back), transform.TransformDirection((punkcior) - transform.position), Vector3.up)+ AngleConstraints[0];

                if (angle > AngleConstraints[1])
                {
                    //Debug.Log("ustawionko max");
                    angle = AngleConstraints[1];
                    punkcior = (transform.position + PunkciorSpinConstraints[1]);
                }
                if (angle < AngleConstraints[0])
                {
                    //Debug.Log("ustawionko min");
                    angle = AngleConstraints[0];
                    punkcior = (transform.position + PunkciorSpinConstraints[0]);
                }
                //punkciorObject.transform.position = punkcior;
                //Debug.Log(punkcior);
                //Debug.Log(angle);

                //transform.eulerAngles = new Vector3(0, Mathf.Lerp(transform.eulerAngles.y, angle, 0.5f), 0);
                transform.LookAt(punkcior);
                transform.Rotate(new Vector3(0, Quadrant, 0));
            }
            else if (axisPoints.Length == 0)
            {
                var newpos = playerTransform.position + playerTransform.TransformDirection(Vector3.forward * gripDistance);


                // this is extremely wonky but oh well
                Vector3 oldVel = rb.velocity;
                //Get the position offset
                Vector3 delta = newpos - rb.position;
                //Get the speed required to reach it next frame
                Vector3 vel = delta / Time.fixedDeltaTime;

                //If you still want gravity, you can do this
                //vel.y = oldVel.y;

                rb.velocity = vel;


                // maybe change this to rb.MoveRotation but the docs are complete garbage
                transform.LookAt(playerTransform);
            }
            else if (axisPoints.Length == 1)
            {
                var tdir = playerTransform.TransformDirection(new Vector3(mouseDeltaX * 0.2f, 0, mouseDeltaY * 0.2f));
                tdir.y = 0;
                punkcior += tdir;


                var newpos = axisPoints[0].position;
                newpos.x += punkcior.x;
                bool broken = false;
                if (newpos.x < PosConstraints[0].x)
                {
                    newpos.x = PosConstraints[0].x;
                    punkcior = (transform.position + PunkciorSlidyConstraints[0]);
                    rb.velocity = Vector3.zero;
                    broken = true;
                }
                if (newpos.x > PosConstraints[1].x)
                {
                    newpos.x = PosConstraints[1].x;
                    punkcior = (transform.position + PunkciorSlidyConstraints[1]);
                    rb.velocity = Vector3.zero;
                    broken = true;
                }

                debugCube.transform.position = newpos;

                if (!broken)
                {
                    Vector3 delta = newpos - (rb.position);
                    //Get the speed required to reach it next frame
                    Vector3 vel = delta / Time.fixedDeltaTime;
                    rb.velocity = vel;
                }
            }
        }
        else
        {
            //do stuff I guess
        }
    }

    public int gripDistance = 5;

    public Vector3 lockAxis1;
    public Vector3 lockAxis2;

    public Vector3 grabLocation;
    public Transform playerTransform;

    public float mouseDeltaX;
    public float mouseDeltaY;
    public float controllerDeltaX;
    public float controllerDeltaY;
    public float totalDeltaX;
    public float totalDeltaY;

    [SerializeField]
    private Transform anchorPoint;
}
