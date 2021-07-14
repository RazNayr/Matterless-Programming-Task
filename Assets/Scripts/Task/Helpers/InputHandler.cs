using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
 * Author: Ryan Camilleri
 * Script responsible for detect user touch and returning touch position
 */

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class InputHandler: IUserInput
{
    public Vector2? GetTouchPosition()
    {
        // If user is touching the screen in the current frame --> enter
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            // Ensure anchoring only on initial finger tap event
            if (touch.phase != TouchPhase.Began)
                return null;

            return touch.position;
        }
        return null;
    }
}
