using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : MonoBehaviour
{
    public Rigidbody pokeball_rb;

    [SerializeField] CaptureManager _capture_manager;

    // Start is called before the first frame update
    void Start()
    {
        this.pokeball_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Pokemon")
        {
            _capture_manager.hit_offset = collision.transform.position;
            _capture_manager.ThrowPokeball();
        }
    }
}
