using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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
 * This script is responsible for the modification and updating of label details.
 * This also handles new anchors created using the InputHandler
 */

public static class LabelHandler
{
    private static List<ARAnchor> anchorPoints = new List<ARAnchor>();
    private static int maxNumOfAnchors = 2;
    private static Gradient colorGradient = null;
    private static bool useColorGradient = false;
    private static Color defaultColor = Color.white;

    public static void SetColorGradient(Gradient newColorGradient, bool useGradient = true)
    {
        colorGradient = newColorGradient;
        useColorGradient = useGradient;
    }

    public static void SetMaxNumOfAnchors(int newMax)
    {
        maxNumOfAnchors = newMax;
    }

    public static void HandleNewAnchor(ARAnchor newAnchor)
    {
        if (newAnchor != null)
        {
            // Remove oldest anchor if max number of anchors is reached (in this case 2)
            if (anchorPoints.Count >= maxNumOfAnchors)
            {
                // Since Lists guarantee insertion ordering, remove elements at first index since these would be the oldest.
                // Recent anchors move up one index to balance out the removed element, thereby becoming older automatically.
                Object.Destroy(anchorPoints[0].transform.gameObject);
                anchorPoints.RemoveAt(0);
            }

            anchorPoints.Add(newAnchor);
        }
    }

    public static void UpdateLabelDistance (Vector3 cameraPosition)
    {
        if (anchorPoints.Count > 0)
        {
            foreach (var anchorPoint in anchorPoints)
            {
                // Update label distance text
                var distance = Vector3.Distance(cameraPosition, anchorPoint.transform.position);
                anchorPoint.GetComponentInChildren<TextMeshProUGUI>().text = distance.ToString();
            }
        }
    }

    public static void UpdateLabelLineRenderer(Vector3 cameraPosition)
    {
        if (anchorPoints.Count > 0)
        {
            foreach (var anchorPoint in anchorPoints)
            {
                // Update label line renderer
                var lineRenderer = anchorPoint.GetComponentInChildren<LineRenderer>();

                if(lineRenderer != null)
                {
                    lineRenderer.SetPosition(0, anchorPoint.transform.position);
                    lineRenderer.SetPosition(1, new Vector3(cameraPosition.x, cameraPosition.y - 0.06f, cameraPosition.z));

                    if (colorGradient != null && useColorGradient)
                    {
                        // Distribute the distance range over 3 meters
                        // If the distance is larger, the value is clamped to 1 which is the max value that the colour gradient could take
                        var distance = Vector3.Distance(cameraPosition, anchorPoint.transform.position);
                        Color distanceColor = colorGradient.Evaluate(Mathf.Clamp(distance / 3, 0, 1));
                        lineRenderer.startColor = distanceColor;
                        lineRenderer.endColor = distanceColor;
                    }
                    else
                    {
                        lineRenderer.startColor = defaultColor;
                        lineRenderer.endColor = defaultColor;
                    }
                }
                
            }
        }
    }
}
