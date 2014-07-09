using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class RSwipeBallInteraction : RBaseInteraction
{
    public GameObject DirectionPlane;
    public float BreakDistance = 2;
    public float PushPower = 75;
    public float FollowSpeed = 30;
    public float ThrowPower = 500;
    public GameObject Borders;

    private Vector3 _prevPosition;
    private float acceleratedPush;
    private bool _isBroken;

    private Vector3 _initPoint;
    private readonly Vector3[] _accumulatedPush = new Vector3[AccumulationSize];
    private const int AccumulationSize = 1;
    private Vector3 _selectPosition;

    private int i = 0;
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (IsInInteraction && !_isBroken)
        {
            if (Vector3.Distance(transform.position, RSelector.HittedPoint) > BreakDistance)
            {
                OnDeselect();
                _isBroken = true;
                return;
            }

            _prevPosition = transform.position;

            /*Vector3 newPosition = new Vector3(
                Mathf.Lerp(transform.position.x, RSelector.HittedPoint.x, FollowSpeed*Time.deltaTime),
                transform.position.y,
                Mathf.Lerp(transform.position.z, RSelector.HittedPoint.z, FollowSpeed*Time.deltaTime)
                );


            if (Vector3.Distance(_prevPosition, newPosition) > 0.001f)
                transform.position = newPosition;*/
            
            rigidbody.velocity = 
                (new Vector3(RSelector.HittedPoint.x, transform.position.y, RSelector.HittedPoint.z) - 
                transform.position) * 15;

            _accumulatedPush[i++] += rigidbody.velocity;
            
            if (i >= AccumulationSize) i = 0;

            /*acceleratedPush += (_prevPosition - transform.position).magnitude * 1000 * Time.deltaTime;
            acceleratedPush -= 0.01f;
            if (acceleratedPush < 0)
                acceleratedPush = 0;*/

        }
    }

    protected override void OnSelect()
    {
        DirectionPlane.SetActive(true);
        DirectionPlane.collider.enabled = true;
        RSelector.SelectorTarget = RSelectorTarget.DirectionPlanes;
        acceleratedPush = 0;


        _isBroken = false;
        _initPoint = RSelector.HittedPoint;


        _selectPosition = transform.position;

        //Borders.SetActive(true);

        for (int j = 0; j < AccumulationSize; j++)
        {
            _accumulatedPush[j] = Vector3.zero;
        }
    }

    protected override void OnDeselect()
    {

        DirectionPlane.collider.enabled = false;
        DirectionPlane.SetActive(false);
        RSelector.SelectorTarget = RSelectorTarget.All;

        Borders.SetActive(false);

        if (_isBroken)
            return;

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        //Vector3 direction = RSelector.HittedPoint - transform.position;
        Vector3 direction = transform.position - _prevPosition;
        direction.y = 0;


        float curDistance = CurrentDistance();

        /*// to fix lagx
        if (curDistance > BreakDistance)
            curDistance = BreakDistance;*/

        //rigidbody.AddForce(direction*PushPower*curDistance);

        Vector3 pushPower = Vector3.zero;
        foreach (Vector3 v in _accumulatedPush)
        {
            pushPower += v;
        }
        //rigidbody.AddForce(direction*_accumulatedPush);
        pushPower.y = 0;
        direction = direction.normalized;
        direction.y = 0;

        rigidbody.AddForce(direction.normalized * PushPower);
        print(direction * PushPower);
    }

    private float CurrentDistance()
    {
        return (RSelector.HittedPoint - transform.position).magnitude;
    }
}