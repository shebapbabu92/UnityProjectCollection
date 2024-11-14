using UnityEngine;
using UnityEngine.UI;

public class ColliderImageDisplay : MonoBehaviour
{
    public Camera mainCamera;
    public Image imageToShow;

    private void Update()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Collider collider = hit.collider;
            if (collider != null)
            {
                Debug.Log("Collider hit: " + collider.name);
                // Check if the collider has an associated image
                Image image = collider.GetComponentInChildren<Image>();
                if (image != null)
                {
                    // Display the image
                    imageToShow.sprite = image.sprite;
                    imageToShow.enabled = true;
                }
                else
                {
                    // Hide the image if no associated image is found
                    imageToShow.enabled = false;
                }
            }
            else
            {
                // Hide the image if no collider is hit
                imageToShow.enabled = false;
            }
        }
    }
}
