﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] Pokeball _pokeball_ref;
    [SerializeField] Transform _muzzle;

    [SerializeField] FollowCam _follow_cam_ref;

    [SerializeField] float _time_left;

    private AudioSource _tank_shot;
    private ParticleSystem _smoke_particles;
    private bool _shoot = false;

    private void Start()
    {
        _tank_shot = this.GetComponent<AudioSource>();
        _smoke_particles = this.transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_shoot)
        {
            _time_left -= Time.deltaTime;
            if (_time_left < 0) // fire after timer is done
            {
                Fire();
                _shoot = false;
            }
        }
    }

    public void Shoot()
    {
        _shoot = true;
    }

    private void Fire()
    {
        // set up the pokeball in the new position
        _pokeball_ref.gameObject.transform.position = _muzzle.position;
        // set it to active and apply a force in the muzzle forward direction by a factor of 600
        _pokeball_ref.gameObject.SetActive(true);
        _pokeball_ref.pokeball_rb.AddForce(_muzzle.forward * 600);

        // play the sound effect and particles
        _tank_shot.Play();
        _smoke_particles.Play();

        // shake the camera for effect,
        _follow_cam_ref.ShakeCamera();

        // move the tank back slightly
        this.GetComponent<Rigidbody>().AddForce(this.transform.forward * -150);

        // switch perspective to the pokeball
        //_follow_cam_ref.SwitchTarget(_pokeball_ref.gameObject);
        StartCoroutine(SwitchCamera());
    }
    IEnumerator SwitchCamera()
    {
        //yield on a new YieldInstruction that waits for 0.4 seconds.
        yield return new WaitForSeconds(0.4f);

        //After we have waited 0.4 seconds, switch camera target
        _follow_cam_ref.SwitchTarget(_pokeball_ref.gameObject);
    }
}
