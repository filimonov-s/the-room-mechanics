using UnityEngine;
using System.Collections;

public class RDoorIntercation : RBaseInteraction
{
    public GameObject TouchPlane;
    public GameObject Axis;
    public GameObject Door;
    public GameObject DoorDirection;
    public GameObject TouchPlanevertical;
    public bool Inverse;
    public bool UseLimits;

    private float _initAngle;
    private float _angle;
    private State _state = State.NotInitialized;

    private enum State
    {
        NotInitialized,
        Initialized,
        Inertion
    }

    protected override void OnDeselect()
    {
        RSelector.SelectorTarget = RSelectorTarget.All;
        TouchPlane.SetActive(false);
        TouchPlane.collider.enabled = false;

        TouchPlanevertical.SetActive(false);
        TouchPlanevertical.collider.enabled = false;

        _state = State.Inertion;
        _initAngle = 0;
    }

    protected override void OnSelect()
    {
        RSelector.SelectorTarget = RSelectorTarget.DirectionPlanes;
        TouchPlane.transform.position = new Vector3(
            TouchPlane.transform.position.x,
            RSelector.HittedPoint.y - 0.001f,
            TouchPlane.transform.position.z
            );

        TouchPlane.SetActive(true);
        TouchPlane.collider.enabled = true;

        TouchPlanevertical.SetActive(true);
        TouchPlane.collider.enabled = true;
        _state = State.NotInitialized;
    }

    private void MoveHorizontalPlane()
    {
        Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1 << 10);
        Debug.DrawRay(raycastHit.point, Input.mousePosition, Color.yellow);

        TouchPlane.transform.position = new Vector3(
            TouchPlane.transform.position.x,
            raycastHit.point.y - 0.001f,
            TouchPlane.transform.position.z
            );
    }


    private void Start()
    {
    }

    private void Update()
    {
        if (_state != State.NotInitialized)
        {
            if (_state == State.Initialized)
            {
                _angle = GetAngle() - _initAngle;
            }

            float rotation = 0;

            if (_state == State.Inertion)
            {
                rotation = _angle*Time.deltaTime*4;
                if (_angle > 0)
                {
                    _angle -= rotation;
                    if (_angle < 0)
                    {
                        _angle = 0;
                    }
                }
                else if (_angle < 0)
                {
                    _angle -= rotation;
                    if (_angle > 0)
                    {
                        _angle = 0;
                    }
                }
            }
            else if (IsInInteraction && InteractionEnabled)
            {
                rotation = _angle*Time.deltaTime*40;
            }

            float curAngle = transform.eulerAngles.y;
            float newAngle = curAngle + rotation;


            if (Inverse)
            {
                if (((newAngle < 180)) && UseLimits)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        180f,
                        transform.eulerAngles.z);
                }
                else if ((newAngle > 359.99f) && UseLimits)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        359.99f,
                        transform.eulerAngles.z);
                }
                else
                {
                    transform.Rotate(0, rotation, 0);
                }
            }
            else
            {
                if (newAngle > 180 && UseLimits)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        180f,
                        transform.eulerAngles.z);
                }
                else if (newAngle < 0 && UseLimits)
                {
                    transform.eulerAngles = new Vector3(
                        transform.eulerAngles.x,
                        0,
                        transform.eulerAngles.z);
                }
                else
                {
                    transform.Rotate(0, rotation, 0);
                }
            }
        }
        else if (_state == State.NotInitialized)
        {
            if (RSelector.SelectorTarget == RSelectorTarget.DirectionPlanes)
            {
                _initAngle = GetAngle();
                print("Init:" + _initAngle);
                _state = State.Initialized;
            }
        }
    }

    private float GetAngle()
    {
        Vector2 doorsDirection = new Vector2(
            DoorDirection.transform.position.x - Axis.transform.position.x,
            DoorDirection.transform.position.z - Axis.transform.position.z
            );

        Vector2 mouseDirection = new Vector2(
            RSelector.HittedPoint.x - Axis.transform.position.x,
            RSelector.HittedPoint.z - Axis.transform.position.z
            );

        float angle = Vector3.Angle(doorsDirection, mouseDirection);
        float dot =
            doorsDirection.x*mouseDirection.y -
            doorsDirection.y*mouseDirection.x;

        if (dot > 0) angle *= -1;

        return angle;
    }


    /*public float Spring = 50;
    public float Damper = 5;
    public float Drag = 10;
    public float AngularDrag = 5;
    public float Distance = 0.2f;

    //private bool attachToCenterOfMass = false;

    private SpringJoint _springJoint;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
       
    }

    protected override void OnSelect()
    {
        RaycastHit hit;

        if (!Physics.Raycast(Camera.mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 100))
            return;

        if (!hit.rigidbody || hit.rigidbody.isKinematic)
            return;

        if (!_springJoint)
        {
            var go = new GameObject("Rigidbody dragger");
            var body = go.AddComponent("Rigidbody") as Rigidbody;
            _springJoint = (SpringJoint)go.AddComponent("SpringJoint");
            body.isKinematic = true;
        }

        var myPoint = hit.point;
        _springJoint.transform.position = myPoint;


        _springJoint.anchor = Vector3.zero;

        _springJoint.spring = Spring;
        _springJoint.damper = Damper;
        _springJoint.maxDistance = Distance;
        _springJoint.breakForce = 1f;
        _springJoint.connectedBody = hit.rigidbody;

        StartCoroutine("DragObject", hit.distance);
    }

    private IEnumerator DragObject(float distance)
    {
        var oldDrag = _springJoint.connectedBody.drag;
        var oldAngularDrag = _springJoint.connectedBody.angularDrag;
        _springJoint.connectedBody.drag = Drag;
        _springJoint.connectedBody.angularDrag = AngularDrag;
        
        while (IsInInteraction)
        {
            var ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
            _springJoint.transform.position = ray.GetPoint(distance);
            yield return null;
        }

        if (_springJoint.connectedBody)
        {
            _springJoint.connectedBody.drag = oldDrag;
            _springJoint.connectedBody.angularDrag = oldAngularDrag;
            _springJoint.connectedBody = null;
        }
    }*/
}