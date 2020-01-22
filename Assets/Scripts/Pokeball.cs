using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    public Rigidbody pokeball_rb;

    [SerializeField] CaptureManager _capture_manager;
    [SerializeField] FollowCam _follow_cam_ref;

    [SerializeField] Tank _tank_ref;

    // Start is called before the first frame update
    void Start()
    {
        this.pokeball_rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Pokemon")
        {
            _capture_manager.hit_offset = collision.transform.position;
            _capture_manager.ThrowPokeball();
        }
        if(collision.gameObject.tag == "Cannon")
        {
            this.gameObject.SetActive(false);
            this.transform.rotation = Quaternion.identity;
            _follow_cam_ref.SwitchTarget(collision.gameObject);
            _tank_ref.Shoot();
        }
    }
}
