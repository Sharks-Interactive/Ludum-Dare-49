using Chrio.World;
using Chrio.World.Loading;
using static SharkUtils.ExtraFunctions;
using System.Collections.Generic;
using UnityEngine;

namespace Chrio.Entities
{
    public class Computer : SharksBehaviour
    {
        public Dictionary<GameObject, IBaseEntity> Ships = new Dictionary<GameObject, IBaseEntity>();
        public List<IBaseEntity> Drydocks = new List<IBaseEntity>();

        public List<IBaseEntity> PlayerShips = new List<IBaseEntity>();

        public List<IBaseEntity> Asteroids = new List<IBaseEntity>();

        public SpaceshipData BuildableShipsDef;

        public int StepSpeed = 2;
        public float TimeAtTime;

        public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
        {
            GameObject[] _allEnts = GameObject.FindGameObjectsWithTag("Entity");

            // For getting currently existing entities at the time of the game starting
            for (int i = 0; i < _allEnts.Length; i++)
            {
                IBaseEntity ent = _allEnts[i].GetComponent<IBaseEntity>();
                if (ent.GetOwnerID() == 1)
                {
                    if (ent.CompareEntityType("Spaceship"))
                        Ships.Add(_allEnts[i], ent);
                    else if (ent.CompareEntityType("Drydock"))
                        Drydocks.Add(ent);
                }
                else if (ent.CompareEntityType("Spaceship"))
                    PlayerShips.Add(ent);
                else if (ent.CompareEntityType("Asteroid"))
                    Asteroids.Add(ent);
            }

            base.OnLoad(_gameState, _callback);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (Time.time < TimeAtTime) return;
            TimeAtTime = Time.time + StepSpeed;

            #region Data Collection



            Ships = new Dictionary<GameObject, IBaseEntity>();
            Drydocks = new List<IBaseEntity>();

            PlayerShips = new List<IBaseEntity>();

            Asteroids = new List<IBaseEntity>();

            foreach (IBaseEntity ent in GlobalState.Game.Entities.WorldEntities.Values)
            {
                if (ent.GetOwnerID() == 1)
                {
                    if (ent.CompareEntityType("Spaceship"))
                        Ships.Add(ent.GetGameObject(), ent);
                    else if (ent.CompareEntityType("Drydock"))
                        Drydocks.Add(ent);
                }
                else if (ent.CompareEntityType("Spaceship"))
                    PlayerShips.Add(ent);
                else if (ent.CompareEntityType("Asteroid"))
                    Asteroids.Add(ent);
            }

            // AI has a few different targets. The first and foremost is drydocks.
            // The ai priorities asteroids if it does not have enough money
            // The ai then prioritises drydocks if they are lost
            // Otherwise it will go after the nearest enemy ship
            List<Drydock> _myDrydocks = new List<Drydock>();
            List<IBaseEntity> _playerDrydocks = new List<IBaseEntity>();
            for (int g = 0; g < Drydocks.Count; g++)
                if (Drydocks[g].GetOwnerID() == 1)
                    _myDrydocks.Add(Drydocks[g] as Drydock);
                else
                    _playerDrydocks.Add(Drydocks[g]);

            for (int i = 0; i < _myDrydocks.Count; i++)
                _myDrydocks[i].BuildShip(BuildableShipsDef, 1); // Make sure every drydock is building

            // Check if we have enough money
            if (BuildableShipsDef.ConstructionCost * _myDrydocks.Count >= GlobalState.Game.Money[1])
            {
                // We don't have enough money, send all ships to the nearest asteroids
                SendShipsToNearestInList(Asteroids);
                return;
            }

            if (_myDrydocks.Count != Drydocks.Count) 
            {
                SendShipsToNearestInList(_playerDrydocks);
                return;
            }
            // If there is an imbalance this is our new goal

            // This is really pretty bad
            // Othwerwise just attack the nearest enemy
            SendShipsToNearestInList(PlayerShips);
            #endregion
        } 

        private void SendShipsToNearestInList(List<IBaseEntity> Entities)
        {
            foreach (IBaseEntity ship in Ships.Values)
            {
                float[] Distances = new float[Entities.Count];
                for (int c = 0; c < Entities.Count; c++/*Ha*/)
                    Distances[c] = Vector2.Distance(ship.GetGameObject().transform.position, Entities[c].GetGameObject().transform.position);

                if (Entities.Count == 0) return;
                ship.OnCommand(new Controls.Command(
                    Controls.CommandType.Move,
                    (Entities[Distances.IndexOfLeast()].GetGameObject().transform.position + transform.right * 2).ToString(), // iNSTRUCT THE SHIP TO BROADSIDE
                    1
                    ));
            }
        }
    }
}
