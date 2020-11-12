using MemoryPrototype.Game;
using MemoryPrototype.Logs;
using MemoryPrototype.Placas;
using MemoryPrototype.StatePattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Game.States
{
    public class GameInitializationState : State
    {                        
        private const string STATE_NAME = "Game Initializacion State";

        private PlacasController placasController;

        public GameInitializationState(GameController context) : base(context)        
        {           
            placasController = gameControllerContext.PlacasController;            
            OnEnter();
        }

        public override IEnumerator StartState()
        {
            IsExecuting = false;
            yield return new WaitForSeconds(3);
            StateExecution();
            yield return new WaitForSeconds(5);
            OnExit();
        }

        private void OnEnter()
        {            
            logController.PrintInConsole(STATE_NAME + " - ENTER");
            IsExecuting = true;
        }

        private void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            //Context.ChangeState(new GameState(Context));
        }

        private void StateExecution()
        {
            logController.PrintInConsole(STATE_NAME + " - EXECUTION");
        }
    }
}
