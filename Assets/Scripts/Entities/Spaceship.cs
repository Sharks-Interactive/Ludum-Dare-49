using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chrio.Entities;
using Chrio.Controls;
using static SharkUtils.ExtraFunctions;
using DG.Tweening;

public class Spaceship : BaseEntity
{
    public SpaceshipData Data;
    protected Vector3 MoveTarget = Vector3.zero;
    public float Speed = 10.0f;
    public float Accel;
    public Vector3 Velocity;

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
        transform.DOScale(1.1f, 0.2f); // Change color on selection!!!
    }

    public override void WhileSelected()
    {
        base.WhileSelected();
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        transform.DOScale(1, 0.2f);
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
