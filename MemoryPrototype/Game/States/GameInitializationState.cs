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

        private new void OnEnter()
        {
            logController.PrintInConsole(STATE_NAME + " - ENTER");
            base.OnEnter();
        }

        public override IEnumerator StartState()
        {
            IsInitialized = false;
            yield return new WaitForSeconds(3);
            OnExecution();
            yield return new WaitForSeconds(5);
            OnExit();
        }
        
        private new void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            base.OnExit();
        }

        public override void OnExecution()
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
            logController.PrintInConsole(STATE_NAME + " Character Initialization - DONE");
        }
    }
}
