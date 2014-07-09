using UnityEngine;
using System.Collections;

public class RDemoCubeIteraction : RBaseInteraction
{
    public GameObject CameraPoint;
    private RDrawerPivotOnFocusLostGag _pivot;

    // Use this for initialization
    private void Start()
    {
        _pivot = CameraPoint.GetComponent<RDrawerPivotOnFocusLostGag>();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    protected override void OnSelect()
    {
        Camera.mainCamera.gameObject.SendMessage("MoveCameraToPoint", CameraPoint);
        Camera.mainCamera.gameObject.SendMessage("SetCameraParams", _pivot.RCameraParams);
    }

    protected override void OnDeselect()
    {

    }

    protected override void OnFocus()
    {
        collider.enabled = false;
    }

    protected override void OnFocusLost()
    {
        collider.enabled = true;
    }

    protected override void OnChildFocusLost()
    {
        collider.enabled = true;
    }

    protected override void OnChildFocus()
    {
        collider.enabled = false;
    }
}