using Chrio.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chrio.World
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Data/Weapon")]
    public class WeaponInfo : ScriptableObject
    {
        [Tooltip("Damage per second for continous weapons, Damage per shot for pulse weapons")]
        public float Damage;
        public WeaponType Type;

        [Tooltip("The colors the weapon should be for each team, 0 is player 1 is computer.")]
        public Color[] Colors = new Color[2];

        [Tooltip("Time between shots for pulse weapons")]
        public float Cooldown;

        [Tooltip("What type of damage this weapon outputs")]
        public DamageType DmgType;

        public enum WeaponType
        {
            Continous,
            Pulse,
            ContinousPulse
        }
    }
}
