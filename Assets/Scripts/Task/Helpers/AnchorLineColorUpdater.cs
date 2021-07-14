using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
 * Author: Ryan Camilleri
 * Script to update anchor line renderer color based on distance
 */
public class AnchorLineColorUpdater : IUpdateAnchorLineColor
{
    Gradient _colorGradient;
    int _distanceRange;

    public AnchorLineColorUpdater(Gradient colorGradient, int distanceRange = 3)
    {
        _colorGradient = colorGradient;
        _distanceRange = distanceRange;
    }

    public void UpdateAnchorLineColorByPosition(Vector3 cameraPosition, object anchor)
    {
        var arAnchor = (ARAnchor)anchor;
        var lineRenderer = arAnchor.GetComponentInChildren<LineRenderer>();

        if (lineRenderer != null)
        {
            var distance = Vector3.Distance(cameraPosition, arAnchor.transform.position);

            // Distribute the distance range over 3 meters
            // If the distance is larger, the value is clamped to 1 which is the max value that the colour gradient could take
            Color distanceColor = _colorGradient.Evaluate(Mathf.Clamp(distance / _distanceRange, 0, 1));

            lineRenderer.startColor = distanceColor;
            lineRenderer.endColor = distanceColor;
        }
    }

    public void UpdateAllAnchors(Vector3 cameraPosition, List<object> anchors)
    {
        foreach (ARAnchor arAnchor in anchors)
        {
            UpdateAnchorLineColorByPosition(cameraPosition, arAnchor);
        }
    }
}
