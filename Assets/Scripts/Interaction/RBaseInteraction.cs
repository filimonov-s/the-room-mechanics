using UnityEngine;
using System.Collections;

public abstract class RBaseInteraction : MonoBehaviour
{
    protected bool IsInInteraction;
    protected bool InteractionEnabled;
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    protected void Select()
    {
        IsInInteraction = true;
        OnSelect();
    }

    protected void Deselect()
    {
        IsInInteraction = false;
        OnDeselect();
    }

    protected void Focus()
    {
        OnFocus();
        SendMessageUpwards("ChildFocus", SendMessageOptions.DontRequireReceiver);
    }

    protected void FocusLost()
    {
        OnFocusLost();
        SendMessageUpwards("ChildFocusLost", SendMessageOptions.DontRequireReceiver);
    }

    protected void ChildFocus()
    {
        OnChildFocus();
    }

    protected void ChildFocusLost()
    {
        OnChildFocusLost();
    }

    protected void ZoomOut()
    {
        OnZoomOut();
    }

    protected virtual void OnZoomOut()
    {

    }


    /// <summary>
    /// Camera calls this method when get it focused on this object
    /// </summary>
    protected virtual void OnFocus()
    {
    }

    /// <summary>
    /// Camera calls this method when it looses focus from this object
    /// </summary>
    protected virtual void OnFocusLost()
    {
    }

    /// <summary>
    /// This method is called by RSelector on End Touch or Mouse Up
    /// </summary>
    protected virtual void OnDeselect()
    {
    }

    /// <summary>
    /// This method is called by RSelector when this object is choosed
    /// </summary>
    protected virtual void OnSelect()
    {
    }


    /// <summary>
    /// This method is called by RSelector when child target is focused
    /// </summary>
    protected virtual void OnChildFocusLost()
    {
    }

    /// <summary>
    /// This method is called by RSelector when child target lost focus
    /// </summary>
    protected virtual void OnChildFocus()
    {
    }

    protected virtual void EnableInteraction()
    {
        InteractionEnabled = true;
    }

    protected virtual void DisableInteraction()
    {
        InteractionEnabled = false;
    }
}