using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gatewayScript : MonoBehaviour
{

    [SerializeField]
    public int sceneID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has a player tag or component
        if (other.CompareTag("Player") || other.GetComponent<PlayerController>() != null)
        {
            Level();
        }
    }
    public void Level()
    {
        // Siguiente nivel
        Debug.Log(sceneID);
        SceneManager.LoadSceneAsync(sceneID);
    }
}
