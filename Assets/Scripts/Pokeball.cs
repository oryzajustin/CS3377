using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    public Rigidbody pokeball_rb;

    [SerializeField] CaptureManager _capture_manager;
    [SerializeField] FollowCam _follow_cam_ref;

    [SerializeField] Tank _tank_ref;

    [SerializeField] List<AudioSource> _sound_effects;

    // Start is called before the first frame update
    void Start()
    {
        this.pokeball_rb = GetComponent<Rigidbody>(); // reference to the RB component
    }

    public List<AudioSource> GetSoundEffects()
    {
        return _sound_effects; // public method for getting the pokemon sound effects
    }

    private void OnCollisionEnter(Collision collision)
    {
        // collide with pokemon => catch sequence
        if(collision.gameObject.tag == "Pokemon")
        {
            _capture_manager.hit_offset = collision.transform.position;
            pokeball_rb.isKinematic = true;
            _capture_manager.ThrowPokeball();
        }
        // collide with cannon => shoot out pokeball
        if(collision.gameObject.tag == "Cannon")
        {
            this.gameObject.SetActive(false);
            this.transform.rotation = Quaternion.identity;
            _follow_cam_ref.SwitchTarget(collision.gameObject);
            _tank_ref.Shoot();
        }
        // edge case catch to prevent pokeball from making noise when first starting up
        if(collision.gameObject.tag != "Initial")
        {
            _sound_effects[2].Play();
        }
    }
}
