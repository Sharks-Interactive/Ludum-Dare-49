using Chrio.Entities;
using UnityEngine;
using static SharkUtils.ExtraFunctions;

namespace Chrio.World
{
    public class LevelController : SharksBehaviour
    {
        public Level Data;

        public override void OnLoad(Game_State.State _gameState, Loading.ILoadableObject.CallBack _callback)
        {
            GameObject WorldPlace = GameObject.FindGameObjectWithTag("World");

            Data.SaveToFile();
            Data.LoadDataFromFile();

            Transform EnemyStartingPoint = GameObject.Find(Data.Enemy.StartingPlace).transform;
            Transform PlayerStartingPoint = GameObject.Find(Data.Player.StartingPlace).transform;
            Vector3 EnemyStartingPlace = EnemyStartingPoint.position;
            Vector3 PlayerStartingPlace = PlayerStartingPoint.position;

            Vector3 World = WorldPlace.transform.position;

            // Spawn starting ships
            for (int i = 0; i < Data.Enemy.StartingShips; i++)
            {
                EnemyStartingPoint.position = RandomPointInsideCircle(EnemyStartingPlace, 10).UpdateAxisEuler(0, Axis.Z); // Randomize location
                Drydock.SpawnShip(_gameState, EnemyStartingPoint, Data.Enemy.StartingShipType, 1);
            }

            for (int i = 0; i < Data.Player.StartingShips; i++)
            {
                PlayerStartingPoint.position = RandomPointInsideCircle(PlayerStartingPlace, 10).UpdateAxisEuler(0, Axis.Z); // Randomize location
                Drydock.SpawnShip(_gameState, PlayerStartingPoint, Data.Player.StartingShipType, 0);
            }

            // Spawn starting asteroids
            for (int i = 0; i < Data.Asteroids; i++)
            {
                EntityData Asteroid = Data.AsteroidData.Random();

                World = RandomPointInsideCircle(WorldPlace.transform.position, Data.AsteroidRange).UpdateAxisEuler(0, Axis.Z);
                GameObject NewObject = (GameObject)Instantiate(Resources.Load("Asteroid"), World, WorldPlace.transform.rotation, WorldPlace.transform);
                NewObject.GetComponent<Asteroid>().AsteroidWorth = (int)RandomFromRange(Data.AsteroidsWorth * ((Asteroid.Health / 5000) + 1.0f));
                Game_State.Entities.InitEntity<Asteroid>(NewObject, _gameState, Asteroid, 2);
            }

            base.OnLoad(_gameState, _callback);
        }
    }
}
