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

    [SerializeField] Color _top_original_color;
    [SerializeField] Color _bottom_original_color;

    [SerializeField] AudioSource _theme;

    [Space]
    [SerializeField] Renderer _top_lid;
    [SerializeField] Renderer _bottom_lid;

    [Space]
    [SerializeField] ParticleSystem _rainbow_effect;

    private float _time_left = 0.15f;
    private float _star_timer = 5.0f;
    private bool _rainbow = false;

    // Start is called before the first frame update
    void Start()
    {
        this.pokeball_rb = GetComponent<Rigidbody>(); // reference to the RB component
        _top_original_color = _top_lid.material.color;
        _bottom_original_color = _bottom_lid.material.color;
    }

    private void Update()
    {
        if (_rainbow)
        {
            _star_timer -= Time.deltaTime;
            if (_star_timer > 0)
            {
                RainbowTime();
            }
            else
            {
                _rainbow = false;
                _rainbow_effect.Stop();
                _top_lid.material.color = _top_original_color;
                _bottom_lid.material.color = _bottom_original_color;
                this.GetComponent<AudioSource>().Stop();
                _theme.volume = 0.1f;
            }
        }
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

    public void StartRainbow()
    {
        _rainbow = true;
        _rainbow_effect.Play();
    }

    private void RainbowTime()
    {
        _time_left -= Time.deltaTime;
        if (_time_left < 0)
        {
            Rainbow();
            _time_left = 0.15f;
        }
    }

    private void Rainbow()
    {
        Color rand_color = new Color(Random.value, Random.value, Random.value);
        _top_lid.material.color = _bottom_lid.material.color = rand_color;
    }
}
