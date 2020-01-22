using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] GameObject _following;

    [SerializeField] float _smooth_factor;
    [SerializeField] Quaternion _turn_angle;
    [SerializeField] float _rotation_speed = 0.5f;
    [SerializeField] bool _changing_target = false;

    [SerializeField] Color _default_color;

    [SerializeField] AudioSource _pokeball_audio;

    private Camera _self_cam;
    private float _time_left = 0.5f;
    private float _star_timer = 7.0f;
    private bool _rainbow = false;

    private Vector3 _origin_position;
    private Quaternion _origin_rotation;
    private float _shake_intensity;
    private float _shake_decay;

    void Start()
    {
        // register offset of camera
        _offset = this.transform.position - _following.transform.position;
        _self_cam = this.GetComponent<Camera>();
        _default_color = _self_cam.backgroundColor;
    }

    void LateUpdate()
    {
        if (_changing_target)
        {
            Vector3 start_pos = this.transform.position;
            Vector3 end_pos = _following.transform.position + _offset;

            this.transform.position = Vector3.Lerp(start_pos, end_pos, Time.deltaTime * 5f);
            if(Tools.V3Equal(this.transform.position, end_pos))
            {
                _changing_target = false;
            }
        }
        else
        {
            CamControl();
        }

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
                _self_cam.backgroundColor = _default_color;
                _pokeball_audio.Stop();
                this.transform.GetChild(0).GetComponent<AudioSource>().UnPause();
            }
        }

        if (_shake_intensity > 0)
        {
            transform.position = _origin_position + Random.insideUnitSphere * _shake_intensity;
            transform.rotation = new Quaternion(
                            (float)(_origin_rotation.x + Random.Range(-_shake_intensity, _shake_intensity) * .2),
                            (float)(_origin_rotation.y + Random.Range(-_shake_intensity, _shake_intensity) * .2),
                            (float)(_origin_rotation.z + Random.Range(-_shake_intensity, _shake_intensity) * .2),
                            (float)(_origin_rotation.w + Random.Range(-_shake_intensity, _shake_intensity) * .2));
            _shake_intensity -= _shake_decay;
        }
    }

    public void StartRainbow()
    {
        _rainbow = true;
    }

    private void RainbowTime()
    {
        _time_left -= Time.deltaTime;
        if (_time_left < 0)
        {
            Rainbow();
            _time_left = 0.25f;
        }
    }

    private void Rainbow()
    {
        Color new_color = new Color(Random.value, Random.value, Random.value);
        _self_cam.backgroundColor = new_color;
    }

    public void SwitchTarget(GameObject obj)
    {
        _following = obj;
        _changing_target = true;
    }

    public void ShakeCamera()
    {
        _origin_position = transform.position;
        _origin_rotation = transform.rotation;
        _shake_intensity = 0.3f;
        _shake_decay = 0.02f;
    }

    private void CamControl()
    {
        // get mouse input to rotate around the object being followed
        _turn_angle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * _rotation_speed, Vector3.up);
        _offset = _turn_angle * _offset;

        // update the position of the camera
        Vector3 new_pos = _following.transform.position + _offset;
        this.transform.position = Vector3.Slerp(this.transform.position, new_pos, _smooth_factor);
        
        // look at the object being followed
        this.transform.LookAt(_following.transform);
    }
}
