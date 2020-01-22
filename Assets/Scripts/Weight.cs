using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weight : MonoBehaviour
{
    [SerializeField] FollowCam _follow_cam_ref;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Next Object")
        {
            _follow_cam_ref.SwitchTarget(collision.gameObject);
        }
    }
}
