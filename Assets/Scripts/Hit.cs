using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hit : MonoBehaviour
{
    Animator _anim;
    public bool _foiHit = false;
    public GameObject _parent;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {

    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "BuriTrigger") // se
        {

            _foiHit = true;
            // Morre();
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "BuriTrigger") // se
        {

            _foiHit = false;
            // Morre();
        }
    }
    void Morre()
    {
        _parent.gameObject.SetActive(false); // desativar pai
        //transform.parent.gameObject.SetActive(false); // desativar pai
    }
}
