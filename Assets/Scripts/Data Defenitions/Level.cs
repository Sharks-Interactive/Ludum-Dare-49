using System;
using System.IO;
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

    private const string FILENAME = "Level1.dat";

    public void SaveToFile()
    {
        var filePath = Path.Combine(Application.persistentDataPath, FILENAME);

        if (!File.Exists(filePath))
            File.Create(filePath);

        var json = JsonUtility.ToJson(this);
        File.WriteAllText(filePath, json);
    }

    public void LoadDataFromFile()
    {
        var filePath = Path.Combine(Application.persistentDataPath, FILENAME);

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"File \"{filePath}\" not found!", this);
            return;
        }

        var json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(json, this);
    }
}
