using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Simpolony.Misc
{
    public abstract class ManagerComponent : ScriptableObject
    {
        public abstract void DoAwake();
        public abstract void DoStart();
        public abstract void DoUpdate();
    }
}
