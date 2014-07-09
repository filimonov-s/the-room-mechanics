using UnityEngine;
using System.Collections;

public class RCircleIneraction : RBaseInteraction
{
    public GameObject TouchPlane;
    public float MaxAngleSpeed;
    public float Slowdown;
    public GameObject Grip;
    public bool IsCogwheel;
    public float Acceleration;
    public bool UseGravity;

    private Transform _gripTransform;
    private float _angle;
    private float _toRotate;
    private float _step = 30;


    private float _rotatedAtStep;
    private bool _isRotatingBetweenPosState;
    private float _stepRotSign;

    // Angle between rotation center and touch point when activate handle
    private float _initAngle;
    private bool _isInitAngleInitialized;
    // TODO: change to auto calculated value
    private const float AddAgleToRotate = 1;

    protected override void OnDeselect()
    {
        TouchPlane.SetActive(false);
        TouchPlane.gameObject.collider.enabled = false;
        RSelector.SelectorTarget = RSelectorTarget.All;

        _isInitAngleInitialized = false;
    }

    protected override void OnSelect()
    {
        TouchPlane.SetActive(true);
        TouchPlane.gameObject.collider.enabled = true;
        RSelector.SelectorTarget = RSelectorTarget.DirectionPlanes;
    }

    // Use this for initialization
    private void Start()
    {
        _gripTransform = Grip.transform;
    }

    private void FixedUpdate()
    {
        Rotate();
    }

    public void Rotate()
    {
     /*   if (_isRotatingBetweenPosState)
        {
            if (_rotatedAtStep < _step)
            {
                float curR = _step*Acceleration*Time.deltaTime;
                transform.Rotate(0, curR*_stepRotSign, 0);
                _rotatedAtStep += curR;

                if (_rotatedAtStep >= _step)
                {
                    _rotatedAtStep = 0;
                    _isRotatingBetweenPosState = false;
                    _stepRotSign = 0;
                }
            }
        }
        else*/ if (IsInInteraction)
        {
            Vector2 handleDirection = new Vector2(
                _gripTransform.position.y - transform.position.y,
                _gripTransform.position.z - transform.position.z);

            Vector2 mouseDirection = new Vector2(
                RSelector.HittedPoint.y - transform.position.y,
                RSelector.HittedPoint.z - transform.position.z);

                   

            _angle = Vector2.Angle(handleDirection, mouseDirection);

            float sign =
                handleDirection.x*mouseDirection.y -
                handleDirection.y*mouseDirection.x;

            if (_angle > MaxAngleSpeed)
                _angle = MaxAngleSpeed;

            if (sign > 0)
                _angle *= -1;

            if (IsCogwheel)
            {
                if (Globals.MouseSpeedX != 0 || Globals.MouseSpeedY != 0)
                    _toRotate += Acceleration*_angle*Time.deltaTime;

                if (_toRotate > _step)
                {
                    _stepRotSign = -1;
                    _toRotate = 0;
                    _isRotatingBetweenPosState = true;
                }
                else if (_toRotate < -_step)
                {
                    _stepRotSign = 1;
                    _toRotate = 0;
                    _isRotatingBetweenPosState = true;
                }
            }
            else
            {
                float rotate = Acceleration*_angle*Time.deltaTime;

                if (_angle > 0)
                    rotate *= _angle;
                else
                    rotate *= -_angle;

                /*if (Mathf.Abs(_angle) > 7)
                {
                    transform.Rotate(0, rotate, 0);
                }*/

                // Rotate right

                if (!_isInitAngleInitialized)
                {
                    if (RSelector.SelectorTarget == RSelectorTarget.DirectionPlanes)
                    {
                        _initAngle = _angle;
                        _isInitAngleInitialized = true;
                        //print("Init: " + _initAngle + "Using object: " + GlobalVars.HittedObject.name);
                    }

                    return;
                }

                if ((_angle > 0 && _angle > _initAngle) || (_angle < 0 && _angle < _initAngle))
                {
                    transform.Rotate(0, 0, rotate);
                    // print("I: " + _initAngle + " A: " + _angle + "using object: " + GlobalVars.HittedObject.name);
                }
                /*else if (_initAngle < 0 && _angle > 0 && _angle > (-_initAngle + GripAngleSize))
                {
                    transform.Rotate(0, rotate, 0);
                }*/
            }
        }
            // Inertion
                   return;
        /*else*/ if (!IsCogwheel && _isInitAngleInitialized)
        {
            if (_angle != 0)
            {
                if (_angle > 0)
                {
                    _angle -= Slowdown;
                    if (_angle < 0)
                        _angle = 0;
                }
                else
                {
                    _angle += Slowdown;
                    if (_angle > 0)
                        _angle = 0;
                }


                transform.Rotate(0, 20*Acceleration*_angle*Time.deltaTime, 0);
            }
        }

        if (!IsInInteraction && UseGravity)
        {
            Vector2 handleDirection = new Vector2(
                _gripTransform.position.x - transform.position.x,
                _gripTransform.position.y - transform.position.y);

            Vector2 gravityDirection = new Vector2(
                0,
                -5);

            float angle = Vector2.Angle(handleDirection, gravityDirection);

            float sign =
                handleDirection.x*gravityDirection.y -
                handleDirection.y*gravityDirection.x;

            if (sign < 0)
                angle *= -1;

            //print(angle);

            float gravityForce = angle*Time.deltaTime;
            /* print("Descreased: " + angle * Time.deltaTime);
             print("_angle: " + _angle);*/

            transform.Rotate(0, gravityForce, 0);
        }
    
    }
}