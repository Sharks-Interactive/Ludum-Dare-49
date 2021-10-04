using Chrio.World;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spaceship", menuName = "Data/Spaceship")]
public class SpaceshipData : EntityData
{
    [Tooltip("Where speed will cap out during Accel")]
    public float MaxSpeed = 50;

    [Tooltip("How quickly we pick up speed")]
    public float Acceleration = 1;

    public WeaponInfo Weapon;

    public int ConstructionTime = 1;

    public float ConstructionCost = 100;
}
