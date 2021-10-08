using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles common main menu related button functions
/// </summary>
public class MenuManager : MonoBehaviour
{
    //See LevelLoader.cs for loading stuff

    public void Quit () => Application.Quit();

    public void SimpleLoad(int t_scene) => SceneManager.LoadScene(t_scene);
}
