using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorAdder : IAttachPlaneAnchor
{
    ARAnchorManager _anchorManager;

    public AnchorAdder(ARAnchorManager anchorManager)
    {
        _anchorManager = anchorManager;
    }

    public object AttachAnchorToPlane(object plane, object position)
    {
        var anchor = _anchorManager.AttachAnchor((ARPlane)plane, (Pose)position);
        return anchor;
    }
}
