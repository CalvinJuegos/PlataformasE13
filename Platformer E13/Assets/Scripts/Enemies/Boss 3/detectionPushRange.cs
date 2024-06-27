using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionPushRange : MonoBehaviour
{
    // Create property for range 
    private bool _inRangeForPush;
    public bool InRangeForPush { get 
        {
            return _inRangeForPush;
        } private set {
            _inRangeForPush = value;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InRangeForPush = true;
        }

    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        InRangeForPush = false;
    }
}
