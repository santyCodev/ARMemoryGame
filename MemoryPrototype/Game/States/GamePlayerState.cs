﻿using MemoryPrototype.Placas;
using MemoryPrototype.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MemoryPrototype.Game.States
{
    public class GamePlayerState : State
    {
        private const string STATE_NAME = "GAME PLAYER STATE";              //Constante con el nombre de la clase

        private PlayerController playerController;                          //Controlador del jugador
        private PlacasController placasController;                          //Controlador de placas

        /*
            Constructor
                - Recoge desde el gameController, el controlador de placas y del player
                - Llama al constructor de la clase padre
                - Asigna la funcion ComparePlacas al evento OnPlacaClicked
                - Llama a la funcion OnEnter para inicializar el estado
         */
        public GamePlayerState(GameController context) : base(context)
        {
            placasController = gameControllerContext.PlacasController;
            playerController = gameControllerContext.PlayerController;
            PlayerController.OnPlacaClicked += ComparePlacas;
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
            Despues espera un frame y llama a OnExit()
         */
        public override IEnumerator StartState()
        {
            IsInitialized = false;
            OnExecution();
            yield return null;
            //OnExit();
        }

        /*
            Funcion de ejecucion del estado
            - Indica que esta en esta funcion
            - Inicializacion de placas
            - Inicializacion del personaje
         */
        public override void OnExecution()
        {
            logController.PrintInConsole(STATE_NAME + " - EXECUTION");
            playerController.StartExecute = true;
        }

        /*
            Indica que esta en esta funcion
            - Desasigna la funcion ComparePlacas del evento OnPlacaClicked
            - Llama al OnExit() del padre para cambiar de estado
            
         */
        private void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            PlayerController.OnPlacaClicked -= ComparePlacas;
            //base.OnExit(new GameMovementState(gameControllerContext));
        }

        #endregion

        private bool ComparePlacas(GameObject placaSelected)
        {
            logController.PrintInConsole(STATE_NAME + "ComparePlacas() - La placa seleccionada es "+placaSelected.name);

            return true;
        }
    }
}

