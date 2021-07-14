using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
 * Author: Ryan Camilleri
 * Script to limit the number of anchors in a single scene and remove the oldest anchors when limit is reached
 */
public class AnchorHandler: ILimitAnchors
{
    int _maxNumOfAnchors;
    List<object> _anchorPoints;

    public AnchorHandler(int maxNumOfAnchors)
    {
        _maxNumOfAnchors = maxNumOfAnchors;
        _anchorPoints = new List<object>();
    }

    public void HandleNewAnchor(object newAnchor)
    {
        // Remove oldest anchor if max number of anchors is reached (in this case 2)
        if (_anchorPoints.Count >= _maxNumOfAnchors)
            RemoveOldestAnchor();

        _anchorPoints.Add((ARAnchor)newAnchor);
    }

    public void RemoveOldestAnchor()
    {
        var arAnchor = (ARAnchor)_anchorPoints[0];
        Object.Destroy(arAnchor.transform.gameObject);
        _anchorPoints.RemoveAt(0);
    }

    public List<object> GetCurrentAnchors()
    {
        return _anchorPoints;
    }
}
