using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IinteractableObject
{
    /// <summary>
    /// Whether or not the object is highlighted
    /// </summary>
    bool _highlighted { get; set; }

    /// <summary>
    /// Called when the object is highlighted
    /// </summary>
    void OnHighlighted();

    /// <summary>
    /// Called when the object is no longer highlighted
    /// </summary>
    void OnHighlightEnd();

    /// <summary>
    /// Called every frame while the object is highlighted
    /// </summary>
    void WhileHighlighted();
}
