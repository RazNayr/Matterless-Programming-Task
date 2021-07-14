using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
 * Author: Ryan Camilleri
 * Script to update anchor line renderer based on distance
 */
public class AnchorLineUpdater : IUpdateAnchorLine
{
    public void UpdateAnchorLineByPosition(Vector3 cameraPosition, object anchor)
    {
        var arAnchor = (ARAnchor)anchor;
        var lineRenderer = arAnchor.GetComponentInChildren<LineRenderer>();

        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, arAnchor.transform.position);
            lineRenderer.SetPosition(1, new Vector3(cameraPosition.x, cameraPosition.y - 0.06f, cameraPosition.z));
        }
    }

    public void UpdateAllAnchors(Vector3 cameraPosition, List<object> anchors)
    {
        foreach(ARAnchor arAnchor in anchors)
        {
            UpdateAnchorLineByPosition(cameraPosition, arAnchor);
        }
    }
}