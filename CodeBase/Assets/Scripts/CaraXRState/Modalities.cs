using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Interactions.SectorInteraction;
using UnityEngine.XR.OpenXR.Features;
public class Modalities
{

    public float posX;
    public float posY;
    public float posZ;
    public float rotX;
    public float rotY;
    public float rotZ;

    public SerializableGuid savedAnchorGuid;

    public ARAnchor anchor;



    // GestureRecognizer.cs
    Func<InputSystem.EnhancedTouch.Touch, InputSystem.EnhancedTouch.Touch, T> s_CreateGestureFromTwoEnhancedTouchFunction;

    static Action<T, InputSystem.EnhancedTouch.Touch> s_ReinitializeGestureFromOneEnhancedTouchFunction;
    static Action<T, InputSystem.EnhancedTouch.Touch, InputSystem.EnhancedTouch.Touch> s_ReinitializeGestureFromTwoEnhancedTouchFunction;


    // FallbackComposite.cs


    //[InputControl(layout = "Vector3")
    public int first;
    //[InputControl(layout = "Vector3")]
    public int second;
    //[InputControl(layout = "Vector3")]
    public int third;
    
    //[InputControl(layout = "Quaternion")] 
    public int Quaternionfirst;
    //[InputControl(layout = "Quaternion")]  
    public int QuaternionSecond;
    //[InputControl(layout = "Quaternion")] 
    public int Quarternionthird;
    
    //[InputControl(layout = "Integer")] 
    public int IntegerFirst;
    //[InputControl(layout = "Integer")] 
    public int IntegerSecond;
    //[InputControl(layout = "Integer")] 
    public int IntegerThird;
    //[InputControl(layout = "Button")] 
    public int buttonFirst;
    //[InputControl(layout = "Button")] 
    public int ButtonSecond;
    //[InputControl(layout = "Button")]
    public int buttonThird;


    // SectorInteraction.cs
    public Directions directions;
    public SweepBehavior sweepBehavior;

    // CameraOffset.cs
    static List<XRInputSubsystem> s_InputSubsystems = new List<XRInputSubsystem>();


    //XRManagerSettings.cs

    [HideInInspector] [SerializeField] bool m_RequiresSettingsUpdate = false;

    // OpenXRFeature.cs

    public bool fixItAutomatic = true;

    // OpenXRInteractionFeature.cs
    public string interactionProfileName;
    public string name;
    private List<ActionBinding> bindings;

    public string desiredInteractionProfile;
    public string manufacturer;

    // DPadInteraction.cs
    public float forceThresholdLeft = 0.5f;
    public float forceThresholdReleaseLeft = 0.4f;
    public float centerRegionLeft = 0.5f;
    public bool isStickyLeft = false;
    public float forceThresholdRight = 0.5f;
    public float forceThresholdReleaseRight = 0.4f;
    public float centerRegionRight = 0.5f;
    public bool isStickyRight = false;

    // OpenXRInput.cs
    public ulong actionId;

    // XrSwapchainCreateInfo.cs
    public ulong UsageFlags;

    [Serializable]
    internal class ActionBinding
    {
        /// <summary>OpenXR interaction profile name</summary>
        public string interactionProfileName;

        /// <summary>OpenXR path for the interaction</summary>
        public string interactionPath;

        /// <summary>Optional OpenXR user paths <see cref="UserPaths"/></summary>
        public List<string> userPaths;
    }
}
