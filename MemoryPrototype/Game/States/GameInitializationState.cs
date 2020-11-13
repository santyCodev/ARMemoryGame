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

        #region State Functions
        private new void OnEnter()
        {
            logController.PrintInConsole(STATE_NAME + " - ENTER");
            base.OnEnter();
        }

        public override IEnumerator StartState()
        {
            IsInitialized = false;
            OnExecution();
            yield return null;
            OnExit();
        }

        private void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            base.OnExit(new GameMovementState(gameControllerContext));
        }

        public override void OnExecution()
        {
            logController.PrintInConsole(STATE_NAME + " - EXECUTION");
            PlacasInitialization();
            CharacterInitialization();
        }

        #endregion

        #region PlacasInitialization
        private void PlacasInitialization()
        {
            placasController.SetRandomPlacas();
            placasController.SetMarkedTag();
            logController.PrintInConsole(STATE_NAME + " Placas Initialization - DONE");
        }

        #endregion

        #region CharacterInitialization
        private void CharacterInitialization()
        {
            characterController.PrepareForMovement(placasController.PlacasRandom);
            logController.PrintInConsole(STATE_NAME + " Character Initialization - DONE");
        }

        #endregion

    }//END CLASS
}
