using UnityEngine;
using static Chrio.World.Game_State;
using Chrio.World.Loading;

namespace Chrio
{
    public class SharksBehaviour : MonoBehaviour, ILoadableObject
    {
        protected State GlobalState;

        public virtual void OnLoad(State _gameState, ILoadableObject.CallBack _callback)
        {
            GlobalState = _gameState;
            _callback();
        }
    }
}
