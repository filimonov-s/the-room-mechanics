using UnityEngine;
using System.Collections;

public class RKeyGrip : RBaseInteraction
{
    public bool IsFirstGrip;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
    }

    protected override void OnSelect()
    {
        if (IsFirstGrip)
        {
            transform.parent.SendMessage("Grip1Selected");
        } else
        {
            transform.parent.SendMessage("Grip2Selected");
        }
    }

   
}