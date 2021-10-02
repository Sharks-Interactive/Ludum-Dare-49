using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chrio.Entities;
using Chrio.Controls;
using static SharkUtils.ExtraFunctions;

public class Spaceship : BaseEntity
{
    protected Vector2 MoveTarget = Vector2.zero;
    public float Speed = 10.0f;
    public float Accel;

    public override void OnDamaged(DamageInfo HitInfo)
    {
        base.OnDamaged(HitInfo);

        // Show effects
    }

    void Update()
    {
        if (MoveTarget != Vector2.zero)
            rectTransform.position.Set(
                (MoveTarget.x - rectTransform.position.x) / Speed,
                (MoveTarget.y - rectTransform.position.y) / Speed,
                0
                );

        Debug.Log((MoveTarget.x - transform.position.x) / Speed);
        Debug.Log((MoveTarget.y - transform.position.y) / Speed);
    }

    public override void OnSelected()
    {
        Debug.Log("Selected!");
    }

    public override void OnCommand(Command CmdInfo)
    {
        base.OnCommand(CmdInfo);

        switch(CmdInfo.Type)
        {
            case CommandType.Move:
                MoveTarget = MoveTarget.Parse(CmdInfo.Data);
                break;
        }
    }
}
