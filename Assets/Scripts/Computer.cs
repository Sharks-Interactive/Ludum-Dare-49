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

        private int _drydocksOwned = 0;

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
            }

            base.OnLoad(_gameState, _callback);
        }

        // Update is called once per frame
        void Update()
        {
            // AI has a few different targets. The first and foremost is drydocks.
            // The ai prioritises drydocks if they are lost
            // Otherwise it will go after the nearest enemy ship
            _drydocksOwned = 0;
            for (int g = 0; g < Drydocks.Count; g++)
                if (Drydocks[g].GetOwnerID() == 1)
                    _drydocksOwned++;

            if (_drydocksOwned != Drydocks.Count) { }
            // If there is an imbalance this is our new goal

            // This is really pretty bad
            // Othwerwise just attack the nearest enemy
            foreach (IBaseEntity ship in Ships.Values)
            {
                float[] Distances = new float[PlayerShips.Count];
                for (int c = 0; c< PlayerShips.Count; c++/*Ha*/)
                    Distances[c] = Vector2.Distance(ship.GetGameObject().transform.position, PlayerShips[c].GetGameObject().transform.position);

                if (PlayerShips.Count == 0) return;
                ship.OnCommand(new Controls.Command(
                    Controls.CommandType.Move,
                    PlayerShips[Distances.IndexOfGreatest()].GetGameObject().transform.position.ToString()
                    ));
            }
        } 
    }
}
