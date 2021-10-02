using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chrio.Controls;

namespace Chrio.Entities
{
    public class BaseEntity : SharksBehaviour, IBaseEntity
    {
        public float Health { get => health; set => health = value; }

        protected float health = 0;

        public virtual void OnDamaged(DamageInfo HitInfo) => health += HitInfo.Amount;

        public virtual void OnSelected() { }

        public virtual void WhileSelected() { }

        public virtual void OnDeseleceted() { }

        public virtual void OnCommand(Command CmdInfo) { }
        public virtual BaseEntity GetEntity() => this;
    }
}
