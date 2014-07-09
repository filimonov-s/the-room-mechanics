using UnityEngine;
using System.Collections;

public class CollisionFixScript : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionStay(Collision hit)
    {
        //_onTouch = true;
        //Debug.Log("Collision name: " + hit.gameObject.name);
        foreach (ContactPoint point in hit.contacts)
        {
            //Debug.Log("contact: " + point.normal.ToString()); 
            if (hit.rigidbody)
                rigidbody.AddForce(point.normal*hit.rigidbody.mass*100f);
        }
        //Debug.Break(); 
    }
}