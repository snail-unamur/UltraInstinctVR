using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.XR.Hands;
using static UnityEngine.XR.Hands.XRHandSubsystem;

public class Hand : Modalities
{

    public XRHand hand;
    public Action<XRHandSubsystem, UpdateSuccessFlags, UpdateType> updatedHands;
    public Action<XRHand> trackingAcquired;
    public Action<XRHand> trackingLost;
    public static List<XRHandSubsystem> s_HandSubsystems;


    //FollowPresetDatum.cs
    public Vector3 rightHandLocalPosition;
    public Vector3 leftHandLocalPosition;
    public Vector3 rightHandLocalRotation;

    public Vector3 leftHandLocalRotation;

    public bool invertAxisForRightHand;




    public virtual void pokeObject(GameObject obj)
    {
        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.N));

        InputSystem.Update();


    }


    public virtual void GrabObject(GameObject obj)
    {
        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.K));

        InputSystem.Update();

    }


    public virtual void UngrabObject(GameObject obj)
    {
        InputSystem.QueueStateEvent(
            Keyboard.current,
            new KeyboardState()
        );

        InputSystem.Update();

    }


    public virtual void pinchObject(GameObject obj)
    {
        InputSystem.QueueStateEvent(Keyboard.current,new KeyboardState(Key.M));
        InputSystem.Update();


    }
    public virtual void unpinchObject(GameObject obj)
     {
        InputSystem.QueueStateEvent(Keyboard.current,new KeyboardState());
        InputSystem.Update();

    }

    public virtual void OpenHand(GameObject obj)
    {

        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.O));
        InputSystem.Update();


    }

    public virtual void fistHand(GameObject obj)
    {
        InputSystem.QueueStateEvent(Keyboard.current, new KeyboardState(Key.P));
        InputSystem.Update();

    }

}
