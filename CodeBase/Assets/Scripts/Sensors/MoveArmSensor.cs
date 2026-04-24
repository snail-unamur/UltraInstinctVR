using Xareus.Scenarios.Context;
using Xareus.Scenarios.Utilities;
using Xareus.Scenarios.Unity;
using UnityEngine;
using System.Collections.Generic;
using System;

[FunctionDescription("A Unity Sensor")]
public class MoveArmSensor : AInUnityStepSensor
{
    
    
    
    [ConfigurationParameter("Arms to follow", Necessity.Required)]
    protected GameObject targetObject;
    
    // A key that will be used in the EventContext
    [EventContextEntry()]
    public static readonly string KEY = "key";

    // A configuration parameter for the sensor that will be displayed
    // in the scenario inspector
    [ConfigurationParameter("Parameter", Necessity.Required)]
    protected string parameter;
    
    [ConfigurationParameter("TrackPosition", Necessity.Required)]
    protected bool TrackPosition;
    
    [ConfigurationParameter("TrackPosition", Necessity.Required)]
    protected bool SmoothPosition;


    [ConfigurationParameter("TrackPosition", Necessity.Required)]
    protected bool ThrowSmoothingDuration;

    // Also add all other parameters



    //Add context to the event
    private Vector3 lastPosition;

    // Threshold to consider as "moving" (avoids floating point noise)
    private float movementThreshold = 0.001f;



    // The event context that will be returned
    private SimpleDictionary eventContext = new SimpleDictionary();

    public MoveArmSensor(Xareus.Scenarios.Event @event,
        Dictionary<string, Xareus.Scenarios.Parameter> nameValueListMap,
        IContext externalContext, 
        IContext scenarioContext, 
        IContext sequenceContext)
        : base(@event, nameValueListMap, new ContextHolder(externalContext, scenarioContext, sequenceContext))
    { }

    public override void SafeReset()
    {
        // Find the GameObject by name (or assign it another way)
        targetObject = GameObject.Find("YourObjectName");
        
        if (targetObject != null)
            lastPosition = targetObject.transform.position;

    }

    public override Result UnityStepSensorCheck()
    {
        if (targetObject == null)
            return new Result(false, eventContext);

        Vector3 currentPosition = targetObject.transform.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

        bool isMoving = distanceMoved > movementThreshold;

        if (isMoving)
        {
            eventContext.Add(KEY, distanceMoved.ToString());
        }

        // Update last position for next frame
        lastPosition = currentPosition;

        return new Result(isMoving, eventContext);
        }
}