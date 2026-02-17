using log4net.Util;
using System;
using System.Collections.Generic;
using TMPro;
using System.Reflection;

using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.OpenXR.Features;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Interactions.SectorInteraction;
public class Modalities
{

    // parameters linkde to movement and position of the object
    public Vector3 position;
    public Quaternion rotation;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 180f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private bool isMoving = false;
    private bool isRotating = false;

    public GameObject gameObject;



    public SerializableGuid savedAnchorGuid;

    public ARAnchor anchor;



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

    [HideInInspector][SerializeField] bool m_RequiresSettingsUpdate = false;

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




    // Constructor
    public Modalities()
    {
        this.position = this.gameObject.transform.position;
        this.rotation = this.gameObject.transform.rotation;

    }




    public void modifyParameters(System.Object parameters, string parameterName, object newValue)
    {
        if (parameters == null) return;

        Type type = parameters.GetType();

        // Cherche un FIELD (variable)
        FieldInfo field = type.GetField(parameterName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (field != null)
        {
            field.SetValue(parameters, newValue);
            return;
        }

        // Cherche une PROPERTY
        PropertyInfo property = type.GetProperty(parameterName,
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (property != null && property.CanWrite)
        {
            property.SetValue(parameters, newValue);
        }
    }


    public void moveObject(GameObject obj, Vector3 firstrange, Vector3 lastrange)
    {
        // Move object logic here
    }

    public void moveObjectExactly(Vector3 newPosition)
    {
        targetPosition = newPosition;
        isMoving = true;
    }

    public void rotateObject(GameObject obj, Quaternion newRotation)
    {
        this.gameObject = obj;
        targetRotation = newRotation;
        isRotating = true;
    }

    private void Update()
    {
        // --- MOVE ---
        if (isMoving)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(
            this.gameObject.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (this.gameObject.transform.position == targetPosition)
                isMoving = false;
        }

        // --- ROTATE ---
        if (isRotating && this.gameObject != null)
        {
            this.gameObject.transform.rotation = Quaternion.RotateTowards(
                this.gameObject.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(this.gameObject.transform.rotation, targetRotation) < 0.1f)
            {
                this.gameObject.transform.rotation = targetRotation;
                isRotating = false;
            }
        }
    }
}
