using System;
using UnityEngine;
using System.Collections;

public class RDrawerInteraction : RBaseInteraction
{
    public GameObject CameraPoint;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsInInteraction)
        {

        }
    }

    protected override void OnDeselect()
    {
    }

    protected override void OnSelect()
    {
        Camera.mainCamera.gameObject.SendMessage("MoveCameraToPoint", CameraPoint);
    }

    protected override void OnFocus()
    {
        collider.enabled = false;
    }

    protected override void OnFocusLost()
    {
        collider.enabled = true;
    }
}