using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrio.Entities
{
    public class DamageInfo
    {
        public float Amount;
        public DamageType Type;
        public IBaseEntity Attacker;
        public IBaseEntity Victim;
    }

    public enum DamageType
    {
        Normal
    }
}
