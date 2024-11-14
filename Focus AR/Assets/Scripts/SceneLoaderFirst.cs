/*
 * SceneLoaderFirst.cs
 *
 * Project: Focus AR 
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 */

using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for loading a new scene after a specified delay. It is meant to be attached to a GameObject that handles scene transitions.
/// </summary>
public class SceneLoaderFirst : MonoBehaviour
{
    #region PROPERTIES

    [Header("Scene Loading Settings")]
    public float delayTime = 5f;        // Time in seconds before the next scene is loaded
    public string nextSceneName;        // The name of the next scene to load

    #endregion

    #region UNITY_EVENT_FUNCTIONS

    /// <summary>
    /// Unity's Start method. Invoked when the script is initialized. It invokes the LoadNextScene method after the specified delay.
    /// </summary>
    void Start()
    {
        // Invoke the LoadNextScene method after the specified delay time
        Invoke("LoadNextScene", delayTime);
    }

    #endregion

    #region SCENE_LOADING_FUNCTIONS

    /// <summary>
    /// This method is invoked after the delay time. It loads the next scene based on the scene name provided.
    /// </summary>
    void LoadNextScene()
    {
        // Log message to console for debugging purposes
        Debug.Log("Loading next scene: " + nextSceneName);

        // Load the next scene by its name
        SceneManager.LoadScene(nextSceneName);
    }

    #endregion
}
