using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class MainController : MonoBehaviour
{
    // Delegate which updates all the required label details
    event Action<Vector3, List<object>> _updateAnchorDetails;

    [SerializeField]
    Gradient _colorGradient;

    [SerializeField]
    GameObject _anchorPrefab;

    // AR Managers
    ARRaycastManager _raycastManager;
    ARAnchorManager _anchorManager;
    ARPlaneManager _planeManager;

    // Helpers
    InputHandler _inputHandler;
    PlaneDetector _planeDetector;
    AnchorAdder _anchorAdder;
    AnchorObjectAttacher _anchorObjectAttacher;
    AnchorHandler _anchorHandler;
    AnchorTextUpdater _anchorTextUpdater;
    AnchorLineUpdater _anchorLineUpdater;
    AnchorLineColorUpdater _anchorLineColorUpdater;


    private void OnEnable()
    {
        // Get AR Managers
        _raycastManager = GetComponent<ARRaycastManager>();
        _anchorManager = GetComponent<ARAnchorManager>();
        _planeManager = GetComponent<ARPlaneManager>();

        // Initialise all helper objects
        _inputHandler = new InputHandler();
        _planeDetector = new PlaneDetector(_raycastManager, _planeManager);
        _anchorAdder = new AnchorAdder(_anchorManager);
        _anchorObjectAttacher = new AnchorObjectAttacher();
        _anchorHandler = new AnchorHandler(maxNumOfAnchors: 2);
        _anchorTextUpdater = new AnchorTextUpdater();
        _anchorLineUpdater = new AnchorLineUpdater();
        _anchorLineColorUpdater = new AnchorLineColorUpdater(_colorGradient);

        // Subscribe functions to updater delegate
        _updateAnchorDetails += _anchorTextUpdater.UpdateAllAnchors;
        _updateAnchorDetails += _anchorLineUpdater.UpdateAllAnchors;
        _updateAnchorDetails += _anchorLineColorUpdater.UpdateAllAnchors;
    }

    // Update is called once per frame
    void Update()
    {
        // Get Touch position
        Vector2? touchPosition = _inputHandler.GetTouchPosition();

        if (touchPosition != null)
        {
            // Get plane details that was touched
            var planeDetails = (PlaneDetails)_planeDetector.GetDetectablePlane((Vector2)touchPosition);

            if(planeDetails != null)
            {
                // Attach new anchor to touched plane
                var newAnchor = (ARAnchor)_anchorAdder.AttachAnchorToPlane(planeDetails._hitPlane, planeDetails._hitPose);

                if(newAnchor != null)
                {
                    // Attach label object to anchor
                    _anchorObjectAttacher.AttachObjectToAnchor(newAnchor, _anchorPrefab);

                    // Limit the number of anchors in one scene
                    _anchorHandler.HandleNewAnchor(newAnchor);
                }
            }
        }

        // Update anchor details
        _updateAnchorDetails?.Invoke(Camera.main.transform.position, _anchorHandler.GetCurrentAnchors());
    }

    private void OnDisable()
    {
        // Unsubscribe delegates to avoid memory leak
        _updateAnchorDetails -= _anchorTextUpdater.UpdateAllAnchors;
        _updateAnchorDetails -= _anchorLineUpdater.UpdateAllAnchors;
        _updateAnchorDetails -= _anchorLineColorUpdater.UpdateAllAnchors;
    }
}
