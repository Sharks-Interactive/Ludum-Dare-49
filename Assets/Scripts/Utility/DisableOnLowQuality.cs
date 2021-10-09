using Chrio;
using Chrio.World;
using Chrio.World.Loading;
using UnityEngine;
using UnityEngine.Assertions;

public class DisableOnLowQuality : SharksBehaviour
{
    public MonoBehaviour ObjectToDisable;
    public GameObject[] Objects;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        base.OnLoad(_gameState, _callback);
        if (ObjectToDisable != null)
        ObjectToDisable.enabled = !_gameState.LowQuality;
        foreach (GameObject Object in Objects)
            Object.SetActive(!_gameState.LowQuality);
    }
}
