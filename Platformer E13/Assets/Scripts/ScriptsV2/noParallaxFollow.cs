using UnityEngine;
using Cinemachine;

public class noParallaxFollow : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 initialPosition;
    private Vector3 cameraInitialPosition;

    void Start()
    {
        // Get the main camera's transform
        cameraTransform = Camera.main.transform;

        // Record the initial positions of the background and the camera
        initialPosition = transform.position;
        cameraInitialPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // Calculate the camera's movement
        Vector3 cameraMovement = cameraTransform.position - cameraInitialPosition;

        // Adjust the background's position to follow the camera's movement
        transform.position = initialPosition + new Vector3(cameraMovement.x, cameraMovement.y, 0);
    }
}