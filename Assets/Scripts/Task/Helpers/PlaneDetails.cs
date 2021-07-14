using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

/*
 * Author: Ryan Camilleri
 * Script acting as a model to store plane details of the touched plane
 */

public class PlaneDetails
{
    public Pose _hitPose;
    public ARPlane _hitPlane;

    public PlaneDetails(Pose hitPose, ARPlane hitPlane)
    {
        _hitPose = hitPose;
        _hitPlane = hitPlane;
    }
    
}
