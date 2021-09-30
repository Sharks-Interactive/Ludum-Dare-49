using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomInteraction : MonoBehaviour, IInteractableElement
{
    [TextArea(3, 5)]
    public string InteractionMessage;

    public string InteractionText { get; set; }

    [Serializable]
    public class CustomEvent : UnityEvent { }

    [SerializeField]
    public CustomEvent m_Event = new CustomEvent();

    void Start()
    {
        InteractionText = InteractionMessage;
    }

    public void OnInteractionExit() { }

    public void OnInteractionEnter() { }

    public void OnInteracted()
    {
        m_Event.Invoke();
    }
}
