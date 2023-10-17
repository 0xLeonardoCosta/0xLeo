using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    //public Transform target;
    public GameObject target;
    public float smoothing = 5f;
    //Vector3 offset;
    Vector3 offset = new Vector3 (0, 5, -13);

    void Start()
    {
        //offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        //Vector3 targetCamPos = target.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        transform.position = target.transform.position + offset;
        transform.position = Vector3.Lerp()
    }
}
