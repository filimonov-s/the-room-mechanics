using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 170, 10, 150, 50), "Reset"))
        {
            Application.LoadLevel(0);
        }
    }
}