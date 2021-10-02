using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chrio.Entities;
using Chrio.Controls;
using static SharkUtils.ExtraFunctions;

public class Spaceship : BaseEntity
{
    protected Vector3 MoveTarget = new Vector3(0, 0, 0);

    public override void OnDamaged(DamageInfo HitInfo)
    {
        base.OnDamaged(HitInfo);

        // Show effects
    }

    void Update()
    {
        transform.position = MoveTarget;
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
                MoveTarget.z = 0; // Reset Z
                break;
        }
    }
}
