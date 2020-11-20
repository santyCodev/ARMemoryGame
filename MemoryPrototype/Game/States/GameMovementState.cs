using MemoryPrototype.Placas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterController = MemoryPrototype.Character.CharacterController;

namespace MemoryPrototype.Game.States
{
    public class GameMovementState : State
    {
        private const string STATE_NAME = "GAME MOVEMENT STATE";                //Constante con el nombre de la clase

        private PlacasController placasController;                              //Controlador de placas
        private CharacterController characterController;                        //Controlador de personaje


        /*
            Constructor del estado
            - Llama al constructor del padre pasandole el contexto
            - Asigna el characterController y el placas controller
            - Asigna el la funcion CharacterHasEnd al evento OnCharacterFinish
            - Llama a OnEnter
         */
        public GameMovementState(GameController context) : base(context)
        {            
            characterController = gameControllerContext.CharacterController;
            placasController = gameControllerContext.PlacasController;
            CharacterController.OnCharacterFinish += CharacterHasEnd;
            OnEnter();
        }

        #region State Functions

        /*
            Imprime que esta en esta funcion
            Inicializa el estado llamando al OnEnter del padre
         */
        private new void OnEnter()
        {
            logController.PrintInConsole(STATE_NAME + " - ENTER");
            base.OnEnter();
        }

        /*
            Avisa al gameController que empieza la ejecucion
            Llama a la funcion OnExecution()            
         */
        public override IEnumerator StartState()
        {
            IsInitialized = false;
            OnExecution();
            yield return null;            
        }

        /*
            Funcion de ejecucion del estado
            - Indica que esta en esta funcion
            - Da el paso al character para mover al personaje
         */
        public override void OnExecution()
        {
            logController.PrintInConsole(STATE_NAME + " - EXECUTION");
            characterController.MoveCharacter();
        }

        /*
            Indica que esta en esta funcion
            - Quita la funcion CharacterHasEnd del evento OnCharacterFinish
            - Llama al siguiente estado
         */
        private void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            CharacterController.OnCharacterFinish -= CharacterHasEnd;
            base.OnExit(new GamePlayerState(gameControllerContext));
        }       

        #endregion

        #region Evento CharacterHasEnd

        /*
            Esta funcion se llama cuando el personaje ha terminado de moverse
                mediante el evento OnCharacterFinish
            - Indica que el personaje ha terminado de moverse
            - Devuelve el color original a las placas random
            - Llama a la funcion onExit
         */
        private void CharacterHasEnd() {
            logController.PrintInConsole(STATE_NAME + " - El chara ha terminado de moverse");
            placasController.SetOriginalMaterialColor();
            OnExit();
        }

        #endregion

    }
}

