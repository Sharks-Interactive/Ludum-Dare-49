using Chrio;
using Chrio.World;
using Chrio.World.Loading;
using UnityEngine;
using UnityEngine.UI;
using static SharkUtils.ExtraFunctions;

public class MinimapManager : UIBehaviour
{
    private RawImage _renderer;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        _renderer = GetComponent<RawImage>();
        base.OnLoad(_gameState, _callback);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            _renderer.transform.localScale = _renderer.transform.localScale.UpdateAxisEuler(0.63f, Axis.X);
            _renderer.transform.localScale = _renderer.transform.localScale.UpdateAxisEuler(0.63f, Axis.Y);
        }
        else
        {
            _renderer.transform.localScale = _renderer.transform.localScale.UpdateAxisEuler(0.2f, Axis.X);
            _renderer.transform.localScale = _renderer.transform.localScale.UpdateAxisEuler(0.2f, Axis.Y);
        }
    }
}
