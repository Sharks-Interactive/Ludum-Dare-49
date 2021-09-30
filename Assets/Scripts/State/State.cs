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

            public Game()
            {
                Entities = new Entities();
                Running = true;
            }
        }

        [Serializable]
        public class Entities
        {
            public Dictionary<GameObject, IBaseEntity> WorldEntities;

            public Entities()
            {
                WorldEntities = new Dictionary<GameObject, IBaseEntity>();
            }
        }

        
        public class State
        {
            public Mouse Cursor;
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