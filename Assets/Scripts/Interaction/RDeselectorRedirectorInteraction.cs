using UnityEngine;
using System.Collections;

public class RDeselectorRedirectorInteraction : RBaseInteraction
{
    public GameObject Target;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    protected override void OnDeselect()
    {
        Camera.mainCamera.SendMessage("RedirectDeselection", Target);
    }
}