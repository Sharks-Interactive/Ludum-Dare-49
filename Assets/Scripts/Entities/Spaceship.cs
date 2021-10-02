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
        // y=ax^{2}+bx+c
        if (MoveTarget != Vector3.zero)
            rectTransform.position = Vector3.SmoothDamp(rectTransform.position, MoveTarget, ref Velocity, Data.Acceleration, Data.MaxSpeed, Time.deltaTime);

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
                MoveTarget.z = 0;
                break;
        }
    }
}
