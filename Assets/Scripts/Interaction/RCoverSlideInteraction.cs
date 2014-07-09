using UnityEngine;
using System.Collections;

public class RCoverSlideInteraction : RBaseInteraction
{
    public GameObject TouchPlane;
    public Vector3 Direction;
    public float MaxOffset = 1;
    public GameObject Keyhole;
    public bool IsEnabled;
    public float SmoothTime;

    private Vector3 _initPosition;
    private float _initOffsetOnPlane;
    private bool _isInitialized;
    private float _velocity;



    // Use this for initialization
    private void Start()
    {
        _initPosition = transform.localPosition;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsInInteraction && IsEnabled && InteractionEnabled)
        {
            if (_isInitialized)
            {
                float zHittedPointToLocal = transform.parent.transform.InverseTransformPoint(RSelector.HittedPoint).z;
                float distance = Mathf.Abs(transform.localPosition.z - zHittedPointToLocal);

                float z = Mathf.SmoothDamp(
                    transform.localPosition.z,
                    zHittedPointToLocal - _initOffsetOnPlane,
                    ref _velocity,
                    SmoothTime/distance*Time.deltaTime
                    );

                z = zHittedPointToLocal - _initOffsetOnPlane;


                Vector3 newPosition = new Vector3(
                    transform.localPosition.x,
                    transform.localPosition.y,
                    z
                    );

                if (Mathf.Abs(newPosition.z - _initPosition.z) > MaxOffset)
                {
                    if (z < 0)
                    {
                        newPosition.z = _initPosition.z - MaxOffset;
                    }
                    else if (z > 0)
                    {
                        newPosition.z = _initPosition.z + MaxOffset;
                    }
                }

                transform.localPosition = newPosition;

            }
            else
            {
                if (RSelector.SelectorTarget == RSelectorTarget.DirectionPlanes)
                {
                    _initOffsetOnPlane =
                        transform.parent.transform.InverseTransformPoint(RSelector.HittedPoint).z -
                        transform.localPosition.z;

                    _isInitialized = true;
                }
            }
        }
    }

    protected override void OnSelect()
    {
        TouchPlane.SetActive(true);
        TouchPlane.collider.enabled = true;
        RSelector.SelectorTarget = RSelectorTarget.DirectionPlanes;
    }

    protected override void OnDeselect()
    {
        TouchPlane.collider.enabled = false;
        TouchPlane.SetActive(false);
        RSelector.SelectorTarget = RSelectorTarget.All;
        _isInitialized = false;
    }

    private void SetEnabled()
    {
        IsEnabled = true;
    }

    private void SetDisabled()
    {
        IsEnabled = false;
    }
}