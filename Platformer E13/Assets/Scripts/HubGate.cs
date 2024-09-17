using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubGate : MonoBehaviour
{
    [SerializeField]
    public int sceneID;
    public GameObject messageBox;

    public void ConfirmTransition()
    {
        // Show a dialogue box in UI to confirm transition
        messageBox.SetActive(true);
    }

    public void CancelTransition()
    {
        // Hide the dialogue box
        messageBox.SetActive(false);
    }

    public void AcceptTransition()
    {
        // Hide the dialogue box
        messageBox.SetActive(false);
        // Load the next scene
        Debug.Log(sceneID);
        SceneManager.LoadSceneAsync(sceneID);
    }
}
