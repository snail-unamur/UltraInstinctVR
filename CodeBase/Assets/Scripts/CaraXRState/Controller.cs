using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Controller : Modalities
{


    // XRSimulatedControllerState.cs
    //[InputControl(usage = "Grip", layout = "Axis", offset = 12)]\n[FieldOffset(12)]
    public float grip;


    //XRControllerRecording.cs
    public bool m_SelectActivatedInFirstFrame;
    public bool m_ActivateActivatedInFirstFrame;
    public bool m_FirstUIPressActivatedInFirstFrame;


    // XRControllerState.cs
    public double time;
    public InputTrackingState inputTrackingState;
    public bool isTracked;
    public Vector3 position;
    public Quaternion rotation;
    public InteractionState selectInteractionState;
    public InteractionState activateInteractionState;
    public InteractionState uiPressInteractionState;






}
