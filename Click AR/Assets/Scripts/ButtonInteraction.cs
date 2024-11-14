/*
 * ButtonInteraction.cs
 *
 * Project: Click AR 
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 * 
 * Description:
 * This script handles user interaction with objects in the scene via touch inputs. 
 * It displays information about the clicked objects on the UI, plays associated sounds, 
 * and shows images related to the object selected by the user.
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteraction : MonoBehaviour
{
    #region Public Variables

    // UI elements and references to show information on touch interactions
    public Text instructionText; // Text element to display the description of the clicked object
    public GameObject panel; // Reference to the UI panel for showing object details
    public GameObject image; // Reference to the image that may show additional info
    private AudioSource currentlyPlayingAudioSource; // AudioSource to play sounds related to clicked objects

    #endregion

    #region Private Variables

    private Dictionary<string, string> colliderTextMap; // A map to store object names and their associated descriptions

    #endregion

    #region Unity Callbacks

    // Called at the start of the script
    void Start()
    {
        // Hide the panel, image, and instruction text by default
        panel.SetActive(false);
        image.SetActive(false);
        instructionText.gameObject.SetActive(false);

        // Initialize the dictionary with the object names and descriptions
        InitializeColliderTextMap();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if a touch is detected on mobile (or mouse input on desktop)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                // Process the touch when it first begins
                ProcessTouch(touch.position);
            }
        }
    }

    #endregion

    #region Methods

    // Method to process the touch input and interact with objects
    void ProcessTouch(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition); // Create a ray from the camera to the touch position
        RaycastHit hit; // Variable to store information about what the ray hits

        // Perform a raycast to detect if any object is touched
        if (Physics.Raycast(ray, out hit))
        {
            // If an object is hit, handle interaction with it
            HandleInteraction(hit.collider.gameObject);
        }
        else
        {
            // If no object is hit, reset the UI
            ResetUI();
        }
    }

    // Method to handle the interaction with a clicked object
    void HandleInteraction(GameObject hitObject)
    {
        string colliderName = hitObject.name; // Get the name of the collider that was clicked
        string customText;

        // Check if the collider name exists in the dictionary
        if (colliderTextMap.TryGetValue(colliderName, out customText))
        {
            // Update the instruction text with the object name and description
            instructionText.text = $"Clicked at: {colliderName}\n{customText}";
        }

        // Play the associated audio if the object has an AudioSource component
        AudioSource audioSource = hitObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            if (currentlyPlayingAudioSource != null && currentlyPlayingAudioSource != audioSource)
            {
                currentlyPlayingAudioSource.Stop(); // Stop the previous audio if a new one is played
            }
            currentlyPlayingAudioSource = audioSource;
            audioSource.Play();
        }

        // Enable UI elements for displaying the information
        EnablePanel();
        EnableInstructionText();
        EnableImage();
    }

    // Method to reset the UI when no object is clicked
    void ResetUI()
    {
        DisablePanel();
        DisableInstructionText();
        DisableImage();
        if (currentlyPlayingAudioSource != null)
        {
            currentlyPlayingAudioSource.Stop();
            currentlyPlayingAudioSource = null;
        }
    }

    // Methods to enable/disable UI components

    void EnablePanel()
    {
        panel.SetActive(true);
    }

    void DisablePanel()
    {
        panel.SetActive(false);
    }

    void EnableInstructionText()
    {
        instructionText.gameObject.SetActive(true);
    }

    void DisableInstructionText()
    {
        instructionText.gameObject.SetActive(false);
    }

    void EnableImage()
    {
        image.SetActive(true);
    }

    void DisableImage()
    {
        image.SetActive(false);
    }

    #endregion

    #region Dictionary Initialization

    // Method to initialize the dictionary with object names and their descriptions
    void InitializeColliderTextMap()
    {
        // Initialize the dictionary with object names and descriptions
        colliderTextMap = new Dictionary<string, string>
        {
            { "Inner_Nozzle", "The inner nozzle guides and shapes the flow of exhaust gases exiting the combustion chamber in a jet engine." },
            { "Outer_Nozzle", "The outer nozzle surrounds the inner components of the jet engine and helps to further direct and accelerate the exhaust gases for efficient propulsion." },
            { "Turbine_Shaft", "The turbine shaft connects the turbine to the compressor, transmitting power generated by the combustion process to drive the engine's components." },
            { "Core_Shell", "The core shell encases the engine's central components, such as the compressor, combustion chamber, and turbine, providing structural support and housing for these vital parts." },
            { "Fan_Rim", "The fan rim is the outermost structure of the engine's fan assembly, supporting the fan blades and aiding in the intake and compression of air for propulsion." },
            { "Turbofan", "The turbofan is a type of jet engine that combines a traditional turbojet engine with a large fan at the front, providing additional thrust and enhanced fuel efficiency." },
            { "Hi_pressure_blades", "High-pressure blades are located in the core of a jet engine and are responsible for extracting energy from the high-pressure gas flow produced by the combustion process, driving the engine's compressor and turbine stages." },
            { "Low_PC_Body", "The low-pressure compressor body houses the components responsible for compressing incoming air at lower pressure levels before it reaches the high-pressure compressor." },
            { "LP_blades", "The low-pressure blades are situated within the low-pressure compressor and are responsible for further compressing incoming air at lower pressure levels before it enters the combustion chamber." },
            { "Turbine_Blades", "Turbine blades are mounted on the turbine shaft and are responsible for extracting energy from the high-pressure and high-velocity gases produced during combustion." },
            { "Low_presure_ribs_and_shell", "The low-pressure ribs and shell provide structural support and enclosure for the low-pressure components of the jet engine." },
            { "Support_Ribs", "Support ribs are structural components within the engine's casing or housing that reinforce and support various parts." },
            { "High_Pressure_compresor", "The high-pressure compressor is responsible for compressing incoming air before it enters the combustion chamber." },
            { "arm", "The arm of an industrial robot typically consists of multiple joints and links, allowing it to move in a coordinated manner to perform various tasks." },
            { "link arm", "The link arm in an industrial robot connects different joints together, providing structural support and facilitating movement." },
            { "Base low", "The base_low of an industrial robot serves as the foundation, providing stability and support to the entire robotic system." },
            { "Gripper", "The gripper of an industrial robot is responsible for grasping and manipulating objects." },
            { "foundation_low", "Foundation low refers to the lower base structure of an industrial robot that ensures stability and minimizes vibrations." }
        };
    }

    #endregion
}
