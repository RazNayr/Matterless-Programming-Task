using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
 * Author: Ryan Camilleri
 * Script to attach a game object to an anchor
 */
public class AnchorObjectAttacher : IAttachAnchorObject
{
    public void AttachObjectToAnchor(object anchor, Object objectToAttach)
    {
        var arAnchor = (ARAnchor)anchor;
        var anchorObject = GameObject.Instantiate(objectToAttach, arAnchor.transform) as GameObject;
        anchorObject.transform.parent = arAnchor.transform;
    }
}
