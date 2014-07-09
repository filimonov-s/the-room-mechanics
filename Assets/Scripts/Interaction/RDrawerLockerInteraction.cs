using UnityEngine;
using System.Collections;

public class RDrawerLockerInteraction : RBaseInteraction
{
    public GameObject LockerContainer;
    public GameObject TouchPlane;
    public GameObject ControlObject;
    public GameObject Floor;
    public float SmoothTime = 0.2f;

    private Vector3 _initPosition;
    private float _initOffset;
    private float _maxOffset;
    private bool _isInitialized;
    private float _velocity;


    // Use this for initialization
    private void Start()
    {
        _initPosition = LockerContainer.transform.localPosition;
        _maxOffset = Floor.renderer.bounds.size.z;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsInInteraction)
        {
            float zHittedPointToLocal = LockerContainer.transform.InverseTransformPoint(RSelector.HittedPoint).z;

            if (!_isInitialized)
            {
                if (RSelector.HittedObject.layer == 8)
                {
                    _isInitialized = true;
                    _initOffset = ControlObject.transform.localPosition.z - zHittedPointToLocal;
                }
                return;
            }

            float distance = Mathf.Abs(ControlObject.transform.localPosition.z - zHittedPointToLocal);
            float z = Mathf.SmoothDamp(
                ControlObject.transform.localPosition.z,
                zHittedPointToLocal + _initOffset,
                ref _velocity,
                SmoothTime / distance * Time.deltaTime
                );

            Vector3 newPosition = new Vector3(
                LockerContainer.transform.localPosition.x,
                LockerContainer.transform.localPosition.y,
                LockerContainer.transform.localPosition.z - (ControlObject.transform.localPosition.z - z));

            if (newPosition.z <= _initPosition.z + _maxOffset && newPosition.z >= _initPosition.z)
            {
                LockerContainer.transform.localPosition = newPosition;
            }
            else if (newPosition.z <= _initPosition.z)
            {
                LockerContainer.transform.localPosition = _initPosition;
                _velocity = 0;
            }
            else if (newPosition.z >= _initPosition.z + _maxOffset)
            {
                LockerContainer.transform.localPosition = new Vector3(
                    LockerContainer.transform.localPosition.x,
                    LockerContainer.transform.localPosition.y,
                    _initPosition.z + _maxOffset);
                _velocity = 0;
            }
        }
    }

    protected override void OnSelect()
    {
        TouchPlane.collider.enabled = true;
        TouchPlane.SetActive(true);
        RSelector.SelectorTarget = RSelectorTarget.DirectionPlanes;
    }

    protected override void OnDeselect()
    {
        TouchPlane.collider.enabled = false;
        TouchPlane.SetActive(false);
        RSelector.SelectorTarget = RSelectorTarget.All;
        _isInitialized = false;
    }
}