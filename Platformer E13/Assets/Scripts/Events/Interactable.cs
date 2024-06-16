using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public UnityEvent interactAction;

    public void OnInteract(InputAction.CallbackContext interact)
    {
        Debug.Log("Pressed E    ");
        if (interact.started && isInRange)
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
}
