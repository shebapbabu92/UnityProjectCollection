/*
 * ButtonScriptScene1.cs
 *
 * Project: Click AR
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 * 
 * Description:
 * This script is attached to a UI button in the first scene (Scene1). When the button is clicked,
 * it will load the next scene (Scene2) asynchronously, allowing for smooth scene transitions.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScriptScene1 : MonoBehaviour
{
    #region Public Methods

    // Method to load Scene2 when the button is clicked
    public void StartScene()
    {
        // Asynchronously load Scene2 in the background
        SceneManager.LoadSceneAsync("Scene2");
    }

    #endregion
}
