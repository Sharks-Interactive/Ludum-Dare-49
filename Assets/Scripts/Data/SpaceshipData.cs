using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spaceship", menuName = "Data/Spaceship")]
public class SpaceshipData : ScriptableObject
{
    [Tooltip("Starting health")]
    public int Health = 5000;

    [Tooltip("Where speed will cap out during Accel")]
    public float MaxSpeed = 50;

    [Tooltip("How quickly we pick up speed")]
    public float Acceleration = 1;
}
