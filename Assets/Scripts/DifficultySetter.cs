using System;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySetter : MonoBehaviour
{
    public Toggle check;

    private void Start()
    {
        check.isOn = PlayerPrefs.GetInt("LQ", 0) == 0 ? false : true;
    }

    // Update is called once per frame
    public void SetDifficulty(string diff)
    {
        float dif = float.Parse(diff);
        PlayerPrefs.SetFloat("Difficulty", dif);
    }

    public void SetLowQuality(bool Low) => PlayerPrefs.SetInt("LQ", Low ? 1 : 0);
}
