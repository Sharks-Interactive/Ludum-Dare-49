using UnityEngine;

public class TimeManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 0.0f;
    }

    public void SetTime(float NewTime) => Time.timeScale = NewTime;
}
