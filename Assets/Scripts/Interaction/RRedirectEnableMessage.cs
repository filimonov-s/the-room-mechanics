using UnityEngine;
using System.Collections;

public class RRedirectEnableMessage : RBaseInteraction
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    protected override void EnableInteraction()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SendMessage("EnableInteraction");
        }
    }

    protected override void DisableInteraction()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SendMessage("DisableInteraction");
        }
    }
}