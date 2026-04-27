using Xareus.Scenarios.Context;
using Xareus.Scenarios.Utilities;
using Xareus.Scenarios.Variables;
using Xareus.Scenarios.Unity;
using UnityEngine;
using System.Collections.Generic;

[FunctionDescription("A Unity Effector")]
public class MoveControllerEffector : AUnityEffector
{
    [ConfigurationParameter("Parameter", Necessity.Required)]
    string parameter;

    [ConfigurationParameter("ParameterInt", Necessity.Required)]
    int parameterInt;

    [ConfigurationParameter("gameObjectToObserve", Necessity.Required)]
    public GameObject gameObjectToObserve;

    [ContextVariable("result", "The result of the operation")]
    protected ContextVariable<float> result;

    // --- Movement tracking ---
    private Vector3 lastPosition;
    private float movementThreshold = 0.001f;
    private bool isMoving = false;

        public MoveControllerEffector(Xareus.Scenarios.Event @event,
            Dictionary<string, Xareus.Scenarios.Parameter> nameValueListMap,
            IContext externalContext,
            IContext scenarioContext,
            IContext sequenceContext,
            IContext eventContext)
            : base(@event, nameValueListMap, new ContextHolder(externalContext, scenarioContext, sequenceContext))

        { }
    public override void SafeReset()
    {


        if(gameObjectToObserve == null)
        {
            Debug.LogWarning("[WARNING] : Controller not found");
        }

        
        lastPosition = gameObjectToObserve.transform.position;
        Debug.Log($"[MoveController] Tracking object: {gameObjectToObserve}");
    
    }

    public override void SafeEffectorUpdate()
    {
        if (gameObjectToObserve == null)
            return;

        Vector3 currentPosition = gameObjectToObserve.transform.position;
        float distanceMoved = Vector3.Distance(currentPosition, lastPosition);

        isMoving = distanceMoved > movementThreshold;

        if (isMoving)
        {
            Debug.Log($"[MoveController] Object is moving! Distance: {distanceMoved}");
            result.Set(distanceMoved); 
        }
        else
        {
            Debug.Log("[MoveController] Object is NOT moving.");
            result.Set(0f);
        }

        // Update last position for next frame
        lastPosition = currentPosition;
    }
}
