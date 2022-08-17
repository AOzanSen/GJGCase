using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScene : MonoBehaviour
{
    public void RestartSceneButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
