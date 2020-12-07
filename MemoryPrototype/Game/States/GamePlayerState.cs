using MemoryPrototype.Placas;
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
        private List<GameObject> placasRandom;                                  //Las placas random
        private GameObject placaActual;                                     //La placa actual a comparar
        private int numPlacaActual;                                         //Indice de conteo de placas
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
            - Recoge la lista de placas random
            - Inicia el conteo de placas desde la ultima
            - Recoge la placa actual desde la lista
            - Inicializa el estado
         */
        private new void OnEnter()
        {
            logController.PrintInConsole(STATE_NAME + " - ENTER");
            placasRandom = placasController.placasRandom;
            numPlacaActual = placasRandom.Count - 1;
            placaActual = placasRandom[numPlacaActual];
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
            - Da paso al jugador para que pueda hacer clickhol
         */
        public override void OnExecution()
        {
            logController.PrintInConsole(STATE_NAME + " - EXECUTION");
            playerController.StartExecute = true;
        }

        /*
            Indica que esta en esta funcion
            - 
            - Desasigna la funcion ComparePlacas del evento OnPlacaClicked
            - Llama al OnExit() del padre para cambiar de estado
            
         */
        private void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            PlayerController.OnPlacaClicked -= ComparePlacas;
            base.OnExit(new GameInitializationState(gameControllerContext));
        }

        #endregion

        

        private void ComparePlacas(GameObject placaSelected)
        {
            //Si es la ultima placa
            if (numPlacaActual == placasRandom.Count - 1) { CheckPlacas(true, placaSelected); }
            //Si la placa es menor a la ultima y mayor que la primera
            else if (numPlacaActual < placasRandom.Count - 1 && numPlacaActual > 0){ CheckPlacas(true, placaSelected); }
            //Si es la primera placa
            else if (numPlacaActual == 0){ CheckPlacas(false, placaSelected); }            
        }

        private void CheckPlacas(bool needPlacaSiguiente, GameObject placaSelected)
        {
            if (placasRandom[numPlacaActual].Equals(placaSelected))
            {
                if (logController.enabled) { PrintPlacaInfo("si", placaSelected); }
                if (needPlacaSiguiente) { placaSiguiente(); }
                else { nextTurn(); }
            }
            else
            {
                if (logController.enabled) { PrintPlacaInfo("no", placaSelected); }
                //gameControllerContext.NumRonda = 0;
                OnExit();
            }
        }

        private void placaSiguiente()
        {
            if(numPlacaActual > 0)
            {
                numPlacaActual--;
                placaActual = placasRandom[numPlacaActual];
            }            
        }

        private void nextTurn()
        {
            //gameControllerContext.NumRonda++;
            OnExit();
        }

        
        private void PrintPlacaInfo(string yesno, GameObject placaSelected)
        {
            logController.PrintInConsole(STATE_NAME + "ComparePlacas() - Esa "+ yesno + "es la placa " + numPlacaActual);
            logController.PrintInConsole(STATE_NAME + "ComparePlacas() - Placa random actual " + placasRandom[numPlacaActual].name);
            logController.PrintInConsole(STATE_NAME + "ComparePlacas() - Placa seleccionada " + placaSelected.name);
        }

        
    }
}

