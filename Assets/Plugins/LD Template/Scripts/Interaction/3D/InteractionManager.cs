using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask LayerMask;

    [Header("Interactables")]
    //Uses a dictionary instead of GetComponents for better performance
    public Dictionary<string, IinteractableObject> Interactables = new Dictionary<string, IinteractableObject>();

    //Cache
    private RaycastHit _lastHit;
    private IinteractableObject _Iobject;

    // Start is called before the first frame update
    void Start()
    {
        GetInteractables(); //Or call this in a loading function
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit t_hit;
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out t_hit, Mathf.Infinity, LayerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * t_hit.distance, Color.yellow);
            Debug.Log(t_hit.collider.name);

            if (_lastHit.collider == t_hit.collider)
            {
                if (Interactables.TryGetValue(t_hit.collider.name, out _Iobject))
                {
                    _Iobject.WhileHighlighted();
                    return;
                }
            }
            else
            {
                if (_lastHit.collider == null)
                    return; //No objects have been highlighted yet

                //Disable the last object - no need to enable the new one here that will happen later down the line already
                if (Interactables.TryGetValue(_lastHit.collider.name, out _Iobject))
                {
                    _Iobject.OnHighlightEnd();
                    _Iobject._highlighted = false;
                }

                _lastHit = t_hit; //Set them equal
            }
            
            if (Interactables.TryGetValue(t_hit.collider.name, out _Iobject))
            {
                //Call neccecary variables on the object
                _Iobject._highlighted = true;
                _Iobject.OnHighlighted();
            }
        }
        else
        {
            //Disable the previously highlighted object
            if (_lastHit.collider == null)
                return; //No objects have been highlighted yet

            if (Interactables.TryGetValue(_lastHit.collider.name, out _Iobject))
            {
                _Iobject.OnHighlightEnd();
                _Iobject._highlighted = false;
            }
        }
    }

    /// <summary>
    /// Gets all Objects in the scene that are tagged as Interactable and caches them
    /// </summary>
    public void GetInteractables ()
    {
        GameObject[] _interactableObjects = GameObject.FindGameObjectsWithTag("Interactable");

        for (int i = 0; i < _interactableObjects.Length; i++)
            Interactables.Add(_interactableObjects[i].name, _interactableObjects[i].GetComponent<IinteractableObject>());
    }
}
