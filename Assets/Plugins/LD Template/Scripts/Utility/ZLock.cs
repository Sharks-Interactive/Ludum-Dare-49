using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Transform/Z Lock")]
public class ZLock : MonoBehaviour
{
    private Vector3 position;
    public int Level;

    void Update()
    {
        position = transform.position;
        position.z = Level;

        transform.position = position;
    }
}
