using Xareus.Scenarios.Context;
using Xareus.Scenarios.Utilities;
using Xareus.Scenarios.Variables;
using Xareus.Scenarios.Unity;
using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

[FunctionDescription("A Unity Effector - Detects button release on XR Controller")]
public class UnclickEffector : AUnityEffector
{
    [ConfigurationParameter("ControllerHand", Necessity.Required)]
    private string controllerHand; // "Left" or "Right"

    [ConfigurationParameter("ButtonToSimulate", Necessity.Required)]
    private string buttonToSimulate; // "Trigger", "Grip", "Primary", "Secondary"

    [ContextVariable("result", "1 if button was just released, 0 otherwise")]
    protected ContextVariable<float> result;

    private InputDevice targetDevice;
    private bool wasPressed = false; // Tracks the previous frame's state

    public UnclickEffector(Xareus.Scenarios.Event @event,
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
            Debug.Log($"[UnclickController] Controller found: {targetDevice.name}");
        }
        else
        {
            Debug.LogWarning("[UnclickController] No controller found!");
        }

        wasPressed = false; // Reset state
    }

    public override void SafeEffectorUpdate()
    {
        if (!targetDevice.isValid)
        {
            Debug.LogWarning("[UnclickController] Controller is not valid.");
            return;
        }

        bool isReleased = DetectRelease(buttonToSimulate);

        if (isReleased)
        {
            Debug.Log($"[UnclickController] Button '{buttonToSimulate}' was released!");
            result.Set(1f);
        }
        else
        {
            result.Set(0f);
        }
    }

    private bool DetectRelease(string button)
    {
        InputFeatureUsage<bool> buttonUsage;

        switch (button)
        {
            case "Trigger":
                buttonUsage = CommonUsages.triggerButton;
                break;
            case "Grip":
                buttonUsage = CommonUsages.gripButton;
                break;
            case "Primary":
                buttonUsage = CommonUsages.primaryButton;
                break;
            case "Secondary":
                buttonUsage = CommonUsages.secondaryButton;
                break;
            default:
                Debug.LogWarning($"[UnclickController] Unknown button: {button}");
                return false;
        }

        bool isCurrentlyPressed = false;
        targetDevice.TryGetFeatureValue(buttonUsage, out isCurrentlyPressed);

        // Released = was pressed last frame AND not pressed this frame
        bool isReleased = wasPressed && !isCurrentlyPressed;

        // Update state for next frame
        wasPressed = isCurrentlyPressed;

        return isReleased;
    }
}