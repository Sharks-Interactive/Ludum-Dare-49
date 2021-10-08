using UnityEngine.UI;
using UnityEngine;
using Chrio.Controls;
using Chrio.World;
using Chrio.World.Loading;
using DG.Tweening;

namespace Chrio.Entities
{
    public class BaseEntity : SharksBehaviour, IBaseEntity
    {
        public float Health { get => health; set => health = value; }

        public int OwnerID;

        public EntityData EntityData;

        protected string EntityType = "BaseEntity";

        protected float health = 0;

        protected RectTransform rectTransform;

        protected Image imageRenderer;

        public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
        {
            imageRenderer = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            health = EntityData.Health;
            imageRenderer.sprite = EntityData.EntitySprite;

            base.OnLoad(_gameState, _callback);
        }

        public virtual void OnDamaged(DamageInfo HitInfo) => health -= HitInfo.Amount;

        public virtual void OnSelected() 
        {
            if (OwnerID == 1) return;
            imageRenderer.DOColor(Color.cyan, 0.2f); // Change color on selection!!!
        }

        public virtual void WhileSelected() { }

        public virtual void OnDeselected() 
        {
            if (OwnerID == 1) return;
            imageRenderer.DOColor(Color.white, 0.2f); // Change color on selection!!!
        }

        public virtual void OnCommand(Command CmdInfo) { }
        public virtual BaseEntity GetEntity() => this;
        public virtual EntityData GetData() => EntityData;
        public virtual GameObject GetGameObject() => gameObject;
        public virtual int GetOwnerID() => OwnerID;
        public virtual string GetEntityType() => EntityType;
        public virtual bool CompareEntityType(string EntType) => EntityType == EntType;
    }
}
