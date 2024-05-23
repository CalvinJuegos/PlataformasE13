using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    // Posicion inicial
    Vector2 startPos;

    //Valor del objeto en el juego
    float startZ;

    //Movimiento de camara desde el principio
    Vector2 camMove=> (Vector2)cam.transform.position-startPos;

    // Distancia de la dirección Z
    float distanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => cam.transform.position.z + (distanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane);
    float parallaxFact => Mathf.Abs(distanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPos + camMove * parallaxFact;

        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
