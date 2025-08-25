using UnityEngine;

public class AttachToCamera : MonoBehaviour
{
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera != null)
        {
            // Make the object a child of the camera
            transform.SetParent(targetCamera.transform);

            // Reset local position to (0,0,0)
            transform.localPosition = Vector3.zero;

            // (Optional) Reset rotation if needed
            transform.localRotation = Quaternion.identity;
        }
    }
}