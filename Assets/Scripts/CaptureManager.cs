using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class CaptureManager : MonoBehaviour
{
    [Header("Public References")]
    [SerializeField] Transform _pokeball;
    [SerializeField] Transform _pokemon;
    [Space]
    [Header("Throw Settings")]
    ////[SerializeField] float throwArc;
    ////[SerializeField] float throwDuration;
    [Space]
    [Header("Hit Settings")]
    public Vector3 hit_offset;
    [SerializeField] Transform _jump_to;
    [SerializeField] float _jump_power = .5f;
    [SerializeField] float _jump_duration;
    [Space]
    [Header("Open Settings")]
    [SerializeField] float _open_angle;
    [SerializeField] float _open_duration;
    [Space]
    [Header("Open Settings")]
    [SerializeField] float _fall_duration = .6f;
    [Space]
    [Header("Cameras Settings")]
    [SerializeField] GameObject _second_camera;
    [SerializeField] float _final_zoom_duration = .5f;

    [SerializeField] Pokeball _pokeball_ref;

    //[Space]
    //[Header("Particles")]
    //public ParticleSystemForceField forceField;
    //public ParticleSystem throwParticle;
    //public ParticleSystem firstLines;
    //public ParticleSystem firstCircle;
    //public ParticleSystem firstFlash;
    //public ParticleSystem firstDust;
    //public ParticleSystem beam;

    //[Space]
    //public ParticleSystem capture1;
    //public ParticleSystem capture2;
    //public ParticleSystem capture3;

    //[Space]
    //public ParticleSystem yellowBlink;
    //public ParticleSystem blueBlink;
    //public ParticleSystem finalCircle;
    //public ParticleSystem stars;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ThrowPokeball();
        }

        if (Input.GetKey(KeyCode.S))
        {
            Time.timeScale = .5f;
        }
        else
        {
            Time.timeScale = 1;
        }

        _second_camera.transform.LookAt(_pokeball);
    }

    public void ThrowPokeball()
    {

        Sequence throwSequence = DOTween.Sequence();

        //turn off gravity for the animation
        _pokeball_ref.pokeball_rb.useGravity = false;

        ////Throw the pokeball
        //throwSequence.Append(pokeball.DOJump(pokemon.position + hitOffset, throwArc, 1, throwDuration));
        //throwSequence.Join(pokeball.DORotate(new Vector3(300, 0, 0), throwDuration, RotateMode.FastBeyond360));

        //Pokeball Jump
        throwSequence.Append(_pokeball.DOJump(_jump_to.position, _jump_power, 1, _jump_duration));
        throwSequence.Join(_pokeball.DOLookAt(_pokemon.position, _jump_duration));

        //throwSequence.AppendCallback(() => throwParticle.Stop());
        //throwSequence.AppendCallback(() => firstCircle.Play());
        //throwSequence.AppendCallback(() => firstLines.Play());
        //throwSequence.AppendCallback(() => firstFlash.Play());
        //throwSequence.AppendCallback(() => firstDust.Play());

        //Pokemon Disappear
        throwSequence.AppendCallback(() => PokemonDisappear());

        //Pokeball Open
        throwSequence.Append(_pokeball.GetChild(0).GetChild(0).DOLocalRotate(new Vector3(-_open_angle, 0, 0), _open_duration).SetEase(Ease.OutBack));
        throwSequence.Join(_pokeball.GetChild(1).DOLocalRotate(new Vector3(_open_angle, 0, 0), _open_duration).SetEase(Ease.OutBack));

        //throwSequence.AppendCallback(() => forceField.gameObject.SetActive(true));
        //throwSequence.Join(firstDust.transform.DORotate(new Vector3(0, 0, 100), .5f, RotateMode.FastBeyond360));

        //throwSequence.Join(beam.transform.DOMove(jumpPosition.position, .2f));

        //Camera Change
        throwSequence.AppendCallback(() => ChangeCamera());

        //Pokeball Close
        throwSequence.Append(_pokeball.GetChild(0).GetChild(0).DOLocalRotate(Vector3.zero, _open_duration / 3));
        throwSequence.Join(_pokeball.GetChild(1).DOLocalRotate(Vector3.zero, _open_duration / 3));

        //throwSequence.AppendCallback(() => capture1.Play());
        //throwSequence.AppendCallback(() => capture2.Play());
        //throwSequence.AppendCallback(() => capture2.Play());
        throwSequence.AppendCallback(() => _second_camera.transform.DOShakePosition(.2f, .1f, 15, 90, false, true));

        throwSequence.Join(_pokeball.DORotate(Vector3.zero, _open_duration / 3).SetEase(Ease.OutBack));

        //Interval
        throwSequence.AppendInterval(.3f);

        //Pokeball Fall
        throwSequence.Append(_pokeball.DOMoveY(-0.4f, _fall_duration).SetEase(Ease.OutBounce));
        throwSequence.Join(_pokeball.DOPunchRotation(new Vector3(-40, 0, 0), _fall_duration, 5, 10));
    }

    private void PokemonDisappear()
    {
        _pokemon.DOScale(0, .3f);
    }

    private void ChangeCamera()
    {
        _second_camera.SetActive(true);
        Camera.main.gameObject.SetActive(false);

        Transform cam = _second_camera.transform;

        Sequence cameraSequence = DOTween.Sequence();
        cameraSequence.Append(cam.DOMoveY(.3f, 1.5f)).SetDelay(.5f);

        cameraSequence.AppendInterval(.5f);
        cameraSequence.Append(cam.DOMoveZ(.3f, _final_zoom_duration).SetEase(Ease.InExpo));

        //Particle
        //cameraSequence.AppendCallback(() => yellowBlink.Play());
        cameraSequence.Join(_pokeball.DOShakeRotation(.5f, 30, 8, 70, true));

        cameraSequence.AppendInterval(.8f);
        cameraSequence.Append(cam.DOMoveZ(-0.5f, _final_zoom_duration).SetEase(Ease.InExpo));


        //Particle
        //cameraSequence.AppendCallback(() => yellowBlink.Play());
        cameraSequence.Join(_pokeball.DOShakeRotation(.5f, 20, 8, 70, true));

        cameraSequence.AppendInterval(1.2f);
        cameraSequence.Append(cam.DOMoveZ(-1.0f, _final_zoom_duration).SetEase(Ease.InExpo));

        //Particle
        //cameraSequence.AppendCallback(() => yellowBlink.Play());
        cameraSequence.Join(_pokeball.DOShakeRotation(.5f, 10, 8, 70, true));

        cameraSequence.AppendInterval(.8f);

        //Particle
        //cameraSequence.AppendCallback(() => blueBlink.Play());
        //cameraSequence.AppendCallback(() => finalCircle.Play());
        //cameraSequence.AppendCallback(() => stars.Play());
        cameraSequence.AppendCallback(() => _second_camera.transform.DOShakePosition(.2f, .1f, 7, 90, false, true));

        cameraSequence.Append(_pokeball.DOPunchRotation(new Vector3(-10, 0, 0), .5f, 8, 1));



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_pokemon.position + hit_offset, .2f);
    }
}
