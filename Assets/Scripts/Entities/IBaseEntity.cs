namespace Chrio.Entities
{
    public interface IBaseEntity
    {
        public float Health { get; set; }

        public void OnDamaged(DamageInfo HitInfo);

        public void OnSelected();

        public void WhileSelected();

        public void OnDeseleceted();

        public void OnCommand(Controls.Command CmdInfo);

        public BaseEntity GetEntity();
    }
}
