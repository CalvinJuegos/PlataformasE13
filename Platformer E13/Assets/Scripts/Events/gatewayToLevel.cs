using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gatewayToLevel : MonoBehaviour
{

    [SerializeField]
    public int sceneID;
    public void GoToLevel()
    {
        // Siguiente nivel
        Debug.Log(sceneID);
        SceneManager.LoadSceneAsync(sceneID);
    }
}
