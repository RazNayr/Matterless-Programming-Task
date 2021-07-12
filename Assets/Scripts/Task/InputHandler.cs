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
 * This script is responsible for checking whether the user started touching the screen and if so, attaches a new anchor to the observable plane
 */

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public static class InputHandler
{
    public static ARAnchor HandleAnchorInput(ARRaycastManager raycastManager, ARPlaneManager planeManager, ARAnchorManager anchorManager)
    {
        List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

        // If user is touching the screen in the current frame --> enter
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            // Ensure anchoring only on initial finger tap event
            if (touch.phase != TouchPhase.Began)
                return null;

            // Ensure touch is within detectable plane (for this task only horizontal planes are found)
            if (raycastManager.Raycast(touch.position, raycastHits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = raycastHits[0].pose;
                var hitTrackableId = raycastHits[0].trackableId;
                var hitPlane = planeManager.GetPlane(hitTrackableId);

                // Attach anchor to plane and add it to list of anchors
                var anchor = anchorManager.AttachAnchor(hitPlane, hitPose);
                LabelHandler.HandleNewAnchor(anchor);

                return anchor;
            }
        }
        return null;
    }
}
