﻿using MemoryPrototype.Data;
using MemoryPrototype.Gui;
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
        private GUIController guiController;                                    //Controlador de GUI
        private DataController dataController;

        #region Inicializacion del estado
        /*
            Constructor del estado
            - Llama al constructor del padre pasandole el contexto
            - Asigna el characterController y el placas controller
            - Asigna el la funcion CharacterHasEnd al evento OnCharacterFinish
            - Llama a OnEnter
         */
        public GameMovementState(GameController context) : base(context)
        {
            dataController = gameControllerContext.DataController;
            characterController = gameControllerContext.CharacterController;
            placasController = gameControllerContext.PlacasController;
            guiController = gameControllerContext.GuiController;
            CharacterController.OnCharacterFinish += CharacterHasEnd;
            GUIController.OnBarraCuentaAtrasTerminada += StopCharacter;
            OnEnter();
        }

        /*
            Imprime que esta en esta funcion
            Inicializa el estado llamando al OnEnter del padre
         */
        private new void OnEnter()
        {
            PrintMessage(" - ENTER");
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
        #endregion

        #region Ejecucion del estado
        /*
            Funcion de ejecucion del estado
            - Indica que esta en esta funcion
            - Da el paso al character para mover al personaje
         */
        public override void OnExecution()
        {
            PrintMessage(" OnExecution() - START");
            characterController.MoveCharacter();
            PrintMessage(" OnExecution() - END");
        }

        #region Evento CharacterHasEnd

        /*
            Esta funcion se llama cuando el personaje ha terminado de moverse
                mediante el evento OnCharacterFinish
            - Indica que el personaje ha terminado de moverse
            - Devuelve el color original a las placas random
            - Llama a la funcion onExit
         */
        private void CharacterHasEnd()
        {
            PrintMessage(" CharacterHasEnd() - START");
            placasController.ChangeOriginalMaterial();
            OnExit();
            PrintMessage(" CharacterHasEnd() - END");
        }

        private void StopCharacter()
        {
            PrintMessage(" StopCharacter() - START");
            characterController.StopWalk = true;
            PrintMessage(" StopCharacter() - END");
        }

        #endregion

        #endregion

        #region Finalizacion del estado
        /*
            Indica que esta en esta funcion
            - Da de baja la funcion CharacterHasEnd del evento OnCharacterFinish
            - Da de baja la funcion StopCharacter del evento OnBarraCuentaAtrasTerminada
            - Evalua si el character ha parado:
                - Si es correcto transita al estado final "ResultState"
                - Si no es correcto desactiva el character y transita al siguiente estado
                    "GamePlayerState"
         */
        private void OnExit()
        {
            PrintMessage(" OnExit() - START");
            CharacterController.OnCharacterFinish -= CharacterHasEnd;
            GUIController.OnBarraCuentaAtrasTerminada -= StopCharacter;
            if (characterController.StopWalk)
            {
                dataController.EndSession();                
                gameControllerContext.CurrentState = null;
            }
            else
            {
                characterController.SetActiveCharacter(false);
                base.OnExit(new GamePlayerState(gameControllerContext));
            }
            PrintMessage(" OnExit() - END");
        }

        #endregion

        #region Gestion de Logs
        /*
            Usa el controlador de logs para imprimir un mensaje en consola
         */
        private void PrintMessage(string message)
        {
            logController.PrintInConsole(STATE_NAME + message);
        }
        #endregion
    
    }//END CLASS
}

