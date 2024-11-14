/*
 * ButtonScriptScene3.cs
 *
 * Project: Click AR
 *
 * Supported Unity version: 2019.2.4f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 * 
 * Description:
 * This script is attached to UI buttons in Scene3. It allows the user to navigate between 
 * Scene1, Scene2, and Scene3 by calling the appropriate methods when the buttons are clicked.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScriptScene3 : MonoBehaviour
{
    #region Public Methods

    // Method to load Scene1 when the "Next" button is clicked
    public void NextScene()
    {
        // Asynchronously load Scene1 in the background
        SceneManager.LoadSceneAsync("Scene1");
    }

    // Method to load Scene2 when the "Previous" button is clicked
    public void PreviousScene()
    {
        // Asynchronously load Scene2 in the background
        SceneManager.LoadSceneAsync("Scene2");
    }

    #endregion
}
