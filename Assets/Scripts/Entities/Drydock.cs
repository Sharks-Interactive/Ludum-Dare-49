using UnityEngine;
using System.Collections.Generic;
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
                OwnerID = (OwnerID == 0 ? 1 : 0); // Swap owners
                health = EntityData.Health; // Reset health
            }
        }

        private void SpawnShip(SpaceshipData Data, int TeamID)
        {
            GameObject _newShip = (GameObject)Instantiate(Resources.Load("Spaceship"), transform.position, transform.rotation);
            IBaseEntity _shipEnt = _newShip.GetComponent<IBaseEntity>();
            Spaceship ship = _shipEnt as Spaceship;

            ship.OnLoad(GlobalState, Dummy);

            _shipEnt.GetEntity().OwnerID = TeamID;
            _shipEnt.OnCommand( // Give the newly created ship a move order so it get's away from us
                new Controls.Command(
                Controls.CommandType.Move, 
                (transform.position + transform.forward).ToString(), 
                TeamID)
                );
        }

        public void Dummy() { }

        public void BuildShip(SpaceshipData Data, int TeamID)
        {
            if (Constructing != null) return;
            if (GlobalState.Game.Money[TeamID] < Data.ConstructionCost) return;
            GlobalState.Game.Money[TeamID] -= Data.ConstructionCost;
            StartCoroutine(Construct(Data, TeamID));
        }

        private IEnumerator Construct(SpaceshipData Data, int TeamID)
        {
            Constructing = Data;
            yield return new WaitForSeconds(Data.ConstructionTime);

            SpawnShip(Data, TeamID);
            Constructing = null;
        }
    }
}
