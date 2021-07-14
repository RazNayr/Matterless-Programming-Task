using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/*
 * Author: Ryan Camilleri
 * Script to return the plane detected when user touches screen
 */
public class PlaneDetector : IDetectPlane
{
    ARRaycastManager _raycastManager;
    ARPlaneManager _planeManager;

    public PlaneDetector(ARRaycastManager raycastManager, ARPlaneManager planeManager)
    {
        _raycastManager = raycastManager;
        _planeManager = planeManager;
    }

    public object GetDetectablePlane(Vector2 touchPosition)
    {
        List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

        // Ensure touch is within detectable plane (for this task only horizontal planes are found)
        if (_raycastManager.Raycast(touchPosition, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = raycastHits[0].pose;
            var hitTrackableId = raycastHits[0].trackableId;
            var hitPlane = _planeManager.GetPlane(hitTrackableId);

            return new PlaneDetails(hitPose, hitPlane);
        }

        return null;
    }

}
