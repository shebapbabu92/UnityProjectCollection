/*
 * ButtonScriptScene2.cs
 *
 * Project: Click AR
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 * 
 * Description:
 * This script is attached to UI buttons in Scene2. It allows the user to navigate between 
 * Scene2, Scene1, and Scene3 by calling the appropriate methods when the buttons are clicked.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScriptScene2 : MonoBehaviour
{
    #region Public Methods

    // Method to load Scene3 when the "Next" button is clicked
    public void NextScene()
    {
        // Asynchronously load Scene3 in the background
        SceneManager.LoadSceneAsync("Scene3");
    }

    // Method to load Scene1 when the "Previous" button is clicked
    public void PreviousScene()
    {
        // Asynchronously load Scene1 in the background
        SceneManager.LoadSceneAsync("Scene1");
    }

    #endregion
}
