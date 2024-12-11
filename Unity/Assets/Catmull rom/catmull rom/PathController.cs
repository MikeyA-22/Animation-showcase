using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;
    [SerializeField] float directionAdjuster = 180f;
    private List<waypoint> thePath;
    waypoint target;

    public float MoveSpeed;
    public float RotateSpeed;
    public GameObject parent;
    
    bool isSprinting;
    float blend = 0;

    private List<Vector3> splinePath;
    private int splineIndex;

    void Start()
    {
        isSprinting = true;
        splinePath = pathManager.GetCatmullRomPath();
        splineIndex = 0;
        
        thePath = pathManager.GetPath();
        if (thePath.Count != null && thePath.Count > 0)
        {
            target = thePath[0];
        }
    }

   

    void Update()
    {
        
        
            //isSprinting = !isSprinting;
            
        

        
            
            rotateTowardsTarget();
            moveForward();
            
        
        
            
    }

   

    private void rotateTowardsTarget()
    {
        if (splineIndex >= splinePath.Count) return;

        float stepSize = RotateSpeed * Time.deltaTime;
        Vector3 targetPos = splinePath[splineIndex];
        Vector3 targetDir = (targetPos - transform.position).normalized;
        targetDir = -targetDir;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, stepSize);
    }

    
    
    private void moveForward()
    {
        if (splineIndex >= splinePath.Count) return;
        float stepSize = MoveSpeed * Time.deltaTime;
        Vector3 targetPos = splinePath[splineIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetPos, stepSize);
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            splineIndex++;
            if (splineIndex >= splinePath.Count)
            {
                splineIndex = 0;
            }
        }
    }

   
  
}
