/*
 * SceneLoaderLast.cs
 *
 * Project: Focus AR 
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This class handles scene transitions based on the player's focus on specific colliders.
/// The scene transitions are triggered when the player is within a specified distance from the colliders.
/// Additionally, it manages instructional UI elements that provide guidance to the player.
/// </summary>
public class SceneLoaderLast : MonoBehaviour
{
    #region PROPERTIES

    [Header("Scene Transition Settings")]
    public string ColliderNameNext;        // Name of the collider that triggers the next scene transition
    public string ColliderNamePrevious;    // Name of the collider that triggers the previous scene transition
    public float maxDistance = 2f;         // Maximum distance within which the collider triggers will be detected
    public string nextSceneName;           // The name of the next scene to load
    public string previousSceneName;       // The name of the previous scene to load

    [Header("Instructional UI Settings")]
    public float thresholdDistance = 5f;   // Distance threshold for displaying instructions
    public Text instructionText;           // UI Text to display the instructions
    public GameObject panel;               // UI panel for instructions and image display
    public GameObject image;               // UI image to accompany instructions

    public float displayDuration = 5f;     // Duration for displaying the instructional UI elements

    #endregion

    #region PRIVATE_VARIABLES

    private Vector3 initialCameraPosition; // Store the initial camera position for distance calculations
    

    #endregion

    #region UNITY_EVENT_FUNCTIONS

    /// <summary>
    /// Unity's Start method is called when the script is initialized. It sets up initial instruction text,
    /// enables the UI elements, and starts the coroutine to disable the UI after a set duration.
    /// </summary>
    void Start()
    {
        initialCameraPosition = Camera.main.transform.position;
        // Set the initial instruction text when the scene is loaded
        SetInstructionText("In this scene, you can see an animation showing each part of the jet engine separately. " +
                           "You can focus on each component to view its description.");

        // Enable the panel and instructional text
        SetPanelAndTextActive(true);

        // Start the coroutine to disable the panel and text after displayDuration
        StartCoroutine(DisablePanelAndText());
    }

    /// <summary>
    /// Unity's Update method is called every frame. It performs raycasting from the camera to detect colliders
    /// that match the names of the designated colliders for scene transition.
    /// </summary>
    void Update()
    {
        // Cast a ray from the center of the camera's viewport
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if the ray hits a collider within the specified maxDistance and matches ColliderNameNext
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.name == ColliderNameNext)
            {
                // Load the next scene if the collider matches
                SceneManager.LoadScene(nextSceneName);
            }
        }

        // Check if the ray hits a collider within the specified maxDistance and matches ColliderNamePrevious
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.name == ColliderNamePrevious)
            {
                // Load the previous scene if the collider matches
                SceneManager.LoadScene(previousSceneName);
            }
        }
    }

    #endregion

    #region HELPER_FUNCTIONS

    /// <summary>
    /// Sets the instruction text in the UI.
    /// </summary>
    /// <param name="text">The text to display in the instruction text field.</param>
    void SetInstructionText(string text)
    {
        if (instructionText != null)
        {
            instructionText.text = text;
        }
    }

    /// <summary>
    /// Enables or disables the instructional UI elements (panel, text, and image).
    /// </summary>
    /// <param name="active">True to enable the UI elements, false to disable them.</param>
    void SetPanelAndTextActive(bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }

        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(active);
        }

        if (image != null)
        {
            image.gameObject.SetActive(active);
        }
    }

    /// <summary>
    /// Coroutine that disables the instructional UI elements after a set duration.
    /// </summary>
    /// <returns>Waits for the specified duration before disabling the UI elements.</returns>
    IEnumerator DisablePanelAndText()
    {
        yield return new WaitForSeconds(displayDuration);
        SetPanelAndTextActive(false);
    }

    #endregion
}
