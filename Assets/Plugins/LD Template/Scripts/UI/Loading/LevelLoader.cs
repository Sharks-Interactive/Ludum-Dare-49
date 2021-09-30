using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles displaying and updating a loading screen
/// </summary>
public class LevelLoader : MonoBehaviour {

    [Header("UI Components")]
    [Tooltip("The canvas group of the loading screen")]
    public CanvasGroup LoadingScreen;

    [Tooltip("A slider to display current progress")]
    public Slider ProgressSlider;

    [Tooltip("Text to display current progress")]
    public TextMeshProUGUI ProgressText;

    /// <summary>
    /// A functions for buttons to call that triggers level loading
    /// </summary>
    /// <param name="_sceneIndex"></param>
    public void LoadScene(int _sceneIndex) => StartCoroutine(LoadAsync(_sceneIndex));

    IEnumerator LoadAsync(int _sceneIndex)
    {
        //Allows us to persist to the next scene
        DontDestroyOnLoad(this);

        //Starts loading the scene and stores it in a variable
        AsyncOperation t_operation = SceneManager.LoadSceneAsync(_sceneIndex);
        
        //Enables the loading screen (Should be a child of this)
        LoadingScreen.alpha = 1.0f;

        //Called every frame while loading is carried out
        while (!t_operation.isDone)
        {
            //Update the value of the progress slider and text percent display (We subtract 10 for later uses)
            ProgressSlider.value = (t_operation.progress * 100f) - 10;
            ProgressText.text = (t_operation.progress * 100f) - 10 + "%";

            //Delay the loop
            yield return null;
        }
    }

    /// <summary>
    /// Calls loading functions on scripts which still have logic that needs to be loaded
    /// </summary>
    public void ScriptLoading ()
    {
        GameObject[] _objectsThatNeedLoading = GameObject.FindGameObjectsWithTag("LoadableObject");
        //Get all gameobjects in scene with loading tag
        //Get IloadableObject or something and call a function with some type of callback/promise
        //Once everything is complete hide the loading screen

        LoadingScreen.alpha = 0.0f;
    }
}
