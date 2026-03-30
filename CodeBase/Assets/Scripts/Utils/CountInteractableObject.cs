using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CountInteractablesObject : MonoBehaviour
{
    void Start()
    {
        CountAllInteractables();
    }

    public static int CountAllInteractables()
    {
        XRBaseInteractable[] interactables = Object.FindObjectsByType<XRBaseInteractable>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        int total = interactables.Length;

        Debug.Log($"Total Interactable objects in scene: {total}");


        return total;
    }
}