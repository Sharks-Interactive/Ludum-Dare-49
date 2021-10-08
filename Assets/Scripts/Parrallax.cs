using Chrio;
using Chrio.World;
using Chrio.World.Loading;
using SharkUtils;
using UnityEngine;

public class Parrallax : SharksBehaviour
{
    public float ParrallaxFactor;

    public Vector3 Offset;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        base.OnLoad(_gameState, _callback);
        enabled = !_gameState.LowQuality;
    }

    void Update()
    {
        transform.position = (((GlobalState.Game.MainCamera.transform.position * ParrallaxFactor).UpdateAxisEuler(0, ExtraFunctions.Axis.Z)) + Offset);
    }

    private void OnDrawGizmosSelected()
    {
        transform.position = Offset;
    }
}
