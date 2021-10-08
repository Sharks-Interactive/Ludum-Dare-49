using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables text when something enters a collider
/// </summary>
public class TextEnabler : MonoBehaviour {

    public float Time = 1.6f;
    public GameObject Text;

    void OnTriggerEnter()
    {
        Text.SetActive(false);
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(Time);
        Text.SetActive(false);
    }
}
