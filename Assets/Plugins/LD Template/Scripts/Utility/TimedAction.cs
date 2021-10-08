using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TimedAction : MonoBehaviour
{
    [Header("Settings")]
    public float TimeToRunAction;

    [Serializable]
    public class CustomEvent : UnityEvent { }

    [SerializeField]
    public CustomEvent m_Event = new CustomEvent();

    private void OnEnable() => StartCoroutine(DelayedRunAction());

    private IEnumerator DelayedRunAction ()
    {
        yield return new WaitForSeconds(TimeToRunAction);
        m_Event.Invoke();
    }
}
