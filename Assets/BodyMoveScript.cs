using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyMoveScript : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField] float _forwardMoveForce;
    [SerializeField] private float _turnTorque = 1;
    [SerializeField] float _maxLinearVelocity = 4;
    [SerializeField] float _maxAngularVelocity = 90;
    Vector2 _currentInput = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxAngularVelocity = _maxAngularVelocity;
        _rigidbody.maxLinearVelocity = _maxLinearVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        _currentInput.x = Input.GetAxis("Horizontal");
        _currentInput.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(transform.forward * _forwardMoveForce * Time.fixedDeltaTime * -_currentInput.y, ForceMode.Acceleration);
        _rigidbody.AddRelativeTorque(transform.up * _turnTorque * Time.fixedDeltaTime * -_currentInput.x, ForceMode.Acceleration);
        
    }
    
    
    
}
