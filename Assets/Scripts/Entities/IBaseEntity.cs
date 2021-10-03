using UnityEngine;

namespace Chrio.Entities
{
    public interface IBaseEntity
    {
        public float Health { get; set; }

        public void OnDamaged(DamageInfo HitInfo);

        public void OnSelected();

        public void WhileSelected();

        public void OnDeselected();

        public void OnCommand(Controls.Command CmdInfo);

        public BaseEntity GetEntity();

        public GameObject GetGameObject();

        public int GetOwnerID();

        public string GetEntityType();

        public bool CompareEntityType(string EntType);
    }
}
