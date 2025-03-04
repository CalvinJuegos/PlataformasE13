using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colliderBehaviour : MonoBehaviour
{
    public Collider2D backOffArea;

    public void EnableBackOffArea()
    {
        if (backOffArea != null)
        {
            backOffArea.enabled = true;
            Debug.Log("Area ON");
        }
    }

    public void DisableBackOffArea()
    {
        if (backOffArea != null)
        {
            backOffArea.enabled = false;
            Debug.Log("Area OFF");
        }
    }

}
