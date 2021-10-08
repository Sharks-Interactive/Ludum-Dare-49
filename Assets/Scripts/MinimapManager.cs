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
            _renderer.color = _renderer.color.UpdateColor(1.0f, Axis.W);
            _renderer.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 264);
            _renderer.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
        }
        else
        {
            _renderer.color = _renderer.color.UpdateColor(0.7f, Axis.W);
            _renderer.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 178.7f);
            _renderer.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 113.8f);
        }
    }
}
