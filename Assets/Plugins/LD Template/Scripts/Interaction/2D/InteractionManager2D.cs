using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager2D : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI InteractionText;

    #region Cache

    private Dictionary<string, IInteractableElement> _interactableElements = new Dictionary<string, IInteractableElement>();
    private string _currentObject;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BuildInteractionCache();
    }

    void Update()
    {
        if (_currentObject == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
            _interactableElements[_currentObject].OnInteracted();
    }

    void BuildInteractionCache()
    {
        GameObject[] _interGO = GameObject.FindGameObjectsWithTag("Interactable");

        for (int i = 0; i < _interGO.Length; i++)
            _interactableElements.Add(_interGO[i].name, _interGO[i].GetComponent<IInteractableElement>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interactable"))
            return;

        _interactableElements[collision.name].OnInteractionEnter();
        _currentObject = collision.name;
        InteractionText.text = _interactableElements[_currentObject].InteractionText;
        InteractionText.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interactable"))
            return;

        _interactableElements[collision.name].OnInteractionExit();
        _currentObject = null;
        InteractionText.enabled = false;
    }
}
