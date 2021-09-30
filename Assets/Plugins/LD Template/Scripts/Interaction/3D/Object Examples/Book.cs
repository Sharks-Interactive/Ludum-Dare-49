using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour, IinteractableObject
{
    public bool _highlighted { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHighlighted()
    {
        Debug.Log("woopie");
    }

    public void WhileHighlighted()
    {
        Debug.Log("Yaayyy");
    }

    public void OnHighlightEnd()
    {
        Debug.Log("Aww");
    }
}
