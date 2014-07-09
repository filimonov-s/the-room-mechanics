using UnityEngine;
using System.Collections;

public class RPushInteraction : RBaseInteraction
{
    public GameObject TouchPlane;
    private Vector3 _initPosition;

    // Use this for initialization
    private void Start()
    {
        _initPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!IsInInteraction) return;
        
        
        /*transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, RSelector.HittedPoint.x, 0.1f),
                _initPosition.y,
                Mathf.Lerp(transform.position.z, RSelector.HittedPoint.z, 0.1f)
                );*/
        var target = RSelector.HittedPoint - transform.position;
        target.y = 0;
            
        //rigidbody.AddForce(target * 100);
        rigidbody.velocity = target*15;

        /*Vector3 target = RSelector.HittedPoint;
            target.y = _initPosition.y;
            rigidbody.velocity = transform.position - target;*/
    }

    protected override void OnSelect()
    {
        RSelector.SelectorTarget = RSelectorTarget.DirectionPlanes;
        collider.enabled = true;
        TouchPlane.SetActive(true);
        TouchPlane.collider.enabled = true;


        transform.position = new Vector3(
            RSelector.HittedPoint.x,
            _initPosition.y,
            RSelector.HittedPoint.z
            );
    }

    protected override void OnDeselect()
    {
        RSelector.SelectorTarget = RSelectorTarget.All;
        collider.enabled = false;
        TouchPlane.SetActive(false);
        TouchPlane.collider.enabled = false;
    }

    private void OnCollisionStay(Collision hit)
    {
        //_onTouch = true;
        //Debug.Log("Collision name: " + hit.gameObject.name);
        foreach (ContactPoint point in hit.contacts)
        {
            //Debug.Log("contact: " + point.normal.ToString()); 
            //rigidbody.AddForce(point.normal*20f*rigidbody.mass);
        }
        //Debug.Break(); 
    }
}