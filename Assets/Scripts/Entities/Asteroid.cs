using Chrio.World;
using Chrio.World.Loading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chrio.Entities
{
    public class Asteroid : BaseEntity
    {
        public int AsteroidWorth = 300;

        public override void OnLoad(Game_State.State _gameState, ILoadableObject.CallBack _callback)
        {
            EntityType = "Asteroid";
            base.OnLoad(_gameState, _callback);
        }

        public override void OnDamaged(DamageInfo HitInfo)
        {
            base.OnDamaged(HitInfo);

            if (health <= 0)
            {
                GlobalState.Game.Money[HitInfo.Attacker.GetOwnerID()] += 300;
                GlobalState.Game.Entities.WorldEntities.Remove(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
