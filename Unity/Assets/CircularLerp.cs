using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class CircularLerp : MonoBehaviour
{
    private Vector2[] points = new Vector2[]
    {
        new Vector2(0, 1),
        new Vector2(1, 0),
        new Vector2(0, -1),
        new Vector2(-1, 0)
    };

    private int currentPointIndex = 0;
    private bool isLerping = false;
    private float lerpStartTime;
    private Rigidbody _rigidbody;
    private float animSpeed = 8;
    

    private Animator animator;

    [Tooltip("Adjust the movement speed of the character")]
    [SerializeField] private float movementSpeed = 5f;
    [Tooltip("Adjust the rotation speed of the character")]
    [SerializeField] private float rotationSpeed = 100f;
    [Tooltip("Reference to the camera used for direction alignment")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float runningSpeed = 8f;
    [SerializeField] private float speedWithBody = 0.5f;
    [SerializeField] private Transform lookAt;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Cinemachine.CinemachineFreeLook playerFreeLook;
    [SerializeField] private Cinemachine.CinemachineImpulseSource cameraShake;
    [SerializeField] private float verticalRecoil;
    [SerializeField] private float duration;
    
    private float time;
    private Vector3 lookAtPosition;
    private Vector2 lastPosition;
    private Vector3 lastVelocity = Vector3.zero;

    private void Start()
    {
        animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        cameraShake = GetComponent<Cinemachine.CinemachineImpulseSource>();
        lastPosition = points[0]; // Start at the first point
        lookAtPosition = lookAt.position;

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Automatically assign the main camera if not set
        }
    }


    private void FixedUpdate()
    {
        float yawCamera = cameraTransform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0,yawCamera,0), 5f * Time.deltaTime);
    }

    void Update()
    {
        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        
        Vector3 inputDirection = CalculateCameraRelativeInput(horizontal, vertical);
        
        animator.SetFloat("x_input", horizontal);
        animator.SetFloat("y_input", vertical);

        //transform.rotation = cameraTransform.rotation;
        
        _rigidbody.MovePosition(transform.position + inputDirection * (movementSpeed * Time.deltaTime));
        
        if (inputDirection.magnitude != 0f)
        {
            //RotateCharacterToDirection(lookAtPosition);
            
            //MoveCharacter(inputDirection);

            if (!isLerping)
            {
                StartLerp();
            }

            LerpToPoint();
        }
        else
        {
            StopLerp();
        }

        
        animator.SetFloat("movement", inputDirection.magnitude * movementSpeed);

        if (Input.GetKey(KeyCode.Space))
        {
            movementSpeed = 8;
            animSpeed = 16;
        }
        else
        {
            movementSpeed = 3;
            animSpeed = 8;
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            movementSpeed = 0.5f;
            animator.SetBool("hasBody", true);
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            movementSpeed = 0.5f;
            animator.SetBool("hasBody", false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(bullet, bulletSpawnPoint.position, bullet.transform.rotation);
            GenerateRecoil();
            
        }
    }

    private void GenerateRecoil()
    {
        time = duration;
        if (time > 0)
        {
            playerFreeLook.m_YAxis.Value -= (verticalRecoil * Time.deltaTime)/duration;
            time -= Time.deltaTime;
        }
        
        cameraShake.GenerateImpulse(cameraTransform.forward);
        
    }

    private Vector3 CalculateCameraRelativeInput(float horizontal, float vertical)
    {
        
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        
        cameraForward.Normalize();
        cameraRight.Normalize();

        
        return (cameraForward * vertical + cameraRight * horizontal).normalized;
    }

    private void StartLerp()
    {
        isLerping = true;
        lerpStartTime = Time.time;
    }

    private void StopLerp()
    {
        isLerping = false;

        
        animator.SetFloat("Movement_axis_x", lastPosition.x);
        animator.SetFloat("Movement_axis_y", lastPosition.y);
    }

    private void LerpToPoint()
    {
        Vector2 startPoint = points[currentPointIndex];
        Vector2 endPoint = points[(currentPointIndex + 1) % points.Length];

        float timeSinceStart = Time.time - lerpStartTime;
        float t = timeSinceStart * animSpeed;

        float x = Mathf.Lerp(startPoint.x, endPoint.x, t);
        float y = Mathf.Lerp(startPoint.y, endPoint.y, t);
        Vector2 position = new Vector2(x, y);

        animator.SetFloat("Movement_axis_x", position.x);
        animator.SetFloat("Movement_axis_y", position.y);

        lastPosition = position;

        if (t >= 1f)
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length;
            lerpStartTime = Time.time;
        }
    }

    private void RotateCharacterToDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void MoveCharacter(Vector3 direction)
    {
        Vector3 velocity = direction * movementSpeed;
        _rigidbody.MovePosition(transform.position + velocity * Time.deltaTime);
        lastVelocity = velocity;
    }


    
}
