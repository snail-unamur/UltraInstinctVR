using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Attachment;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Provider;
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


    public virtual void ClickButtonA(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {

        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.Digit2));

        InputSystem.Update();


    }

    public virtual void ClickButtonB(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {


        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.Digit2));

        InputSystem.Update();

    }

    public virtual void ClickTrigger(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {
        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.T));

        InputSystem.Update();
    }
    
    public virtual void ClickGrip(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {
        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.G));

        InputSystem.Update();
    }

    public virtual void ClickButtonX(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {

        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.Digit7));

        InputSystem.Update();



    }

    public virtual void ClickButtonY(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {


        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.Digit8));

        InputSystem.Update();

    }

    public virtual void ClickButtonMenu(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {
        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.M));

        InputSystem.Update();
    }

    public virtual void ClickButtonOption(GameObject obj, XRGrabInteractable ObjectToInteract = null)
    {
        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.Digit7));

        InputSystem.Update();

    }
}
