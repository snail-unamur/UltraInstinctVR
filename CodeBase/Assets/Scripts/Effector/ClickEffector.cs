using Xareus.Scenarios.Context;
using Xareus.Scenarios.Utilities;
using Xareus.Scenarios.Variables;
using Xareus.Scenarios.Unity;
using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

[FunctionDescription("A Unity Effector")]
public class ClickEffector : AUnityEffector
{
    [ConfigurationParameter("gameObjectToObserve", Necessity.Required)]

    protected GameObject gameObjectToObserve;

    [ConfigurationParameter("ControllerHand", Necessity.Required)]

    private string controllerHand; 

    [ConfigurationParameter("ButtonToSimulate", Necessity.Required)]
    private string buttonToSimulate; 

    [ContextVariable("result", "The result of the operation")]
    protected ContextVariable<float> result;

    private InputDevice targetDevice;

    public ClickEffector(Xareus.Scenarios.Event @event,
         Dictionary<string, Xareus.Scenarios.Parameter> nameValueListMap,
        IContext externalContext,
        IContext scenarioContext,
        IContext sequenceContext,
        IContext eventContext)
        : base(@event, nameValueListMap, externalContext, scenarioContext, sequenceContext, eventContext)
    { }

    public override void SafeReset()
    {
        

        // Find the correct controller (Left or Right)
        InputDeviceCharacteristics characteristics = controllerHand == "Left"
            ? InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller
            : InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            Debug.Log($"[MoveController] Controller found: {targetDevice.name}");
        }
        else
        {
            Debug.LogWarning("[MoveController] No controller found!");
        }
    }

    public override void SafeEffectorUpdate()
    {
        if (!targetDevice.isValid)
        {
            Debug.LogWarning("[MoveController] Controller is not valid.");
            return;
        }

        SimulateClick(buttonToSimulate);
    }

    private void SimulateClick(string button)
    {
        switch (button)
        {
            case "Trigger":
                SimulateButton(CommonUsages.triggerButton, CommonUsages.trigger);
                break;

            case "Grip":
                SimulateButton(CommonUsages.gripButton, CommonUsages.grip);
                break;

            case "Primary": // A or X button
                SimulateButton(CommonUsages.primaryButton, null);
                break;

            case "Secondary": // B or Y button
                SimulateButton(CommonUsages.secondaryButton, null);
                break;

            default:
                Debug.LogWarning($"[MoveController] Unknown button: {button}");
                break;
        }
    }

    private void SimulateButton(InputFeatureUsage<bool> buttonUsage, 
                                 InputFeatureUsage<float>? axisUsage)
    {
        bool isPressed = false;
        if (targetDevice.TryGetFeatureValue(buttonUsage, out isPressed))
        {
            Debug.Log($"[MoveController] {buttonUsage.name} is pressed: {isPressed}");
            result.Set(isPressed ? 1f : 0f);
        }

        // Also check axis value (e.g. how hard the trigger is pressed)
        if (axisUsage.HasValue)
        {
            float axisValue = 0f;
            if (targetDevice.TryGetFeatureValue(axisUsage.Value, out axisValue))
            {
                Debug.Log($"[MoveController] {axisUsage.Value.name} axis value: {axisValue}");
                result.Set(axisValue);
            }
        }
    }
}