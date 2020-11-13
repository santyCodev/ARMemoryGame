using MemoryPrototype.Placas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterController = MemoryPrototype.Character.CharacterController;

namespace MemoryPrototype.Game.States
{
    public class GameMovementState : State
    {
        private const string STATE_NAME = "GAME MOVEMENT STATE";

        private PlacasController placasController;
        private CharacterController characterController;

        public GameMovementState(GameController context) : base(context)
        {            
            characterController = gameControllerContext.CharacterController;
            placasController = gameControllerContext.PlacasController;
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

        private new void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            //placasController.SetOriginalMaterialColor();
            //base.OnExit(new State());
        }

        public override void OnExecution()
        {
            logController.PrintInConsole(STATE_NAME + " - EXECUTION");
            characterController.MoveCharacter();
        }

        #endregion
    }
}

