using System;
using UnityEngine;
using System.Collections;

public class RSelector : MonoBehaviour
{
    public static RSelectorTarget SelectorTarget;
    public static GameObject SelectedObject;
    public static GameObject HittedObject;
    public static Vector3 HittedPoint;
    public static Boolean Enabled = true;

    private float _lastClick;
    private GameObject _prevClickObject;

    private Vector2 _touch1Pos;
    private Vector2 _touch2Pos;

    // Use this for initialization
    private void Start()
    {
    }

    private void ProcessTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _touch1Pos = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
        {
            _touch2Pos = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 1 &&
            (Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if (
                Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) -
                Vector2.Distance(_touch1Pos, _touch2Pos) > 1f)
            {
                Camera.mainCamera.SendMessage("ZoomOut");
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Enabled)
        {
            bool doubleClick = false;

            Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            switch (SelectorTarget)
            {
                default:
                case RSelectorTarget.All:
                    Physics.Raycast(ray, out raycastHit, Mathf.Infinity);
                    Debug.DrawRay(raycastHit.point, Input.mousePosition, Color.green);
                    break;

                case RSelectorTarget.DirectionPlanes:
                    Physics.Raycast(ray, out raycastHit, Mathf.Infinity, 1 << 8);
                    Debug.DrawRay(raycastHit.point, Input.mousePosition, Color.red);
                    break;


                case RSelectorTarget.Selectable:
                    throw new NotSupportedException();
                    //Debug.DrawRay(raycastHit.point, Input.mousePosition, Color.yellow);
                    //break;
            }

            if (Input.touchCount > 1 && SelectedObject)
            {
                SelectedObject.SendMessage("Deselect");
                SelectedObject = null;
            }

            HittedPoint = raycastHit.point;

            //ProcessTouch();

            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time - _lastClick < 0.2)
                {
                    _lastClick = 0;
                    doubleClick = true;
                    print("DC");
                    //   Camera.mainCamera.SendMessage("ZoomOut");
                }
                else
                {
                    _lastClick = Time.time;
                }
            }

            if (raycastHit.collider != null)
            {
                HittedObject = raycastHit.collider.gameObject;
                // Debug.Log("Hitted: " + HittedObject.name);


                if (doubleClick && raycastHit.collider.gameObject.tag == "ZoomIn")
                {
                    _lastClick = 0;
                    SelectedObject = HittedObject;
                    SelectedObject.SendMessage("Select");
                    //Debug.Log("Selected: " + SelectedObject.name);
                    return;
                }


                /*  if (doubleClick && raycastHit.collider.gameObject.tag == "ZoomOut")
                {
                    _lastClick = 0;
                    Camera.mainCamera.SendMessage("ZoomOut");
                    return;
                }*/


                if (Input.GetMouseButtonDown(0) && !doubleClick && raycastHit.collider.gameObject.tag == "Select")
                {
                    //_lastClick = 0;
                    SelectedObject = HittedObject;
                    SelectedObject.SendMessage("Select");
                    //Debug.Log("Selected: " + SelectedObject.name);
                    return;
                }
            }

            if (Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                if (SelectedObject)
                {
                    SelectedObject.SendMessage("Deselect");
                    SelectedObject = null;
                }
            }
                //Select Camera
            else if (Input.GetMouseButtonDown(0) && !doubleClick && SelectedObject == null &&
                     (raycastHit.collider == null || raycastHit.collider.gameObject.tag != "NoCameraSelect"))
            {
                SelectedObject = Camera.mainCamera.gameObject;
                Camera.mainCamera.gameObject.SendMessage("Select");
                //Debug.Log("Selected: " + SelectedObject.name);
            }
#if UNITY_EDITOR
            else if (Input.GetMouseButtonDown(1))
            {
                Camera.mainCamera.SendMessage("ZoomOut");
            }
#endif
        }
    }

    private void RedirectSelection(GameObject target)
    {
        SelectedObject = target;
        SendMessageSelected();
    }

    private void RedirectDeselection(GameObject target)
    {
        SelectedObject = target;
        SendMessageDeselected();
    }


    private void SendMessageSelected()
    {
        SelectedObject.SendMessage("Select");
    }

    private void SendMessageDeselected()
    {
        SelectedObject.SendMessage("Deselect");
    }
}