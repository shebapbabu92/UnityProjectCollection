/*
 * CamPosScript.cs
 *
 * Project: Forklift
 *
 * Supported Unity version: 2021.2.7f1 (tested)
 *
 * Author: Sheba Ponmelil Babu
 * 
 * Description:
 * This script adjusts the position of the "Main Camera" in the scene. 
 * The camera is moved to a new position on the Z-axis at the start of the scene and then updated each frame.
 * 
 * Purpose:
 * - At the start (`Start` method), the camera is set to position (0, 0, 0).
 * - At each frame (`LateUpdate` method), the camera is reset to position (0, 0, -5).
 */

using UnityEngine;
using System.Collections;

public class CamPosScript : MonoBehaviour
{
    #region Unity Callbacks

    // Start is called before the first frame update
    void Start()
    {
        // Move the Main Camera to the origin at the start
        GameObject.Find("Main Camera").transform.position = new Vector3(0, 0, 0);
    }

    // LateUpdate is called once per frame, after all Update functions have been called
    void LateUpdate()
    {
        // Move the Main Camera to a fixed position at (0, 0, -5) each frame
        GameObject.Find("Main Camera").transform.position = new Vector3(0, 0, -5);
    }

    #endregion
}
