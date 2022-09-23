using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public static SceneManagment Instance;

    public void Awake()
    {
        if(Instance != null)
        {
            return;
        }
        else
        {
            Instance = this;
        }
    }
    
    public void ChangeToGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void ChangeToMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
