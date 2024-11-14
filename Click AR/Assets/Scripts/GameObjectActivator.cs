/*
 * GameObjectActivator.cs
 *
 * Project: Click AR
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 * 
 * Description:
 * This script activates a target GameObject at a specified time and deactivates it after a certain duration.
 * It uses `Time.time` to determine the right moment for activation and deactivation.
 * 
 * The script has three main parameters:
 *  - targetObject: The GameObject to activate and deactivate.
 *  - activationTime: The time when the object should be activated.
 *  - duration: The time the object should stay active before being deactivated.
 */

using UnityEngine;

public class GameObjectActivator : MonoBehaviour
{
    #region Public Variables

    // The GameObject that will be activated and deactivated
    public GameObject targetObject;

    // The time (in seconds) when the object should be activated
    public float activationTime;

    // The duration (in seconds) for which the object should stay active
    public float duration;

    #endregion

    #region Private Variables

    // Flag to check if the object has been activated
    private bool activated = false;

    // Time when the object should stop being active
    private float activationEndTime;

    #endregion

    #region Unity Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the time when the activation period should end
        activationEndTime = activationTime + duration;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current time in seconds
        float currentTime = Time.time;

        // Check if it's time to activate the object
        if (!activated && currentTime >= activationTime && currentTime < activationEndTime)
        {
            ActivateObject();
            activated = true; // Mark as activated
        }
        else if (activated && currentTime >= activationEndTime)
        {
            DeactivateObject();
        }
    }

    #endregion

    #region Private Methods

    // Activate the target object
    void ActivateObject()
    {
        if (targetObject != null && !targetObject.activeSelf)
        {
            targetObject.SetActive(true);
        }
    }

    // Deactivate the target object
    void DeactivateObject()
    {
        if (targetObject != null && targetObject.activeSelf)
        {
            targetObject.SetActive(false);
        }
    }

    #endregion
}
