using System.Collections;
using MemoryPrototype.Logs;
using MemoryPrototype.StatePattern;
using UnityEngine;

namespace MemoryPrototype.Game.States
{
    public class State : IState
    {
        protected GameController gameControllerContext;
        protected LogController logController;

        protected bool IsInitialized { get; set; }

        public State(GameController context)
        {
            gameControllerContext = context;
            logController = gameControllerContext.LogController;
        }
        public bool Initialized()
        {
            return IsInitialized;
        }

        public virtual IEnumerator StartState()
        {
            throw new System.NotImplementedException();
        }
        
        public virtual void OnExecution()
        {
            throw new System.NotImplementedException();
        }

        public void OnEnter()
        {
            IsInitialized = true;
        }

        public void OnExit(State state)
        {
            gameControllerContext.ChangeState(state);
        }
    }
}

