using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractableElement
{
    /// <summary>
    /// What text should be shown when the element is being interacted with
    /// </summary>
    string InteractionText { get; set; }

    /// <summary>
    /// Called when the player enters the elements interaction trigger
    /// </summary>
    public void OnInteractionEnter();

    /// <summary>
    /// Called when the player leaves the elements interaction trigger
    /// </summary>
    public void OnInteractionExit();

    /// <summary>
    /// Called when the player is in the component and trys to interact with it
    /// </summary>
    public void OnInteracted();
}
