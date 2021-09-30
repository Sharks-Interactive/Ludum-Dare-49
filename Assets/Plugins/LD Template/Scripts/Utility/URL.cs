using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Opens a specified URL when told
/// </summary>
public class URL : MonoBehaviour
{
    public string Website;

    public void OpenURL() => Application.OpenURL(Website);
}
