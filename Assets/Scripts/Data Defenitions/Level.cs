using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Data/Level")]
public class Level : ScriptableObject
{
    [Header("World")]
    public int Asteroids;
    public EntityData[] AsteroidData;
    public Vector2 AsteroidsWorth;
    public int AsteroidRange;

    [Header("Computer")]
    public PlayerData Enemy;

    [Header("Player")]
    public PlayerData Player;

    [Serializable]
    public class PlayerData
    {
        public int StartingShips;
        public string StartingPlace;
        public SpaceshipData StartingShipType;
    }
}
