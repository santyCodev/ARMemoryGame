using MemoryPrototype.Game;
using MemoryPrototype.Logs;
using MemoryPrototype.StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Game.States
{
    public class State : IState
    {
        protected GameController gameControllerContext;
        protected LogController logController;

        protected bool IsExecuting { get; set; }
        //public GameController GameControllerContext { get; set; }

        public State(GameController context)
        {
            gameControllerContext = context;
            logController = gameControllerContext.LogController;
        }
        public bool IsRunning()
        {
            return IsExecuting;
        }

        public virtual IEnumerator StartState()
        {
            throw new System.NotImplementedException();
        }
    }
}

