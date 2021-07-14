using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdateAnchorText
{
    void UpdateAnchorTextByPosition(Vector3 cameraPosition, object anchor);
    void UpdateAllAnchors(Vector3 cameraPosition, List<object> anchors);
}
