using UnityEngine;
using System.Collections;

public class RSelectRediretorInteraction : RBaseInteraction
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

    protected override void OnSelect()
    {
        Camera.mainCamera.SendMessage("RedirectSelection", Target);
    }



    
}