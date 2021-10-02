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

        protected RectTransform rectTransform;

        void Start ()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public virtual void OnDamaged(DamageInfo HitInfo) => health += HitInfo.Amount;

        public virtual void OnSelected() { }

        public virtual void WhileSelected() { }

        public virtual void OnDeselected() { }

        public virtual void OnCommand(Command CmdInfo) { }
        public virtual BaseEntity GetEntity() => this;
        public virtual GameObject GetGameObject() => gameObject;
    }
}
