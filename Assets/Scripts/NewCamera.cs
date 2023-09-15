using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    [SerializeField] Transform _buri;
    Vector3 _offset;
    float _smoothing;

    void Start()
    {

    }

    void Update()
    {
        Vector3 positionOffset = new Vector3(0, 20, -25);
        Vector3 rotationOffset = new Vector4(30, 0, 0);

        _buri.position = new Vector3(transform.position.x, 0, transform.position.z);

        transform.position = positionOffset + _buri.position;
        transform.rotation = Quaternion.Euler(rotationOffset);
    }
}
