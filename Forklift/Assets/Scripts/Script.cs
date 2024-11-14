/*
 * Script.cs
 *
 * Project: Forklift
 *
 * Supported Unity version: 2021.2.7f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 * 
 * Description:
 * This script handles input detection (both touch and mouse clicks) to detect when a user clicks on specific objects in the scene. 
 * Based on the object clicked, it triggers animations and plays associated audio clips if available.
 *
 * Key Features:
 * - Detects touch and mouse input to cast a ray and check for collisions.
 * - Checks for specific objects (using the object names like "Activity01", "Activity02", etc.).
 * - Plays corresponding animations and audio if the correct object is hit.
 */

using System;
using UnityEngine;

public class Script : MonoBehaviour
{
    #region Fields

    // Animator for controlling animations.
    Animator animator;

    // Reference to the GameObject that will be used in the raycast logic.
    GameObject obj;

    // Array to store state numbers for identifying objects.
    string[] stateNum = { "01", "02", "03", "04" };

    // Array to store colliders in the scene (not currently used).
    Collider[] colliders;

    #endregion

    #region Unity Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Find and assign the Animator component from the scene.
        animator = FindObjectOfType<Animator>();

        // Find all colliders in the scene (although colliders array is not used in this script).
        colliders = FindObjectsOfType<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize hitInfo to store raycast hit details.
        RaycastHit hitInfo = new RaycastHit();
        bool hit = false;

        // Check for touch input (for mobile or tablet).
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hitInfo);
        }
        // Check for mouse click input (for desktop).
        else if (Input.GetMouseButtonDown(0))
        {
            hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
        }

        // If a valid hit is detected, proceed with checking the hit object.
        if (hit)
        {
            // Loop through the state numbers (activity names).
            for (int i = 0; i < stateNum.Length; i++)
            {
                // Check if the hit object matches any of the activity states.
                if (hitInfo.collider != null && hitInfo.collider.name.Equals("Activity" + stateNum[i]))
                {
                    // Play the corresponding animation.
                    animator.Play("Activity" + stateNum[i]);

                    // Find the GameObject associated with the hit activity.
                    obj = GameObject.Find("ObjectRoot/Labels/Activity" + stateNum[i]);

                    // If the object is found and contains an AudioSource, play the audio.
                    if (obj != null && obj.GetComponent<AudioSource>() != null)
                    {
                        obj.GetComponent<AudioSource>().Play();
                    }
                }
            }
        }
    }

    #endregion
}
