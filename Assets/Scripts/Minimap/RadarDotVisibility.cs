using Chrio;
using Chrio.Entities;
using Chrio.World;
using Chrio.World.Loading;
using SharkUtils;
using UnityEngine;
using UnityEngine.UI;

public class RadarDotVisibility : SharksBehaviour
{
    private Image _radarDot;
    private BaseEntity _parentEntity;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        _parentEntity = GetComponentInParent<BaseEntity>();
        _radarDot = GetComponent<Image>();
        base.OnLoad(_gameState, _callback);
    }

    void Update() { _radarDot.enabled = !_radarDot.rectTransform.IsVisibleFrom(GlobalState.Game.MainCamera); if (_radarDot.color == Color.black) return; _radarDot.color = (_parentEntity.OwnerID == 2 ? Color.white : _parentEntity.EntityData.TeamColors[_parentEntity.OwnerID].colorKeys[0].color); }
}
