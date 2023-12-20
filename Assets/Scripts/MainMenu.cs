using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //loads Level1 scene
    public void playGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    //Loads Model Viewer Scene
    public void playModelViewer()
    {
        SceneManager.LoadScene(2);
    }
    //Loads Controls scene 
    public void playControls()
    {
        SceneManager.LoadScene(5);
    }

    //Loads Main Menu Scene
    public void playReturnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //Closes Game
    public void ExitGame()
    {
        Application.Quit();
    }
}
