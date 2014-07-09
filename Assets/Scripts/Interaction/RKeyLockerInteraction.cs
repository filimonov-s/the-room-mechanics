using UnityEngine;
using System.Collections;

public class RKeyLockerInteraction : RBaseInteraction
{
    public float RotationSpeed = 100;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (IsInInteraction && InteractionEnabled)
        {
            var rotation = Globals.MouseSpeedX * RotationSpeed* Time.deltaTime;
            transform.Rotate(0, 0, rotation);
        }
    }

    protected override void OnSelect()
    {
        print("S");
    }

    protected override void OnDeselect()
    {
        print("D");
    }
}