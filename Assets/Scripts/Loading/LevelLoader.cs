using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SharkUtils;

namespace Chrio.World.Loading
{
    /// <summary>
    /// Handles displaying and updating a loading screen
    /// </summary>
    public class LevelLoader : MonoBehaviour
    {
        [Tooltip("Global Game State")]
        public Game_State.State GameState;

        [Header("UI Components")]
        [Tooltip("The canvas group of the loading screen")]
        public CanvasGroup LoadingScreen;

        [Tooltip("A slider to display current progress")]
        public Slider ProgressSlider;

        [Tooltip("Text to display current progress")]
        public TextMeshProUGUI ProgressText;

        [Tooltip("Whether or not this level loader is a debug one")]
        public bool Debug;

        #region Cache
        private int _totalScripts;
        private int _loadedScripts;
        #endregion

        void Start()
        {
            if (Debug) StartScriptLoading();
        }

        /// <summary>
        /// A functions for buttons to call that triggers level loading
        /// </summary>
        /// <param name="_sceneIndex"></param>
        public void LoadScene(int _sceneIndex) => StartCoroutine(LoadAsync(_sceneIndex));

        IEnumerator LoadAsync(int _sceneIndex)
        {
            //Allows us to persist to the next scene
            //DontDestroyOnLoad(this);

            //Starts loading the scene and stores it in a variable
            AsyncOperation t_operation = SceneManager.LoadSceneAsync(_sceneIndex);

            //Enables the loading screen (Should be a child of this)
            LoadingScreen.alpha = 1.0f;

            //Called every frame while loading is carried out
            while (!t_operation.isDone)
            {
                //Update the value of the progress slider and text percent display (We subtract 30 for later uses)
                ProgressSlider.value = (t_operation.progress * 100f) - 30;
                ProgressText.text = "Loading Game... " + ((t_operation.progress * 100f) - 30) + "%";

                //Delay the loop
                yield return null;
            }

            StartScriptLoading();

            while (_loadedScripts < _totalScripts)
            {
                //Update the value of the progress slider and text percent display (We add 70 for previous uses)
                // Also we divide 30  by the completion to turn the 0 - 100 to 0 - 30%
                ProgressSlider.value = 70 + (30 / (_loadedScripts / _totalScripts));
                ProgressText.text = "Loading Game... " + (70 + (30 / (_loadedScripts / _totalScripts))) + "%";
            }

            // Hide the loading screen when loading is done
            LoadingScreen.alpha = 0.0f;
        }

        /// <summary>
        /// Calls loading functions on scripts which still have logic that needs to be loaded
        /// </summary>
        public void StartScriptLoading()
        {
            // HACK: THIS IS BEYOND ATROCIOUS
            MonoBehaviour[] _allObjects = FindObjectsOfType<MonoBehaviour>(true);

            List<ILoadableObject> _interfaceList;
            ExtraFunctions.GetAllInterfaces<ILoadableObject>(out _interfaceList);

            _totalScripts = _interfaceList.Count;
            foreach (ILoadableObject t_lO in _interfaceList)
                t_lO.OnLoad(GameState, ScriptLoadComplete);
        }

        /// <summary>
        /// Used as a callback when a script has finished loading
        /// </summary>
        public void ScriptLoadComplete() => _loadedScripts++;
    }
}