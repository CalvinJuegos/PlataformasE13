using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public UnityEvent interactAction;
    public InputAction interactInputAction;

    public void Awake()
    {
        interactInputAction.performed += OnInteract;
        interactInputAction.Enable();
    }

    public void OnInteract(InputAction.CallbackContext interact)
    {
        Debug.Log("Pressed E    ");
        Debug.Log($"Pressed E: {interact.phase}");
        if (interact.performed && isInRange)
        {
            Debug.Log("Action");
            interactAction.Invoke();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            Debug.Log("Inside range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isInRange = false;
    }
    private void OnDisable()
    {
        interactInputAction.performed -= OnInteract;
        interactInputAction.Disable();
    }
}
