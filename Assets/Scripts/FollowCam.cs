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

    void Start()
    {
        // register offset of camera
        _offset = this.transform.position - _following.transform.position;
    }

    void LateUpdate()
    {
        CamControl();
    }

    public void SwitchTarget(GameObject obj)
    {
        _following = obj;
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
