using UnityEngine;
using static SharkUtils.ExtraFunctions;
using System.Collections;
using Chrio.World;
using Chrio.World.Loading;

namespace Chrio.Entities
{
    public class Drydock : BaseEntity
    {
        public EntityData Constructing;

        public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
        {
            EntityType = "Drydock";
            base.OnLoad(_gameState, _callback);
        }

        public override void OnDamaged(DamageInfo HitInfo)
        {
            base.OnDamaged(HitInfo);

            if (Health <= 0)
            {
                OwnerID = HitInfo.Attacker.GetOwnerID(); // Swap owners
                health = EntityData.Health * 5; // Reset health
                if (OwnerID < 2) imageRenderer.color = (OwnerID != 0 ? EntityData.TeamColors[OwnerID].colorKeys[0].color : Color.white);
            }
        }

        public static void SpawnShip(Game_State.State GState, Transform SpawnLocation, SpaceshipData Data, int TeamID)
        {
            GameObject _newShip = (GameObject)Instantiate(Resources.Load("Spaceship"), SpawnLocation.position, SpawnLocation.rotation, SpawnLocation.parent);
            _newShip.transform.eulerAngles = Vector3.zero;
            IBaseEntity _shipEnt = _newShip.GetComponent<IBaseEntity>();
            Spaceship ship = _shipEnt as Spaceship;

            _shipEnt.GetEntity().OwnerID = TeamID;
            ship.Data = Data;
            ship.EntityData = Data;
            ship.Turret.Info = Data.Weapon;

            ship.OnLoad(GState, Dummy); // Init ship
            ship.Turret.OnLoad(GState, Dummy); // Init ship turret

            /*_shipEnt.OnCommand( // Give the newly created ship a move order so it get's away from us
                new Controls.Command(
                Controls.CommandType.Move,
                (SpawnLocation.position + (SpawnLocation.up * 2)).ToString(),
                TeamID)
                );*/

            GState.Game.Entities.AddEntity(_newShip.GetInstanceID(), _newShip, _shipEnt);
        }

        public static void Dummy() { }

        public void BuildShip(SpaceshipData Data, int TeamID)
        {
            if (Constructing != null) { if (TeamID == 0) infoPopupController.ShowMessage("This drydock is already building something!"); return; }
            if (GlobalState.Game.Money[TeamID] < Data.ConstructionCost) { if (TeamID == 0) infoPopupController.ShowMessage($"You need {Data.ConstructionCost} currency to build that!"); return; }
            GlobalState.Game.Money[TeamID] -= Data.ConstructionCost;
            StartCoroutine(Construct(Data, TeamID));
        }

        private IEnumerator Construct(SpaceshipData Data, int TeamID)
        {
            Constructing = Data;
            yield return new WaitForSeconds(Data.ConstructionTime);

            SpawnShip(GlobalState, transform, Data, TeamID);
            Constructing = null;
        }
    }
}
