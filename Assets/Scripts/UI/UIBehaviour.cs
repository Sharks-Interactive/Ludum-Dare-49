using Chrio.World;
using Chrio.World.Loading;
using UnityEngine;

namespace Chrio
{
    public class UIBehaviour : SharksBehaviour
    {
        protected Canvas canvas;

        public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
        {
            canvas = GameObject.FindGameObjectWithTag("World").GetComponent<Canvas>();

            base.OnLoad(_gameState, _callback);
        }
    }
}
