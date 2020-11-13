using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MemoryPrototype.Game.States;

namespace MemoryPrototype.StatePattern
{
    public interface IState
    {
        bool Initialized();
        IEnumerator StartState();
        void OnEnter();
        void OnExit(State state);
        void OnExecution();
    }
}

