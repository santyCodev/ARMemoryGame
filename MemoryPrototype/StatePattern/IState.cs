using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.StatePattern
{
    public interface IState
    {
        bool Initialized();
        IEnumerator StartState();
        void OnEnter();
        void OnExit();
        void OnExecution();

    }
}

