using MemoryPrototype.Placas;
using System.Collections;
using UnityEngine;
using CharacterController = MemoryPrototype.Character.CharacterController;

namespace MemoryPrototype.Game.States
{
    public class GameInitializationState : State
    {                        
        private const string STATE_NAME = "GAME INITIALIZACION STATE";

        private PlacasController placasController;
        private CharacterController characterController;

        public GameInitializationState(GameController context) : base(context)        
        {           
            placasController = gameControllerContext.PlacasController;
            characterController = gameControllerContext.CharacterController;
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
            PlacasInitialization();
            CharacterInitialization();
        }

        private void PlacasInitialization()
        {
            placasController.SetRandomPlacas();
            placasController.SetMarkedTag();
            logController.PrintInConsole(STATE_NAME + " Placas Initialization - DONE");
        }

        private void CharacterInitialization()
        {            
            characterController.PrepareForMovement(placasController.PlacasRandom);            
        }
    }
}
