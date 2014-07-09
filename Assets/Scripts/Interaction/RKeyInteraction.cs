using UnityEngine;
using System.Collections;

public class RKeyInteraction : RBaseInteraction
{
    public GameObject TouchPlane;
    public float MaxAngleSpeed;
    public float Slowdown;
    public GameObject Grip;
    public GameObject Grip2;
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

    private float _initOffset;
    private bool _isInitialized;

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

        _isInitialized = false;
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
        if (IsInInteraction && InteractionEnabled)
        {
           
                Vector2 gripVector = new Vector2(
                    Grip.transform.position.z - transform.position.z,
                    Grip.transform.position.y - transform.position.y
                    );

                Vector2 mouseVector = new Vector2(
                    RSelector.HittedPoint.z - transform.position.z,
                    RSelector.HittedPoint.y - transform.position.y
                    );

                float angle = Vector3.Angle(gripVector, mouseVector);

                float dot =
                    gripVector.x*mouseVector.y -
                    gripVector.y*mouseVector.x;

                if (dot < 0)
                    angle *= -1;

                //Angle = angle;

            if (!_isInitialized && RSelector.SelectorTarget == RSelectorTarget.DirectionPlanes)
            {
                _initOffset = angle;
                _isInitialized = true;
            }
            else
            {
                transform.Rotate(0, 0, angle - _initOffset);
            }

        }
            
    }


    private int _curGrip = 1;

    private void Grip2Selected()
    {
        if (_curGrip == 1)
        {
            _gripTransform = Grip2.transform;
            _curGrip = 2;
        }
        //print("Grip1");
    }

    private void Grip1Selected()
    {
        if (_curGrip == 2)
        {
            _gripTransform = Grip.transform;
            _curGrip = 1;
        }
        //print("Grip2");
    }

}