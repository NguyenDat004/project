using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void Update()
    {
        // Ensure the object this script is attached to always faces the camera
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
