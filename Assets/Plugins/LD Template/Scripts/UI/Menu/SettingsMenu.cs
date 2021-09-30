using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// A class that manages setting all sorts of video and performance related things in the Unity Engine
/// /!\ NOTE: NOT ALL SETTINGS PERSIST (YET) /!\
/// </summary>
public class SettingsMenu : MonoBehaviour {

    #region Components

    [Header("UI Components")]
    [Tooltip("Control for Post Processing")]
    public Toggle PostProccessToggle;

    [Tooltip("Control for Fullscreen")]
    public Toggle FullscreenToggle;

    [Tooltip("Control for volume in the range of -80 to 0")]
    public Slider VolSlider;

    [Tooltip("Control for quality preset")]
    public TMP_Dropdown QualDropdown;

    [Tooltip("Control for resolution selection")]
    public TMP_Dropdown ResDropdown;

    [Tooltip("The mixer whose volume should be modified, must have an exposed float called, 'Volume'")]
    public AudioMixer AudioMixer;

    #endregion

    #region Cache
    //A float to store the out val of the initial volume level
    private float _initialVolume;

    //A list of availible screen resolutions
	private Resolution[] Resolutions;
    #endregion

    //Initialize UI to the currently active values
    void Start () 
    {
        SetUIInitialState();
        InitializeResolutionSettings();
	}

    /// <summary>
    /// Initializes all UI components to the current settings at startup
    /// </summary>
    public void SetUIInitialState ()
    {
        //Initializes toggles to their initial value
        PostProccessToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("PP"));
        FullscreenToggle.isOn = Screen.fullScreen;

        //Get current volume and set the volume slider to it
        AudioMixer.GetFloat("Volume", out _initialVolume);
        VolSlider.value = _initialVolume;

        //Init Quality Level dropdown
        QualDropdown.value = QualitySettings.GetQualityLevel();
    }

    /// <summary>
    /// Gets a list of resolutions and populates the resolution dropdown with them
    /// </summary>
    public void InitializeResolutionSettings ()
    {
        //Get list of resolutions, then reset the resolution dropdown and populate it with those
        Resolutions = Screen.resolutions;
        ResDropdown.ClearOptions();

        //Define local cache variables to store options and the currently selected quality preset
        List<string> _options = new List<string>();
        int _currentResolutionIndex = 0;

        for (int i = 0; i < Resolutions.Length; i++)
        {
            //Add a human readable version of the resolution to our options list
            _options.Add(Resolutions[i].width + " x " + Resolutions[i].height);

            //If the options resolution matches our screens current res set it to be the active one
            if (Resolutions[i].width == Screen.currentResolution.width &&
                Resolutions[i].height == Screen.currentResolution.height)
                _currentResolutionIndex = i;
        }

        //Give the resolution dropdown our new options, set it's current option and refresh it
        ResDropdown.AddOptions(_options);
        ResDropdown.value = _currentResolutionIndex;
        ResDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Sets the applications resolution
    /// </summary>
    /// <param name="_resolutionIndex"> The index of the resolution in a list, primarily the index of it in a dropdown </param>
	public void SetResolution (int _resolutionIndex)
    {
        Resolution _resolution = Resolutions[_resolutionIndex];
        Screen.SetResolution(_resolution.width, _resolution.height, Screen.fullScreen);
	}

    /// <summary>
    /// Sets the applications volume
    /// </summary>
    /// <param name="_volume"> The volume level from -80 to 12 </param>
	public void SetVolume (float _volume) => AudioMixer.SetFloat("Volume", _volume);
    
    /// <summary>
    /// Sets the applications quality preset
    /// </summary>
    /// <param name="_qIndex"> What quality level to set the application to as an index in the list </param>
	public void SetQuality (int _qIndex) => QualitySettings.SetQualityLevel(_qIndex);       
    
    /// <summary>
    /// Sets the applications fullscreen state
    /// </summary>
    /// <param name="_full"> Whether or not the application should be fullscreen </param>
	public void SetFullscreen (bool _full) => Screen.fullScreen = _full;     
    
    /// <summary>
    /// Sets the value of the FPS counter enabled PlayerPref thus saving it to memory
    /// </summary>
    /// <param name="_state"> Whether or not Post Processing is enabled </param>
    public void SetFpsCounter (bool _state) => PlayerPrefs.SetInt("FPS", Convert.ToInt32(_state));

    /// <summary>
    /// Sets the value of the Post Process PlayerPref thus saving it to memory
    /// </summary>
    /// <param name="_state"> Whether or not Post Processing is enabled </param>
    public void SetPostProccess(bool _state) => PlayerPrefs.SetInt("PP", Convert.ToInt32(_state));
}
