using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] FollowCam _follow_cam_ref;
    [SerializeField] AudioSource _pokeball_audio;
    void Update()
    {
        transform.Rotate(Vector3.forward * (80.0f * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Next Object")
        {
            _follow_cam_ref.StartRainbow();
            //_pokeball_audio.Play();
            Destroy(this.gameObject);
        }
    }
}
