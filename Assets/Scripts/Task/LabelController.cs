using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*
 * Matterless programming task for Unity hires

Make a simple app where you can place two labels in AR, each displaying the distance to the camera in real time.

Requirements
* The two labels should be anchored to the real world.
* The distance to each individual label to the camera should be displayed in realtime.
* Only ARFoundation can be used


BONUS POINTS:
*You can only use one monobehaviour.
* This one monobehaviour does no logic, it just delegates tasks to Unity so that Unity could be replaced by another engine easily.
* Render the distance as a line between camera and label, change color based on distance.
* Clean, minimalistic and functional code that's easy to understand for another developer.
*/

/*
 * Author: Ryan Camilleri
 * This script is responsible making use of the label and input handlers for executing the above task
 */

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class LabelController : MonoBehaviour
{
    // Delegate which returns ARAnchor after handling user finger input
    Func<ARRaycastManager, ARPlaneManager, ARAnchorManager, ARAnchor> addAnchor;

    // Delegate which updates all the required label details
    event Action<Vector3> updateLabelDetails;

    [SerializeField]
    Gradient colorGradient;

    [SerializeField]
    GameObject anchorPrefab;

    // AR Managers
    ARRaycastManager raycastManager;
    ARAnchorManager anchorManager;
    ARPlaneManager planeManager;

    private void OnEnable()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
        planeManager = GetComponent<ARPlaneManager>();

        LabelHandler.SetColorGradient(newColorGradient: colorGradient);

        // Subscribe functions to delegates
        addAnchor += InputHandler.HandleAnchorInput;
        updateLabelDetails += LabelHandler.UpdateLabelDistance;
        updateLabelDetails += LabelHandler.UpdateLabelLineRenderer;
    }

    // Update is called once per frame
    void Update()
    {
        var newAnchor = addAnchor?.Invoke(raycastManager, planeManager, anchorManager);

        // In practical situations, users will be given the decision to do what they want to the anchors.
        // In this case, the user is manipulating the anchor and adding a prefab to it to be able to visualise the anchor.
        if(newAnchor != null)
        {
            var anchorObject = Instantiate(anchorPrefab, newAnchor.transform);
            anchorObject.transform.parent = newAnchor.transform;
        }

        updateLabelDetails?.Invoke(Camera.main.transform.position);
    }

    private void OnDisable()
    {
        // Unsubscribe delegates to avoid memory leak
        addAnchor -= InputHandler.HandleAnchorInput;
        updateLabelDetails -= LabelHandler.UpdateLabelDistance;
        updateLabelDetails -= LabelHandler.UpdateLabelLineRenderer;
    }


}
