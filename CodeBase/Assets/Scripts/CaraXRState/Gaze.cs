using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.UI.BodyUI;

public class Gaze : Head
{

    //FollowPresetDatum.cs

    public FollowReferenceAxis palmReferenceAxis = FollowReferenceAxis.Down;
    public bool snapToGaze; 
    public float snapToGazeAngleThreshold;


    // EyeGazeInteraction.cs
    
    public static InputFeatureUsage<Vector3> gazePosition = new InputFeatureUsage<Vector3>("gazePosition");
    public static InputFeatureUsage<Quaternion> gazeRotation = new InputFeatureUsage<Quaternion>("gazeRotation");


}
