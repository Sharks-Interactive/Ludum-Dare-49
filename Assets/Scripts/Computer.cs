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
        public List<Drydock> Drydocks = new List<Drydock>();

        public List<IBaseEntity> PlayerShips = new List<IBaseEntity>();

        public List<IBaseEntity> Asteroids = new List<IBaseEntity>();

        public SpaceshipData FighterDef;
        public SpaceshipData FrigateDef;
        public SpaceshipData WarshipDef;

        public List<Spaceship> EnemyFighters;
        public List<Spaceship> EnemyFrigates;
        public List<Spaceship> EnemyWarships;

        public List<Spaceship> MyFighters;
        public List<Spaceship> MyFrigates;
        public List<Spaceship> MyWarships;

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
                        Drydocks.Add(ent as Drydock);
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

            EnemyFighters = new List<Spaceship>();
            EnemyFrigates = new List<Spaceship>();
            EnemyWarships = new List<Spaceship>();

            MyFighters = new List<Spaceship>();
            MyFrigates = new List<Spaceship>();
            MyWarships = new List<Spaceship>();

            #region Data Collection



            Ships = new Dictionary<GameObject, IBaseEntity>();
            Drydocks = new List<Drydock>();

            PlayerShips = new List<IBaseEntity>();

            Asteroids = new List<IBaseEntity>();

            foreach (IBaseEntity ent in GlobalState.Game.Entities.WorldEntities.Values)
            {
                if (ent.GetOwnerID() == 1)
                {
                    if (ent.CompareEntityType("Spaceship"))
                    {
                        Ships.Add(ent.GetGameObject(), ent);
                        switch (ent.GetData().DisplayName)
                        {
                            case "Fighter":
                                MyFighters.Add(ent as Spaceship);
                                break;

                            case "Frigate":
                                MyFrigates.Add(ent as Spaceship);
                                break;

                            case "Warship":
                                MyWarships.Add(ent as Spaceship);
                                break;
                        }
                    }
                    else if (ent.CompareEntityType("Drydock"))
                        Drydocks.Add(ent as Drydock);
                }
                else if (ent.CompareEntityType("Spaceship"))
                {
                    PlayerShips.Add(ent);
                    switch (ent.GetData().DisplayName)
                    {
                        case "Fighter":
                            EnemyFighters.Add(ent as Spaceship);
                            break;

                        case "Frigate":
                            EnemyFrigates.Add(ent as Spaceship);
                            break;

                        case "Warship":
                            EnemyWarships.Add(ent as Spaceship);
                            break;
                    }
                }
                else if (ent.CompareEntityType("Asteroid"))
                    Asteroids.Add(ent);
                else if (ent.CompareEntityType("Drydock"))
                    Drydocks.Add(ent as Drydock);
            }

            // AI has a few different targets. The first and foremost is drydocks.
            // The ai priorities asteroids if it does not have enough money
            // The ai then prioritises drydocks if they are lost
            // Otherwise it will go after the nearest enemy ship
            List<Drydock> _myDrydocks = new List<Drydock>();
            List<IBaseEntity> _playerDrydocks = new List<IBaseEntity>();
            List<IBaseEntity> _neutralDrydocks = new List<IBaseEntity>();
            for (int g = 0; g < Drydocks.Count; g++)
                if (Drydocks[g].GetOwnerID() == 1)
                    _myDrydocks.Add(Drydocks[g]);
                else if (Drydocks[g].GetOwnerID() == 0)
                    _playerDrydocks.Add(Drydocks[g]);
                else
                    _neutralDrydocks.Add(Drydocks[g]);

            if (_myDrydocks.Count == 0) // We NEED drydocks
            {
                if (_neutralDrydocks.Count > _playerDrydocks.Count)
                    SendShipsToNearestLeastPopulatedInList(_neutralDrydocks);
                else
                    SendShipsToNearestLeastPopulatedInList(_playerDrydocks);
                return;
            }

            int BuildingFighters = 0; // Num of drydocks already building fighters
            foreach (Drydock dock in _myDrydocks)
                if (dock.Constructing != null) 
                    if (dock.Constructing.DisplayName == "Fighter") BuildingFighters++;

            // We always want a 50/50 ratio of warships and frigates. Fighters can be used for swarm killing enemy warships
            SpaceshipData[] shipsToBuild = new SpaceshipData[_myDrydocks.Count];

            for (int s = 0; s < shipsToBuild.Length; s++)
            {
                // If their are lone enemy warships and we haven't alread/don't already have enough fighters to kill a warship and we are low on money
                if (EnemyWarships.Count < 2 && s - MyWarships.Count - BuildingFighters < (WarshipDef.Health / FighterDef.Weapon.Damage) + 1 && GlobalState.Game.Money[1] < 1000) { shipsToBuild[s] = FighterDef; continue; }

                // If the enemy has signifigant military value and we have enough money just do a 50 50 of warships and frigates
                if (EnemyWarships.Count + EnemyFrigates.Count > 3 && GlobalState.Game.Money[1] > 1250) { shipsToBuild[s] = (s % 2 == 0 ? FrigateDef : WarshipDef); continue; }

                // If the enemy has more fighters then us and none of the above conditions are met provided (we aren't close to having enough money to do other stuff || we have no ships :( )
                if (EnemyFighters.Count > MyFighters.Count && (GlobalState.Game.Money[1] < 800 || MyFighters.Count + MyFrigates.Count + MyWarships.Count < 4)) { shipsToBuild[s] = FighterDef; continue; }

                // Otherwise we are not going to build anything and are just going to save up
                shipsToBuild[s] = null;
            }
            

            for (int i = 0; i < _myDrydocks.Count; i++)
                if (shipsToBuild[i] != null) _myDrydocks[i].BuildShip(shipsToBuild[i], 1); // Make sure every drydock is building

            // Check if we have enough money for all of our plans
            float BuildPlanCost = 0;
            for (int j = 0; j < shipsToBuild.Length; j++)
                if (shipsToBuild[j] != null) BuildPlanCost += shipsToBuild[j].ConstructionCost;

            // If we don't have enough for our plans or we don't have much money to start off with and we (have enough drydocks and have a decent amount of ships) go after asteroids
            if (BuildPlanCost >= GlobalState.Game.Money[1] || GlobalState.Game.Money[1] < 1300 && (_myDrydocks.Count > 2 || MyFighters.Count + MyFrigates.Count + MyWarships.Count > 4))
            {
                // We don't have enough money, send all ships to the nearest asteroids
                SendShipsToNearestLeastPopulatedInList(Asteroids);
                return;
            }

            // If there are drydocks that we don't own let's go there
            if (_myDrydocks.Count != Drydocks.Count) 
            {
                if (_neutralDrydocks.Count > _playerDrydocks.Count)
                    SendShipsToNearestLeastPopulatedInList(_neutralDrydocks);
                else
                    SendShipsToNearestLeastPopulatedInList(_playerDrydocks);
                return;
            }
            // If there is an imbalance this is our new goal

            // This is really pretty bad
            // Othwerwise just attack the nearest enemy
            SendShipsToNearestLeastPopulatedInList(PlayerShips);
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
                    (Entities[Distances.IndexOfLeast()].GetGameObject().transform.position + transform.right * 2).ToString(), // INSTRUCT THE SHIP TO BROADSIDE
                    1
                    ));
            }
        }

        private void SendShipsToNearestLeastPopulatedInList(List<IBaseEntity> Entities)
        {
            foreach (IBaseEntity ship in Ships.Values)
            {
                float[] Distances = new float[Entities.Count];
                for (int c = 0; c < Entities.Count; c++/*Ha*/)
                    Distances[c] = (NumberOfShipsNearby(Entities[c], 15) > 2 ? Vector2.Distance(ship.GetGameObject().transform.position, Entities[c].GetGameObject().transform.position) * NumberOfShipsNearby(Entities[c], 15) : Vector2.Distance(ship.GetGameObject().transform.position, Entities[c].GetGameObject().transform.position));
                // ^ Make populated entities less attractive

                if (Entities.Count == 0) return;
                ship.OnCommand(new Controls.Command(
                    Controls.CommandType.Move,
                    (Entities[Distances.IndexOfLeast()].GetGameObject().transform.position + transform.right * 2).ToString(), // INSTRUCT THE SHIP TO BROADSIDE
                    1
                    ));
            }
        }

        private int NumberOfShipsNearby(IBaseEntity t_ent, float Dist = 10)
        {
            int NumberOfShips = 0;
            foreach (IBaseEntity ent in PlayerShips)
                if (IsInsideCircle(ent.GetGameObject().transform.position, t_ent.GetGameObject().transform.position, Dist)) NumberOfShips++;

            return NumberOfShips;
        }
    }
}
