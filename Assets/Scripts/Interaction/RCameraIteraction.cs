using UnityEngine;
using System.Collections;

public class RCameraIteraction : MonoBehaviour
{
    public Transform Target;
    public float Distance = 3;
    public float XSpeed = 250;
    public float YSpeed = 120;

    public float YMinLimit = -20;
    public float YMaxLimit = 80;

    public float XMinLimit = -180;
    public float XMaxLimit = -360;
    public bool DontUseXLimits;
    public bool DontUseYLimits;

    public float SmoothTime = 0.3f;
    public float SmoothLookAtSmooth = 20;

    private float _xSmooth;
    private float _ySmooth;
    private float _xVelocity;
    private float _yVelocity;

    private Vector3 _posSmooth = Vector3.zero;
    private float _initDistance;

    // New 
    private bool _isMovingToNewTarget;
    private bool _isMoveToDistanceMode;
    private GameObject _moveTo;

    private bool _isInInteraction;


    public float X { get; private set; }
    public float Y { get; private set; }

    public bool IsMovingToNewTarget
    {
        get { return _isMovingToNewTarget; }
        private set
        {
            _isMovingToNewTarget = value;
            RSelector.Enabled = !value;
        }
    }

    // Use this for initialization
    private void Start()
    {
        var angles = transform.eulerAngles;
        X = angles.y;
        Y = angles.x;

        _xSmooth = X;
        _ySmooth = Y;

        _initDistance = Distance;

        if (Target)
        {
            Target.SendMessage("Focus");
        }
    }

    private void SetCameraParams(RCameraParams rCameraParams)
    {
        XMaxLimit = rCameraParams.XMaxLimit;
        XMinLimit = rCameraParams.XMinLimit;
        YMaxLimit = rCameraParams.YMaxLimit;
        YMinLimit = rCameraParams.YMinLimit;
        XSpeed = rCameraParams.XSpeed;
        YSpeed = rCameraParams.YSpeed;
        SmoothTime = rCameraParams.SmoothTime;
        DontUseXLimits = rCameraParams.DontUseXLimits;
        DontUseYLimits = rCameraParams.DontUseYLimits;
        Distance = rCameraParams.Distance;
    }


    // Update is called once per frame
    private void Update()
    {
    }


    private void LateUpdate()
    {
        if (IsMovingToNewTarget)
        {
            if (_isMoveToDistanceMode)
                MoveCameraToTarget();
            else
                MoveCameraToPoint();
            return;
        }

        if (_isInInteraction)
        {
            float xInput = -Globals.MouseSpeedX*XSpeed*0.02f/10;
            if (((xInput > 0 && X < XMaxLimit)
                 || (xInput < 0 && X > XMinLimit)) || DontUseXLimits)
            {
                X += xInput;
            }

            float yInput = Globals.MouseSpeedY*YSpeed*0.02f/10;

            if (((yInput > 0 && Y < YMaxLimit) ||
                 (yInput < 0 && Y > YMinLimit)) || DontUseYLimits)
            {
                Y += yInput;
            }
        }


        _xSmooth = Mathf.SmoothDamp(_xSmooth, X, ref _xVelocity, SmoothTime);
        _ySmooth = Mathf.SmoothDamp(_ySmooth, Y, ref _yVelocity, SmoothTime);

        if (!DontUseXLimits)
        {
            _xSmooth = ClampAngle(_xSmooth, XMinLimit, XMaxLimit);
        }

        if (!DontUseYLimits)
        {
            _ySmooth = ClampAngle(_ySmooth, YMinLimit, YMaxLimit);
        }


        var rotation = Quaternion.Euler(_ySmooth, _xSmooth, 0);

        // _posSmooth = Vector3.SmoothDamp(_posSmooth,Target.position, ref _posVelocity, _rCameraParams.SmoothTime);

        _posSmooth = Target.position; // no follow smoothing

        transform.rotation = rotation;
        transform.position = rotation*new Vector3(0, 0, -Distance) + _posSmooth;
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// Receives message and starts mooving camera to new position
    /// </summary>
    /// <param name="moveTo">New Camera Position</param>
    private void MoveCameraToPoint(GameObject moveTo)
    {
        if (moveTo.transform.parent.transform != Target)
        {
            Target.SendMessage("FocusLost");

            _moveTo = moveTo;
            Target = _moveTo.transform.parent;
            IsMovingToNewTarget = true;
        }
    }

    private void MoveBackCameraToPoint(GameObject moveTo)
    {
        Vector3 direction = transform.position - moveTo.transform.parent.transform.position;
        Vector3 newDirection = moveTo.transform.position - moveTo.transform.parent.transform.position;

        direction.y = 0;
        newDirection.y = 0;

        float angle = Vector3.Angle(direction, newDirection);


        float dot =
            direction.x*newDirection.z -
            direction.z*newDirection.x;

        moveTo.transform.RotateAround(
            moveTo.transform.parent.position,
            Vector3.up,
            dot > 0 ? angle : -angle
            );

        MoveCameraToPoint(moveTo);
    }


    /// <summary>
    /// Receives message and starts mooving camera to new target
    /// </summary>
    /// <param name="moveTo">New Camera Target</param>
    private void MoveCameraToTarget(GameObject moveTo)
    {
        _isMoveToDistanceMode = true;
        MoveCameraToPoint(moveTo);
    }

    private void ZoomInCamera(GameObject target)
    {
        if (target.transform != Target)
        {
        }
    }

    /// <summary>
    /// Smoothly moves camera to new position 
    /// </summary>
    private void MoveCameraToPoint()
    {
        const float smooth = 3;
        Vector3 newPosition = new Vector3(
            Mathf.Lerp(
                transform.position.x,
                _moveTo.transform.position.x,
                Time.deltaTime*smooth),
            Mathf.Lerp(
                transform.position.y,
                _moveTo.transform.position.y,
                Time.deltaTime*smooth),
            Mathf.Lerp(
                transform.position.z,
                _moveTo.transform.position.z,
                Time.deltaTime*smooth));

        if ((newPosition - _moveTo.transform.position).magnitude < 0.1)
        {
            IsMovingToNewTarget = false;

            var angles = transform.eulerAngles;
            X = angles.y;
            Y = angles.x;

            _xSmooth = X;
            _ySmooth = Y;

            _posSmooth = Target.position;

            _xVelocity = 0;
            _yVelocity = 0;


            // transform.position = newPosition;
            float distance = Vector3.Magnitude(transform.position - Target.position);
            Distance = distance;
            // transform.LookAt(_moveTo.position);
            Target.SendMessage("OnFocus");
        }
        else
        {
            transform.position = newPosition;
            //transform.LookAt(Target);
            SmoothLookAt((newPosition - _moveTo.transform.position).magnitude);
        }
    }

    /// <summary>
    /// Smoothly moves Camera to new Target as long as distance between Target and Camera become equals Distance
    /// </summary>
    private void MoveCameraToTarget()
    {
        float curDistance = Vector3.Distance(transform.position, Target.position);
        const float smooth = 1;

        Vector3 newPosition;

        if (curDistance > _initDistance)
        {
            newPosition = new Vector3(
                Mathf.Lerp(
                    transform.position.x,
                    Target.position.x,
                    Time.deltaTime*smooth),
                Mathf.Lerp(
                    transform.position.y,
                    Target.position.y,
                    Time.deltaTime*smooth),
                Mathf.Lerp(
                    transform.position.z,
                    Target.position.z,
                    Time.deltaTime*smooth));
        }
        else if (curDistance < _initDistance)
        {
            Vector3 direction = (transform.position - Target.position).normalized;

            newPosition = new Vector3(
                Mathf.Lerp(
                    transform.position.x,
                    transform.position.x + direction.x,
                    Time.deltaTime*smooth),
                Mathf.Lerp(
                    transform.position.y,
                    transform.position.y + direction.y,
                    Time.deltaTime*smooth),
                Mathf.Lerp(
                    transform.position.z,
                    transform.position.z + direction.z,
                    Time.deltaTime*smooth));
        }
        else
        {
            newPosition = transform.position;
        }

        float distance = Vector3.Distance(newPosition, Target.position);

        if ((curDistance < _initDistance && distance >= _initDistance) ||
            (curDistance > _initDistance && distance <= _initDistance))
        {
            IsMovingToNewTarget = false;

            var angles = transform.eulerAngles;
            X = angles.y;
            Y = angles.x;

            _xSmooth = X;
            _ySmooth = Y;

            _posSmooth = Target.position;

            _xVelocity = 0;
            _yVelocity = 0;

            Distance = curDistance;
            Distance = curDistance;


            //transform.position = newPosition;

            Target.SendMessage("Focus");

            _isMoveToDistanceMode = false;
        }
        else
        {
            SmoothLookAt(Mathf.Abs(curDistance - _initDistance));
            transform.position = newPosition;
            //transform.LookAt(Target);
        }
    }

    /// <summary>
    /// Smooth look at Target.position 
    /// </summary>
    private void SmoothLookAt(float distance)
    {
        var rotation = Quaternion.LookRotation(Target.position - transform.position);
        var smooth = (1/(distance*distance*distance));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, smooth*Time.deltaTime);
    }

    private void ZoomOut()
    {
        if (Target)
        {
            Target.SendMessage("ZoomOut");
        }
    }

    private bool _pinching;

    private float _initFingersDistance;
    private float _lastFingerDistance;

    private void OnPinch(PinchGesture gesture)
    {
        if (gesture.Phase == ContinuousGesturePhase.Started)
        {
            _pinching = true;
            _initFingersDistance = gesture.Gap;
        }
        else if (gesture.Phase == ContinuousGesturePhase.Updated)
        {
            if (_pinching)
            {
                _lastFingerDistance = gesture.Gap;
            }
        }
        else if (gesture.Phase == ContinuousGesturePhase.Ended)
        {
            if (_initFingersDistance - _lastFingerDistance > 1)
            {
                if (Target)
                {
                    Target.SendMessage("ZoomOut");
                }
            }
        }
        else
        {
            if (_pinching)
            {
                _pinching = false;
            }
        }
    }

    private void Select()
    {
        _isInInteraction = true;
    }

    private void Deselect()
    {
        _isInInteraction = false;
    }
}