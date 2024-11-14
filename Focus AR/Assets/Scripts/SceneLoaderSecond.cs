/*
 * SceneLoaderSecond.cs
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
/// This class handles scene transitions based on raycasting from the player's camera to colliders in the environment.
/// It also provides instructional text to guide the player in the scene. The instructions are shown when the scene is loaded,
/// and can be disabled after a set duration.
/// </summary>
public class SceneLoaderSecond : MonoBehaviour
{
    #region PROPERTIES

    [Header("Scene Transition Settings")]
    public string ColliderNameNext;        // Name of the collider that triggers the next scene change
    public string ColliderNamePrevious;    // Name of the collider that triggers the previous scene change
    public float maxDistance = 2f;         // Maximum distance within which the raycast can hit the collider
    public string nextSceneName;           // The name of the next scene to load
    public string previousSceneName;       // The name of the previous scene to load

    [Header("Instructional UI Settings")]
    public float thresholdDistance = 5f;   // Distance threshold for showing instructions (not used in the current code)
    public Text instructionText;           // UI Text to display instructional messages
    public GameObject panel;               // UI panel for instructions and image display
    public GameObject image;               // UI image to accompany instructions

    public float displayDuration = 5f;     // Duration for displaying the instructional UI elements

    private Vector3 initialCameraPosition; // Initial camera position for potential distance calculations (not used in the current code)
   

    // Instructional texts that explain the scene to the player
    private string text = "In this scene you can see two 3D models of jet engines and one 3D model of an industrial robot. " +
                             "Go closer to the 3D model to get more information. If you want to go to the previous scene, " +
                             "walk towards the left from the left Jet engine. If you want to go to the next scene, walk towards the right from the right 3D objects.";
    
    #endregion

    #region UNITY_EVENT_FUNCTIONS

    /// <summary>
    /// Unity's Start method is called when the script is initialized. It sets up the instruction text, 
    /// enables the instructional UI, and starts the coroutine to disable the UI after a set duration.
    /// </summary>
    void Start()
    {
        initialCameraPosition = Camera.main.transform.position;

        // Set the initial instruction text when the scene is loaded
        SetInstructionText(text);

        // Enable the panel and text to show the instructions
        SetPanelAndTextActive(true);

        // Start the coroutine to disable the panel and text after the specified duration
        StartCoroutine(DisablePanelAndText());
    }

    /// <summary>
    /// Unity's Update method is called every frame. It performs raycasting from the center of the camera's viewport
    /// to detect colliders that match the specified names for scene transitions.
    /// </summary>
    void Update()
    {
        // Cast a ray from the center of the camera's viewport (screen center)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if the ray hits a collider within the specified maxDistance and matches the ColliderNameNext
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.name == ColliderNameNext)
            {
                // Load the next scene if the collider name matches
                SceneManager.LoadScene(nextSceneName);
            }
        }

        // Check if the ray hits a collider within the specified maxDistance and matches the ColliderNamePrevious
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.collider.name == ColliderNamePrevious)
            {
                // Load the previous scene if the collider name matches
                SceneManager.LoadScene(previousSceneName);
            }
        }
    }

    #endregion

    #region HELPER_FUNCTIONS

    /// <summary>
    /// Sets the instruction text in the UI.
    /// </summary>
    /// <param name="text">The instruction text to display in the instruction UI element.</param>
    void SetInstructionText(string text)
    {
        if (instructionText != null)
        {
            instructionText.text = text;
        }
    }

    /// <summary>
    /// Enables or disables the instructional UI elements (panel, instruction text, and image).
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
