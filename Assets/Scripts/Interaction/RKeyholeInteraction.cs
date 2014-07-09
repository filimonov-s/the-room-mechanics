using UnityEngine;
using System.Collections;

public class RKeyholeInteraction : RBaseInteraction
{
    public bool Opened;
    public GameObject Key;
    public GameObject Cover;

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
        if (Opened)
        {
            Key.SetActive(true);
            Key.SendMessage("EnableInteraction", SendMessageOptions.DontRequireReceiver);
            Cover.SendMessage("SetDisabled");
        }
    }


    private void SetOpened()
    {
        Opened = true;
    }

    private void SetClosed()
    {
        Opened = false;
    }

    private void OnTriggerStay(Collider other)
    {
        SetClosed();
    }

    private void OnTriggerEnter(Collider other)
    {
        SetClosed();
    }

    private void OnTriggerExit(Collider other)
    {
        SetOpened();
    }
}