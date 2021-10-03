using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chrio.Entities;
using Chrio.Controls;
using static SharkUtils.ExtraFunctions;
using DG.Tweening;
using UnityEngine.UI;
using Chrio.World;
using Chrio.World.Loading;

public class Spaceship : BaseEntity
{
    public SpaceshipData Data;
    protected Vector3 MoveTarget = Vector3.zero;
    public float Speed = 10.0f;
    public float Accel;
    public Vector3 Velocity;
    public Image Renderer;

    public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
    {
        Renderer = GetComponent<Image>();
        EntityType = "Spaceship";
        base.OnLoad(_gameState, _callback);
    }

    public override void OnDamaged(DamageInfo HitInfo)
    {
        base.OnDamaged(HitInfo);

        // Show effects
    }

    void Update()
    {
        if (MoveTarget != Vector3.zero)
        {
            rectTransform.position = Vector3.SmoothDamp(rectTransform.position, MoveTarget, ref Velocity, Data.Acceleration, Data.MaxSpeed, Time.deltaTime);
            transform.up += ((MoveTarget - transform.position) / Data.MaxSpeed) * Time.deltaTime * 50;
        }
    }

    public override void OnSelected()
    {
        Renderer.DOColor(Color.blue, 0.2f); // Change color on selection!!!
    }

    public override void WhileSelected()
    {
        base.WhileSelected();
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        Renderer.DOColor(Color.white, 0.2f); // Change color on selection!!!
    }

    public override void OnCommand(Command CmdInfo)
    {
        base.OnCommand(CmdInfo);

        switch(CmdInfo.Type)
        {
            case CommandType.Move:
                MoveTarget = RandomPointInsideCircle(MoveTarget.Parse(CmdInfo.Data), 2);
                MoveTarget.z = 0;
                break;
        }
    }
}
