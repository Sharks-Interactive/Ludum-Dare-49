using UnityEngine;
using Chrio.Entities;
using Chrio.Controls;
using static SharkUtils.ExtraFunctions;
using UnityEngine.UI;
using Chrio.World;
using Chrio.World.Loading;

public class Spaceship : BaseEntity
{
    public SpaceshipData Data;
    public TurretController Turret;
    protected Vector3 MoveTarget = Vector3.zero;
    
    public Vector3 Velocity;
    private IBaseEntity _attackTarget = null;

    private CircleCollider2D _collider;
    private Image _range;
    private CanvasGroup _rangeIdentifier;
    private TrailRenderer _trail;
    private Image _radarDot;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        EntityType = "Spaceship";
        _collider = GetComponent<CircleCollider2D>();
        _rangeIdentifier = GetComponentInChildren<CanvasGroup>();
        _range = transform.GetChild(1).GetComponent<Image>();
        _trail = GetComponentInChildren<TrailRenderer>();
        _radarDot = transform.Find("Radar Dot").GetComponent<Image>();

        _trail.colorGradient = Data.TeamColors[OwnerID];
        _radarDot.color = Data.TeamColors[OwnerID].colorKeys[0].color;

        _collider.radius = Data.Range;

        base.OnLoad(_gameState, _callback);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        _rangeIdentifier.alpha = 1.0f;
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        _rangeIdentifier.alpha = 0.0f;
    }

    public override void OnDamaged(DamageInfo HitInfo)
    {
        base.OnDamaged(HitInfo);

        MoveTarget = RandomPointInsideCircle(MoveTarget, 3); // Scramble!
        MoveTarget.z = 0;

        if (health <= 0)
        {
            GlobalState.Game.Entities.WorldEntities.Remove(gameObject);
            gameObject.SetActive(false);
        }
        // Show effects
    }

    void Update()
    {
        // Update selection healthbar
        _range.fillAmount = (health / Data.Health);

        _radarDot.enabled = !_radarDot.rectTransform.IsVisibleFrom(GlobalState.Game.MainCamera);

        Turret.Target = _attackTarget;
        if (MoveTarget != Vector3.zero)
        {
            rectTransform.position = Vector3.SmoothDamp(rectTransform.position, MoveTarget, ref Velocity, Data.Acceleration, Data.MaxSpeed, Time.deltaTime);
            transform.up += ((MoveTarget - transform.position) / Data.MaxSpeed) * Time.deltaTime * 50;
            rectTransform.eulerAngles = new Vector3(0, 0, rectTransform.eulerAngles.z);
        }
        if (_attackTarget == null) return;
        if (_attackTarget.GetOwnerID() == OwnerID) { _attackTarget = null; return; }// Owner could have changed
        if (_attackTarget.Health <= 0) _attackTarget = null; // Target could have died
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        IBaseEntity _tochingEntity;

        if (!collision.CompareTag("Entity")) return; // We're only interested in Entities
        if (!GlobalState.Game.Entities.WorldEntities.TryGetValue(collision.gameObject, out _tochingEntity)) return; // GlobalState is not aware of this entity
        if (_tochingEntity.GetOwnerID() == OwnerID) return; // The entity is not an enemy
        if (_attackTarget != null) return; // We are already attacking soemthing
        if (Vector2.Distance(collision.transform.position, transform.position) > Data.Range) return; // This enemy is out of range

        _attackTarget = _tochingEntity;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        IBaseEntity _tochingEntity;

        if (_attackTarget != null) return; // We are already attacking soemthing
        if (!collision.CompareTag("Entity")) return; // We're only interested in Entities
        if (!GlobalState.Game.Entities.WorldEntities.TryGetValue(collision.gameObject, out _tochingEntity)) return; // GlobalState is not aware of this entity
        if (_tochingEntity.GetOwnerID() == OwnerID) return; // The entity is not an enemy

        _attackTarget = _tochingEntity;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        IBaseEntity _tochingEntity;

        if (!collision.CompareTag("Entity")) return; // We're only interested in Entities
        if (!GlobalState.Game.Entities.WorldEntities.TryGetValue(collision.gameObject, out _tochingEntity)) return; // GlobalState is not aware of this entity
        if (_tochingEntity.GetOwnerID() == OwnerID) return; // The entity is not an enemy
        if (_attackTarget != _tochingEntity) return; // We are not attacking this ent

        _attackTarget = null; // The ent is out of range
    }

    public override void OnCommand(Command CmdInfo)
    {
        if (CmdInfo.Issuer != OwnerID) return;
        base.OnCommand(CmdInfo);

        switch(CmdInfo.Type)
        {
            case CommandType.Move:
                MoveTarget = MoveTarget.Parse(CmdInfo.Data);
                MoveTarget.z = 0;
                break;
        }
    }
}
