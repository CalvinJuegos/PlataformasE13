using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoParallaxFollow : MonoBehaviour
{
    public Transform cameraTransform;

    private Vector3 offset;

    private void Start()
    {
        // Calcula la diferencia inicial entre la posici�n de la c�mara y la posici�n del fondo
        offset = transform.position - cameraTransform.position;
    }

    private void LateUpdate()
    {
        // Actualiza la posici�n del fondo para que siga la c�mara, manteniendo el offset inicial
        transform.position = cameraTransform.position + offset;
    }
}