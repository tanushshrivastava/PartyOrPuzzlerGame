using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    int currentScene;
    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(currentScene + 1);
    }

}
