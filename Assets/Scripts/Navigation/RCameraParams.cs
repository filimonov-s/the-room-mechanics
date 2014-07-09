using UnityEngine;
using System.Collections;

public struct RCameraParams
{
    public float Distance;
    public float XSpeed;
    public float YSpeed;
    public float YMinLimit;
    public float YMaxLimit;
    public float XMinLimit;
    public float XMaxLimit;
    public bool DontUseXLimits;
    public bool DontUseYLimits;
    public float SmoothTime;
}