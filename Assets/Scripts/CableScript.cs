using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class CableScript : MonoBehaviour
{
    public Transform plugposition;
    private LineRenderer lr;
    private Transform raycastOrigin;
    private RaycastHit hit;
    private int currentPointNumber;
    private List<HitPoint> points = new List<HitPoint>();

    struct HitPoint
    {
        public bool normalFacingRight;
        public Vector3 position;

        public HitPoint(bool nfr, Vector3 pointPosition)
        {
            normalFacingRight = nfr;
            position = pointPosition;
        }
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        raycastOrigin = transform;
        Physics.Linecast(raycastOrigin.position, plugposition.position, out hit);
        currentPointNumber = 1;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, plugposition.position);
    }

    void Update()
    {
        //creating the line
        lr.SetPosition(currentPointNumber, plugposition.position);

        //removing the bend point and moving the raycast origin
        if (lr.positionCount > 2)
        {
            float lastTwoPointsAngle = Vector3.SignedAngle(lr.GetPosition(currentPointNumber - 1) - lr.GetPosition(currentPointNumber - 2), lr.GetPosition(currentPointNumber) - lr.GetPosition(currentPointNumber - 1), Vector3.up);
            Debug.Log(lastTwoPointsAngle);
            var hitPoint = points[currentPointNumber - 2];
            if ((hitPoint.normalFacingRight && lastTwoPointsAngle > 5) || (!hitPoint.normalFacingRight && lastTwoPointsAngle < -5))
            {
                lr.positionCount--;
                currentPointNumber--;
                lr.SetPosition(currentPointNumber, plugposition.position);
                raycastOrigin.position = lr.GetPosition(currentPointNumber - 1);
            }
        }

        //checking for collisions on the cable's path
        if (!Physics.Linecast(raycastOrigin.position, plugposition.position, out hit, ~LayerMask.GetMask("HeldObject", "Player", "Ignore Raycast")))
        {
            return;
        }

        //adding the point and moving the raycast origin position
        lr.positionCount++;
        lr.SetPosition(currentPointNumber, hit.point);
        lr.SetPosition(currentPointNumber + 1, plugposition.position);
        currentPointNumber++;
        raycastOrigin.position = hit.point;
        points.Add(new(Camera.main.transform.InverseTransformDirection(hit.normal).x < 0, hit.point));
    }
}
