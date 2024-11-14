/*
 * ImageTargetDistanceScaleManager.cs
 *
 * Project: 4ME306 - Cross Media Design and Production; AR workshop 2
 *
 * Supported Unity version: 2021.2.7f1 Personal (tested)
 *
 * Author: Nico Reski
 * Modifications by: Sheba Ponmelil Babu
 * GitHub: https://github.com/nicoversity
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class responsible for the interaction between multiple Augmented Reality (AR) Markers (implemented as ImageTarget component using Vuforia Engine AR).
/// </summary>
public class minus : MonoBehaviour
{
    #region PROPERTIES

    // references to image target (= AR marker) game objects (set in the Unity Inspector)
    // (specifically: references to ImageTargetTracker script components of these game objects)
    [Header("ImageTargetTracker References")]
    public ImageTargetTracker imageTargetContent;
    public ImageTargetTracker imageTargetMinus;

    [Header("Virtual Object of Content AR Marker")]
    public Transform contentVirtualObject;              // reference to content virtual object of the CONTENT AR marker (set in the Unity Inspector)
    private Vector3 contentVirtualObjectOriginalScale;  // helper value to keep track of the CONTENT AR marker's virtual object original scale
    private static readonly float minScale = -2.0f;      // helper value indicating the maximum scaling of our CONTENT AR marker's virtual object

    [Header("Debug")]
    public bool showDebugMessages = true;   // indicator whether (true) or not (false) to display custom debug messages in the Unity Console (default = true; can also be set in the Unity Inspector)

    #endregion


    #region UNITY_EVENT_FUNCTIONS

    /// <summary>
    /// Instantiation and reference setup.
    /// </summary>
    private void Awake()
    {
        // check if references are set up (via Unity Inspector) correctly (and print error message to the Unity Console if not)
        if (!imageTargetContent) Debug.LogError("[ImageTargetDistanceManager] Reference not set: Image Target Content");
        if (!imageTargetMinus) Debug.LogError("[ImageTargetDistanceManager] Reference not set: Image Target Minus");
        if (!contentVirtualObject) Debug.LogError("[ImageTargetDistanceManager] Reference not set: Content Virtual Object");

        // keep track of (= "remember") the original scale of the CONTENT AR marker's virtual object at the start of the application / scene
        contentVirtualObjectOriginalScale = contentVirtualObject.transform.localScale;
    }

    /// <summary>
    /// General update routine (called once per frame).
    /// </summary>
    private void Update()
    {
        // check if the CONTENT AR marker is currently tracked
        if (imageTargetContent.isTracked)
        {
            // while CONTENT AR marker is tracked, check also if PLUS AR marker is currently tracked
            if (imageTargetMinus.isTracked)
            {
                // CONTENT and PLUS AR markers are tracked at the same time: calculate distance and scale up the CONTENT AR marker's virtual object accordingly
                float distance = distanceContentToMinus();
                if (showDebugMessages) Debug.Log("[ImageTargetDistanceManager] Distance: Content <-> Minus = " + distance);
                scaleDownContent(distance);
            }
        }
    }

    #endregion


    #region DISTANCE

    /// <summary>
    /// Function to calculate the distance between the CONTENT and PLUS AR markers.
    /// </summary>
    /// <returns>Float value representing the distance between the two markers. Note: Float value returns 0.0f if one or both AR markers are not detected.</returns>
    private float distanceContentToMinus()
    {
        float distance = 0.0f;
        if (imageTargetContent.isTracked && imageTargetMinus.isTracked)
        {
            Vector3 contentPosition = imageTargetContent.transform.position;
            Vector3 MinusPosition = imageTargetMinus.transform.position;
            distance = Vector3.Distance(contentPosition, MinusPosition);
        }
        return distance;
    }

    #endregion


    #region SCALING

    /// <summary>
    /// Helper function to calculate a scale factor based on a provided distance value and applying simple quantization.
    /// </summary>
    /// <param name="distance">Float value representing the distance for the scale calculation. A smaller distance value results in a larger scale value, and vice verse.</param>
    /// <returns>Float value representing the calculated scale factor.</returns>
    private float calculateScaleMultiplier(float distance)
    {
        float scaleMultiplier = -0.0001f;
        if (distance >= 3.5f) scaleMultiplier = -0.0001f;
        else if (distance >= 3.0f && distance < 3.5) scaleMultiplier = -0.0005f;
        else if (distance >= 2.5f && distance < 3.0) scaleMultiplier = -0.001f;
        else if (distance >= 2.0f && distance < 2.5) scaleMultiplier = -0.005f;
        else if (distance >= 1.5f && distance < 2.0) scaleMultiplier = -0.01f;
        else if (distance >= 1.0f && distance < 1.5) scaleMultiplier = -0.02f;
        else if (distance < 1.0f) scaleMultiplier = -0.03f;

        return scaleMultiplier;


    }

    /// <summary>
    /// Function to scale up (= increase the size / scaling) of the virtual object attached to the CONTENT AR marker based on a provided distance value. 
    /// </summary>
    /// <param name="distance">Float value representing the distance between the CONTENT and PLUS AR markers.</param>
    private void scaleDownContent(float distance)
    {
        // get scale multiplier based on distance
        float scaleMultiplier = calculateScaleMultiplier(distance);

        // check for upper scaling maximum (to restrict the virtual object from growing any further)
        if ((contentVirtualObject.transform.localScale.x > contentVirtualObjectOriginalScale.x / minScale) &&
           (contentVirtualObject.transform.localScale.y > contentVirtualObjectOriginalScale.y / minScale) &&
           (contentVirtualObject.transform.localScale.z > contentVirtualObjectOriginalScale.z / minScale))
        {
            // if max scaling is not yet reached: apply scaling
            contentVirtualObject.transform.localScale += new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
        }

        // reposition / align virtual object to AR marker
        repositionVirtualObject();
    }

    /// <summary>
    /// Helper function to reposition the virtual object on top of the CONTENT AR marker after change of scale.
    /// </summary>
    private void repositionVirtualObject()
    {
        // reposition virtual object to "float" (align) slightly over the the physical CONTENT AR marker
        contentVirtualObject.transform.localPosition = new Vector3(contentVirtualObject.transform.localPosition.x,
                                                                    contentVirtualObject.transform.localScale.y * 0.5f + 0.25f,
                                                                    contentVirtualObject.transform.localPosition.z);
    }

    #endregion
}
