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
 * This script is responsible for the above programming task
 */

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class LabelController : MonoBehaviour
{
    [SerializeField]
    Gradient colorGradient;

    [SerializeField]
    GameObject anchorPrefab;

    [SerializeField]
    int maxNumOfAnchors = 2;

    Camera arCamera;
    List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    List<ARAnchor> anchorPoints;

    // AR Managers
    ARRaycastManager raycastManager;
    ARAnchorManager anchorManager;
    ARPlaneManager planeManager;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;
        raycastManager = GetComponent<ARRaycastManager>();
        anchorManager = GetComponent<ARAnchorManager>();
        planeManager = GetComponent<ARPlaneManager>();
        anchorPoints = new List<ARAnchor>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForFingerInput();
        UpdateAnchorDetails();
    }

    void CheckForFingerInput()
    {
        // If user is touching the screen in the current frame --> enter
        if(Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            // Ensure anchoring only on initial finger tap event
            if (touch.phase != TouchPhase.Began)
                return;

            // Ensure touch is within detectable plane (for this task only horizontal planes are found)
            if(raycastManager.Raycast(touch.position, raycastHits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = raycastHits[0].pose;
                var hitTrackableId = raycastHits[0].trackableId;
                var hitPlane = planeManager.GetPlane(hitTrackableId);

                // Attach anchor to plane and parent label object to anchor
                var anchor = anchorManager.AttachAnchor(hitPlane, hitPose);
                var anchorObject = Instantiate(anchorPrefab, anchor.transform);
                anchorObject.transform.parent = anchor.transform;

                if (anchor != null)
                {
                    // Remove oldest anchor if max number of anchors is reached (in this case 2)
                    if(anchorPoints.Count >= maxNumOfAnchors)
                        RemoveOldestAnchor();

                    // Add most recent anchor
                    anchorPoints.Add(anchor);
                }
            }
        }
    }

    void RemoveOldestAnchor()
    {
        // Since Lists guarantee insertion ordering, remove elements at first index since these would be the oldest.
        // Recent anchors move up one index to balance out the removed element, thereby becoming older automatically.
        Destroy(anchorPoints[0].transform.gameObject);
        anchorPoints.RemoveAt(0);
    }

    void UpdateAnchorDetails()
    {
        if(anchorPoints.Count > 0)
        {
            Vector3 cameraPosition = arCamera.transform.position;

            foreach (var anchorPoint in anchorPoints)
            {
                // Update label distance text
                var distance = Vector3.Distance(cameraPosition, anchorPoint.transform.position);
                anchorPoint.GetComponentInChildren<TextMeshProUGUI>().text = distance.ToString();

                // Update label line renderer
                var lineRenderer = anchorPoint.GetComponentInChildren<LineRenderer>();
                lineRenderer.SetPosition(0, anchorPoint.transform.position);
                lineRenderer.SetPosition(1, new Vector3(cameraPosition.x, cameraPosition.y-0.06f, cameraPosition.z));

                // Distribute the distance range over 3 meters
                // If the distance is larger, the value is clamped to 1 which is the max value that the colour gradient could take
                Color lineColor = colorGradient.Evaluate(Mathf.Clamp(distance/3, 0, 1));
                lineRenderer.startColor = lineColor;
                lineRenderer.endColor = lineColor;
            }
        }
    }
}
