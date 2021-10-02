using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Chrio.Entities;
using System.Runtime.CompilerServices;

namespace Chrio.World
{
    public static class Game_State
    {
        //public static State GlobalState = new State();

        [Serializable]
        public class Mouse
        {
            public bool Locked;
            public float Sensitivity;

            public Mouse()
            {
                Locked = true;
                Sensitivity = 1.5f;
            }
        }

        [Serializable]
        public class Game
        {
            public bool Running;
            public Entities Entities;
            public Camera MainCamera;

            public Game()
            {
                Entities = new Entities();
                Running = true;
                MainCamera = Camera.main;
            }
        }

        [Serializable]
        public class Entities
        {
            private Dictionary<int, IBaseEntity> EntityIDs;
            public Dictionary<GameObject, IBaseEntity> WorldEntities;

            public Entities()
            {
                WorldEntities = new Dictionary<GameObject, IBaseEntity>();
                EntityIDs = new Dictionary<int, IBaseEntity>();

                GameObject[] _allEnts = GameObject.FindGameObjectsWithTag("Entity");

                // For getting currently existing entities at the time of the game starting
                for (int i = 0; i < _allEnts.Length; i++)
                {
                    WorldEntities.Add(_allEnts[i], _allEnts[i].GetComponent<IBaseEntity>());
                    EntityIDs.Add(_allEnts[i].GetInstanceID(), WorldEntities[_allEnts[i]]);
                }                
            }

            public IBaseEntity GetEntityByID (int ID) => EntityIDs.ContainsKey(ID) ? EntityIDs[ID] : null;
        }

        
        public class State
        {
            public Mouse Cursor = new Mouse();
            public Game Game;
            public int LocalPlayerNum;

            public State()
            {
                Cursor = new Mouse();
                Game = new Game();
                LocalPlayerNum = 0;
            }
        }
    }
}