using Chrio;
using Chrio.World;
using Chrio.World.Loading;
using UnityEngine;

public class DisableOnLowQuality : SharksBehaviour
{
    public MonoBehaviour ObjectToDisable;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        base.OnLoad(_gameState, _callback);
        ObjectToDisable.enabled = !_gameState.LowQuality;
    }
}
