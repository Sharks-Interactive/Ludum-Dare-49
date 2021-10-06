using System;
using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    // Update is called once per frame
    public void SetDifficulty(string diff)
    {
        int dif = Int32.Parse(diff);
        PlayerPrefs.SetInt("Difficulty", dif);
    }
}
