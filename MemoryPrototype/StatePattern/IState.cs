using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.StatePattern
{
    public interface IState
    {
        bool IsRunning();
        IEnumerator StartState();
    }
}

