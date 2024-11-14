/*
 * ARInteraction.cs
 *
 * Project: Focus AR 
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import UI functionality for Text and UI elements

/// <summary>
/// This class handles the AR interaction system, allowing the user to interact with objects
/// in augmented reality by focusing on them and triggering interactions (like displaying UI elements
/// or playing audio) based on user input and object focus time.
/// </summary>
public class ARInteraction : MonoBehaviour
{
    #region PROPERTIES

    [Header("UI Elements")]
    public LayerMask interactableLayer;       // Defines which layers can be interacted with
    public Text instructionText;              // UI Text component to display information
    public GameObject panel;                  // UI panel that shows object information
    public GameObject image;                  // UI image displayed upon interaction
    public GameObject reticle;                // Visual reticle indicating the center of view

    [Header("Interaction Settings")]
    public float maxDistance = 5f;            // Maximum distance for detecting interactable objects
    public float cooldownDuration = 1f;       // Minimum time between interactions
    public float focusDurationThreshold = 3f; // Time needed to focus on an object to trigger interaction
    public float displayDuration = 10f;       // Duration for which interaction display remains visible
    public float reticleDistance = 2f;        // Distance at which reticle is positioned in front of the camera

    // Private state variables
    private float lastInteractionTime;        // Tracks the time of the last interaction
    private GameObject lastFocusedObject;     // The object currently being focused on
    private float focusStartTime;             // Tracks when focus on an object started
    private bool isDisplaying;                // Whether interaction display is currently active
    private float lastDisplayStartTime;       // Time when current display started
    private bool interactionTriggered;        // Indicates if interaction has been triggered
    private Dictionary<string, string> colliderTextMap; // Maps object names to descriptions
    private AudioSource currentlyPlayingAudio; // Currently playing audio source

    #endregion

    #region UNITY_EVENT_FUNCTIONS

    /// <summary>
    /// Initializes UI elements and sets up initial state.
    /// </summary>
    void Start()
    {
        InitializeUI();
        InitializeColliderTextMap();
        lastInteractionTime = -cooldownDuration;  // Allows immediate interaction at start
    }

    /// <summary>
    /// Updates the position of the reticle and checks for user interactions.
    /// </summary>
    void Update()
    {
        UpdateReticlePosition();

        if (Time.time - lastInteractionTime < cooldownDuration)
            return;

        if (isDisplaying && Time.time - lastDisplayStartTime >= displayDuration)
            ResetFocus();

        CheckForInteraction();
    }

    #endregion

    #region INTERACTION_FUNCTIONS

    /// <summary>
    /// Initializes the UI elements to their default state.
    /// </summary>
    void InitializeUI()
    {
        panel.SetActive(false);
        image.SetActive(false);
        reticle.SetActive(true);
    }

    /// <summary>
    /// Continuously updates the position of the reticle in front of the camera.
    /// </summary>
    void UpdateReticlePosition()
    {
        reticle.transform.position = Camera.main.transform.position + Camera.main.transform.forward * reticleDistance;
    }

    /// <summary>
    /// Checks if the user is interacting with any object within the defined range.
    /// </summary>
    void CheckForInteraction()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));  // Casts ray from center of screen
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != lastFocusedObject)
            {
                ResetFocus();           // Clear previous interaction display
                SetNewFocus(hitObject); // Set focus to the new object
            }

            if (!interactionTriggered && Time.time - focusStartTime >= focusDurationThreshold)
            {
                TriggerFocusInteraction(hitObject); // Trigger the interaction for the focused object
                lastInteractionTime = Time.time;    // Update last interaction time for cooldown
                StartDisplayTimer();                // Begin display timer
            }
        }
        else if (isDisplaying)
        {
            ResetFocus(); // Reset if no object is focused
        }
    }

    /// <summary>
    /// Sets the new object focus and prepares interaction.
    /// </summary>
    void SetNewFocus(GameObject hitObject)
    {
        StopCurrentAudio();             // Stop any currently playing audio
        lastFocusedObject = hitObject;  // Update to the newly focused object
        focusStartTime = Time.time;     // Track the time focus started
        interactionTriggered = false;   // Reset the interaction trigger
    }

    /// <summary>
    /// Stops any currently playing audio.
    /// </summary>
    void StopCurrentAudio()
    {
        if (currentlyPlayingAudio != null)
        {
            currentlyPlayingAudio.Stop();
            currentlyPlayingAudio = null;
        }
    }

    /// <summary>
    /// Triggers interaction for the focused object by playing audio and displaying instructions.
    /// </summary>
    void TriggerFocusInteraction(GameObject focusedObject)
    {
        AudioSource audioSource = focusedObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();                    // Play the audio fully
            currentlyPlayingAudio = audioSource;   // Reference to the playing audio
        }

        instructionText.text = $"Focused on: {focusedObject.name}\n{GetObjectDescription(focusedObject.name)}";
        EnableUI();  // Enable display of UI elements
    }

    /// <summary>
    /// Starts the display timer to show interaction UI elements.
    /// </summary>
    void StartDisplayTimer()
    {
        isDisplaying = true;             // Mark display as active
        lastDisplayStartTime = Time.time; // Track when display started
        interactionTriggered = true;     // Mark that interaction has been triggered
    }

    /// <summary>
    /// Resets the focus and clears all displayed information.
    /// </summary>
    void ResetFocus()
    {
        lastFocusedObject = null;        // Clear the last focused object
        interactionTriggered = false;    // Reset the interaction trigger
        isDisplaying = false;            // Mark display as inactive
        instructionText.text = "";       // Clear displayed text
        StopCurrentAudio();              // Stop audio when focus is lost
        DisableUI();                     // Hide UI elements
    }

    /// <summary>
    /// Enables the UI elements when an interaction is triggered.
    /// </summary>
    void EnableUI()
    {
        panel.SetActive(true);
        image.SetActive(true);
    }

    /// <summary>
    /// Disables the UI elements when interaction ends.
    /// </summary>
    void DisableUI()
    {
        panel.SetActive(false);
        image.SetActive(false);
    }

    /// <summary>
    /// Retrieves a description for the focused object based on its name.
    /// </summary>
    string GetObjectDescription(string objectName)
    {
        return colliderTextMap.TryGetValue(objectName, out string description) ? description : "No description available.";
    }

    /// <summary>
    /// Initializes the dictionary mapping object names to descriptions.
    /// </summary>
    void InitializeColliderTextMap()
    {
        colliderTextMap = new Dictionary<string, string>
        {
            { "Inner_Nozzle", "The inner nozzle guides and shapes the flow of exhaust gases exiting the combustion chamber in a jet engine." },
            { "Outer_Nozzle", "The outer nozzle surrounds the inner components of the jet engine and helps to further direct and accelerate the exhaust gases for efficient propulsion." },
            { "Turbine_Shaft", "The turbine shaft connects the turbine to the compressor, transmitting power generated by the combustion process to drive the engine's components." },
            { "Core_Shell", "The core shell encases the engine's central components, such as the compressor, combustion chamber, and turbine, providing structural support and housing for these vital parts." },
            { "Fan_Rim", "The fan rim is the outermost structure of the engine's fan assembly, supporting the fan blades and aiding in the intake and compression of air for propulsion." },
            { "Turbofan", "The turbofan is a type of jet engine that combines a traditional turbojet engine with a large fan at the front, providing additional thrust and enhanced fuel efficiency." },
            { "Hi_pressure_blades", "High-pressure blades are located in the core of a jet engine and are responsible for extracting energy from the high-pressure gas flow produced by the combustion process, driving the engine's compressor and turbine stages." },
            { "Hi_pressure_blades2", "High-pressure blades are located in the core of a jet engine and are responsible for extracting energy from the high-pressure gas flow produced by the combustion process, driving the engine's compressor and turbine stages." },
            { "Low_PC_Body", "The low-pressure compressor body houses the components responsible for compressing incoming air at lower pressure levels before it reaches the high-pressure compressor, contributing to the overall compression process within the jet engine." },
            { "LP_blades", "The low-pressure blades are situated within the low-pressure compressor and are responsible for further compressing incoming air at lower pressure levels before it enters the combustion chamber, aiding in the overall compression process of the jet engine." },
            { "Turbine_Blades", "Turbine blades are mounted on the turbine shaft and are responsible for extracting energy from the high-pressure and high-velocity gases produced during combustion, converting this energy into rotational motion to drive the engine's compressor and other components." },
            { "Low_presure_ribs_and_shell", "The low-pressure ribs and shell provide structural support and enclosure for the low-pressure components of the jet engine, such as the low-pressure compressor and associated blades, contributing to the overall integrity and efficiency of the engine's operation." },
            { "Support_Ribs", "Support ribs are structural components within the engine's casing or housing that reinforce and support various parts, such as compressor and turbine sections, ensuring structural integrity and proper alignment during operation." },
            { "High_Pressure_compresor", "The high-pressure compressor is a crucial component of a jet engine's core, responsible for compressing incoming air before it enters the combustion chamber, thereby increasing its pressure and facilitating efficient combustion." },
            { "arm", "The arm of an industrial robot typically consists of multiple joints and links, allowing it to move in a coordinated manner to perform various tasks with precision and flexibility." },
            { "link arm", "The link arm in an industrial robot connects different joints together, providing structural support and facilitating movement in a coordinated manner to execute specific tasks efficiently." },
            { "Base low", "The base_low of an industrial robot serves as the foundation, providing stability and support to the entire robotic system while allowing for precise positioning and movement during operation." },
            { "Gripper", "The gripper of an industrial robot is responsible for grasping and manipulating objects, employing various mechanisms such as jaws, vacuum suction, or magnetic attraction to securely hold items during tasks." },
            { "foundation_low", "The foundation low of an industrial robot provides stability and support to the robot, ensuring proper alignment and minimizing vibrations during operation to maintain accuracy and safety." }

        };
    }

    #endregion
}
