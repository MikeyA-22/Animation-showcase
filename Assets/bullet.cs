using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class bullet : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField]private GameObject aimAt;
    [SerializeField]private float _speed = 25f;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        aimAt = GameObject.FindGameObjectWithTag("Aim");
        transform.Rotate(new Vector3(90, 0, 0));
        transform.LookAt(aimAt.transform);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        _rigidbody.velocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(this.gameObject);
    }
}
