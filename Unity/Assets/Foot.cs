using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Foot : MonoBehaviour
{
    
    
    [SerializeField] Transform _placementTarget;
    [SerializeField] Transform _bodyTransform;
    [SerializeField] private float _stepSize = 1f;
    [SerializeField] private float _footSpeed = 3f;
    [SerializeField] private float _liftHeight = 1.5f;
    [SerializeField] private float _minDistanceTolerance = 0.1f;
    [SerializeField] Foot _opposingFoot;
    
    Vector3 _targetPosition = Vector3.zero;
    
    
    public enum StepPhases
    {
        RESTING,
        MOVING_TO_TARGET,
        MOVING_TO_LIFT
        
    }
    
    
    public StepPhases _currentPhase = StepPhases.MOVING_TO_LIFT;
    public UnityEvent<bool> OnPlantedChange;
    // Start is called before the first frame update
    void Start()
    {
        _targetPosition = _placementTarget.position;//setpos to target at rest    
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _placementTarget.position) > _stepSize &&
            _currentPhase == StepPhases.RESTING && _opposingFoot._currentPhase == StepPhases.RESTING)// if we need to lift
        {
            _targetPosition = GetLiftPosition();
            _currentPhase = StepPhases.MOVING_TO_LIFT;
            OnPlantedChange?.Invoke(false);
        }

        if (Vector3.Distance(transform.position, _targetPosition) < _minDistanceTolerance &&
            _currentPhase == StepPhases.MOVING_TO_LIFT) //if we are close to lift
        {
            _targetPosition = _placementTarget.position;
            _currentPhase = StepPhases.MOVING_TO_TARGET;
        }

        if (Vector3.Distance(transform.position, _targetPosition) < _minDistanceTolerance &&
            _currentPhase == StepPhases.MOVING_TO_TARGET) //if we are close to final placement
        {
            _currentPhase = StepPhases.RESTING;//rest til next movement
            OnPlantedChange?.Invoke(true);
        }

        Move();
    }

    private void Move()
    {
        if (_currentPhase != StepPhases.RESTING)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * _footSpeed);
        }
    }


    private Vector3 GetLiftPosition()
    {
        Vector3 midPointDistance = (_placementTarget.position - transform.position)/2;//find the vector to add to the current position to get midpoint
        Vector3 midPoint = transform.position + midPointDistance;// the middle of a distance between currentpos and target pos
        Vector3 liftPoint = midPoint + (_bodyTransform.up * _liftHeight);//add the liftheight to the direction of the body's up to midpoint to get final liftpos
        return liftPoint;
    }
}
