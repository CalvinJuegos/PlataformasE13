using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoParallaxFollow : MonoBehaviour
{
    public Transform cameraTransform;

    private Vector3 offset;

    private void Start()
    {
        // Calcula la diferencia inicial entre la posición de la cámara y la posición del fondo
        offset = transform.position - cameraTransform.position;
    }

    private void LateUpdate()
    {
        // Actualiza la posición del fondo para que siga la cámara, manteniendo el offset inicial
        transform.position = cameraTransform.position + offset;
    }
}