using UnityEngine;
using System.Collections;

public class RCircleFastInteraction : RBaseInteraction
{
    public GameObject TouchPlane;
    public GameObject Grip;

    public float Angle;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (IsInInteraction && InteractionEnabled)
        {
            Vector2 gripVector = new Vector2(
                Grip.transform.position.z - transform.position.z,
                Grip.transform.position.y - transform.position.y
                );

            Vector2 mouseVector = new Vector2(
                RSelector.HittedPoint.z - transform.position.z,
                RSelector.HittedPoint.y - transform.position.y
                );

            float angle = Vector3.Angle(gripVector, mouseVector);
            
            float dot =
                gripVector.x*mouseVector.y -
                gripVector.y*mouseVector.x;

            if (dot > 0)
                angle *= -1;

            Angle = angle;
            transform.Rotate(0, 0, angle);

        }
    }

    protected override void OnDeselect()
    {
        RSelector.SelectorTarget = RSelectorTarget.All;
        TouchPlane.SetActive(false);
        TouchPlane.collider.enabled = false;
    }

    protected override void OnSelect()
    {
        RSelector.SelectorTarget = RSelectorTarget.DirectionPlanes;
        TouchPlane.SetActive(true);
        TouchPlane.collider.enabled = true;
    }
}