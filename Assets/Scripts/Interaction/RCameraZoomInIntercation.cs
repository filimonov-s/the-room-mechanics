using UnityEngine;
using System.Collections;

public class RCameraZoomInIntercation : RBaseInteraction
{
    public GameObject CameraPoint;
    private RDrawerPivotOnFocusLostGag _pivot;

    // Use this for initialization
    private void Start()
    {
        _pivot = CameraPoint.transform.parent.GetComponent<RDrawerPivotOnFocusLostGag>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    protected override void OnFocus()
    {
        collider.enabled = false;
    }

    protected override void OnFocusLost()
    {
        collider.enabled = true;
        foreach (Transform child in transform)
        {
            child.gameObject.SendMessage("DisableInteraction", SendMessageOptions.DontRequireReceiver);
        }
    }

    protected override void OnSelect()
    {
        Camera.mainCamera.gameObject.SendMessage("MoveCameraToPoint", CameraPoint);
        Camera.mainCamera.gameObject.SendMessage("SetCameraParams", _pivot.RCameraParams);

        foreach (Transform child in transform)
        {
            child.gameObject.SendMessage("EnableInteraction", SendMessageOptions.DontRequireReceiver);
        }
       
    }

    protected override void OnDeselect()
    {
       
    }
}