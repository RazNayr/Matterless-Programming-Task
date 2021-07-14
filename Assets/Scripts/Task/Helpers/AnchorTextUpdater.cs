using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
 * Author: Ryan Camilleri
 * Script to update anchor text showing distance
 */
public class AnchorTextUpdater : IUpdateAnchorText
{   
    public void UpdateAnchorTextByPosition(Vector3 cameraPosition, object anchor)
    {
        var arAnchor = (ARAnchor)anchor;
        var textMeshPro = arAnchor.GetComponentInChildren<TextMeshProUGUI>();

        if(textMeshPro != null)
        {
            var distance = Vector3.Distance(cameraPosition, arAnchor.transform.position);
            textMeshPro.text = distance.ToString();
        }
            
    }

    public void UpdateAllAnchors(Vector3 cameraPosition, List<object> anchors)
    {
        foreach (ARAnchor arAnchor in anchors)
        {
            UpdateAnchorTextByPosition(cameraPosition, arAnchor);
        }
    }
}