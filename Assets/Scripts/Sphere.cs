using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    [SerializeField] FollowCam _follow_cam_ref;
    private void OnCollisionEnter(Collision collision)
    {
        // if the sphere collides with the next object in the sequence
        if(collision.gameObject.tag == "Next Object")
        {
            // switch the camera target
            _follow_cam_ref.SwitchTarget(collision.gameObject);

            // set angular drag to prevent the ball from rolling off into the infinite void
            this.GetComponent<Rigidbody>().angularDrag = 20.0f;
        }
    }
}
