using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using Filo;

public class Wedge : MonoBehaviour
{
    public Material crossMaterial;

    public LayerMask layerMask;

    [SerializeField] Cable _cable_ref;
    [SerializeField] GameObject _cable_solver_ref;
    [SerializeField] Bounds _bounds;

    [SerializeField] FollowCam _follow_cam_ref;
    [SerializeField] GameObject _weight;
    private void Start()
    {
        _bounds = GetComponent<MeshRenderer>().bounds;
    }

    // Update is called once per frame
    void Update()
    {
        Slice();
    }

    public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(this.transform.position, /*new Vector3(5, 0.1f, 5)*/ _bounds.size, this.transform.rotation, layerMask);

        if (hits.Length <= 0)
        {
            return;
        }

        for (int i = 0; i < hits.Length; i++)
        {
            // if you slice a cable anchor
            if(hits[i].gameObject.tag == "Cable Anchor")
            {
                SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
                if (hull != null)
                {
                    // slice into top and bottom
                    GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                    GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);

                    // place the bottom object into the heirarchy, and rename for debugging purposes
                    bottom.transform.SetParent(_cable_solver_ref.transform);
                    bottom.name = "HELLO";
                    bottom.AddComponent<CablePoint>();

                    // make a new cable link since the old game object will be destroyed 
                    // and will give a null reference
                    Cable.Link new_link = new Cable.Link();
                    new_link.type = Cable.Link.LinkType.Attachment;
                    new_link.body = bottom.GetComponent<CablePoint>();
                    new_link.inAnchor = new Vector3(0, 0, 0);
                    new_link.outAnchor = new_link.inAnchor;
                    new_link.cableSpawnSpeed = 0;

                    //Debug.Log(new_link.body.gameObject.name);

                    if (_cable_ref.links[0].body.tag == "Cable Anchor") // the first index of the links list is always the main anchor point
                    {
                        _cable_ref.links[0] = new_link; // set the new link
                        _cable_ref.Setup(); // reinitialize the rope simulation
                    }
                    AddHullComponents(bottom); // add components to each hull object
                    AddHullComponents(top);
                    Destroy(hits[i].gameObject); // destroy the old object for the (illusion) of being cut in half
                }
            }
            
        }
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 8; // place new generated objects on layer 8 (cuttable layer mask)
        Rigidbody rb = go.AddComponent<Rigidbody>(); // add rigidbody
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        rb.mass = 0.2f;
        rb.drag = 1.0f;
        rb.angularDrag = 3.0f;
        MeshCollider collider = go.AddComponent<MeshCollider>(); // add collider
        collider.convex = true;

        rb.AddExplosionForce(50, go.transform.position, 10); // apply small explosion force to the object, to separate and put in motion
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
        {
            return null;
        }

        return obj.Slice(this.transform.position, this.transform.up, crossSectionMaterial);
    }
}
