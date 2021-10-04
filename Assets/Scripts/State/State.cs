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
            public Vector2 Money = new Vector2(0, 0);

            public Game()
            {
                Entities = new Entities();
                Running = true;
                MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }
        }

        [Serializable]
        public class Entities
        {
            private Dictionary<int, IBaseEntity> EntityIDs;
            public Dictionary<GameObject, IBaseEntity> WorldEntities;
            public List<IBaseEntity> Selected;

            public Entities()
            {
                Selected = new List<IBaseEntity>();
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
            
            /// <summary>
            /// Adds an entity to the global state
            /// </summary>
            /// <param name="ID"> ID Of the entity to add. </param>
            /// <param name="EntObject"> The gameobejct of the entity to add. </param>
            /// <param name="Ent"> A reference to the entities entity </param>
            public void AddEntity(int ID, GameObject EntObject, IBaseEntity Ent)
            {
                WorldEntities.Add(EntObject, Ent);
                EntityIDs.Add(EntObject.GetInstanceID(), Ent);
            }

            public static void InitEntity<T>(GameObject NewEntity, State GlobalState, EntityData Data, int TeamID) where T : Asteroid
            {
                IBaseEntity Entity = NewEntity.GetComponent<IBaseEntity>();
                T EntBehaviour = Entity.GetEntity() as T;

                EntBehaviour.OwnerID = TeamID;
                EntBehaviour.EntityData = Data;

                EntBehaviour.OnLoad(GlobalState, Drydock.Dummy); // Init ship

                GlobalState.Game.Entities.AddEntity(NewEntity.GetInstanceID(), NewEntity, Entity.GetEntity());
            }
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