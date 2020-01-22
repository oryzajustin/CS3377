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
        // if an object enters the star, trigger the rainbow effect
        if (other.gameObject.tag == "Next Object")
        {
            // call rainbow function from the follow cam, pause the overworld music, play the star theme attached to the pokeball
            _follow_cam_ref.StartRainbow();
            _follow_cam_ref.gameObject.transform.GetChild(0).GetComponent<AudioSource>().Pause();
            _pokeball_audio.Play();
            Destroy(this.gameObject);
        }
    }
}
