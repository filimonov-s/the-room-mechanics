using UnityEngine;
using System.Collections;

/// <summary>
/// This Component must be placed on Pivot Object.
/// Camera stores Pivot Object as Target, when Camera changes Target, 
/// it sends messages to Pivot Object and this Component forwards them to its Parent Object.
/// </summary>
public class RDrawerPivotOnFocusLostGag : RBaseInteraction
{
    public GameObject ParentCameraPoint;

    public Transform Target;
    public float Distance = 3;
    public float XSpeed = 250;
    public float YSpeed = 120;

    public float YMinLimit = -20;
    public float YMaxLimit = 80;

    public float XMinLimit = -80;
    public float XMaxLimit = 80;
    public bool DontUseXLimits = true;
    public bool DontUseYLimits = true;

    public float SmoothTime = 0.3f;
    public float SmoothLookAtSmooth = 20;
    public RCameraParams RCameraParams;
    private RDrawerPivotOnFocusLostGag _parentPivot;

    private void Start()
    {
        SetCameraParams();
        if (ParentCameraPoint)
            _parentPivot = ParentCameraPoint.transform.parent.GetComponent<RDrawerPivotOnFocusLostGag>();
    }

    /// <summary>
    /// Camera calls this method when it looses focus from this object
    /// </summary>
    protected override void OnFocusLost()
    {
        transform.parent.SendMessage("FocusLost");
        if (ParentCameraPoint)
        {
            transform.parent.SendMessage("ChildFocusLost");
        }
    }

    /// <summary>
    /// Camera calls this method when get it focused on this object
    /// </summary>
    protected override void OnFocus()
    {
        transform.parent.SendMessage("Focus");

        if (ParentCameraPoint)
        {
            transform.parent.SendMessage("ChildFocus");
        }
    }

    protected override void OnZoomOut()
    {
        if (ParentCameraPoint)
        {
            Camera.mainCamera.gameObject.SendMessage("MoveBackCameraToPoint", ParentCameraPoint);
            Camera.mainCamera.gameObject.SendMessage("SetCameraParams", _parentPivot.RCameraParams);
        }
    }

    protected override void OnChildFocusLost()
    {
        transform.parent.SendMessage("ChildFocusLost");
    }

    protected override void OnChildFocus()
    {
        transform.parent.SendMessage("ChildFocus");
    }

    private void SetCameraParams()
    {
        RCameraParams.XMaxLimit = XMaxLimit;
        RCameraParams.XMinLimit = XMinLimit;
        RCameraParams.YMaxLimit = YMaxLimit;
        RCameraParams.YMinLimit = YMinLimit;
        RCameraParams.XSpeed = XSpeed;
        RCameraParams.YSpeed = YSpeed;
        RCameraParams.SmoothTime = SmoothTime;
        RCameraParams.DontUseXLimits = DontUseXLimits;
        RCameraParams.DontUseYLimits = DontUseYLimits;
        RCameraParams.Distance = Distance;
    }
}