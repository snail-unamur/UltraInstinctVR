using System;
using System.Collections.Generic;
using UnityEngine;
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




    public void GripObject(GameObject obj)
    {
        // Logic to grip the object using the hand
    }


    public void ReleaseObject(GameObject obj)
    {
        // Logic to release the object from the hand
    }


}
