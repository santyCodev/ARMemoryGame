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
        private List<GameObject> placasRandom;                              //Las placas random
        private GameObject placaActual;                                     //La placa actual a comparar
        private int numPlaca;                                         //Indice de conteo de placas

        #region Inicializacion de estado
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
        
        /*
            Imprime que esta en esta funcion
            - Recoge la lista de placas random
            - Inicia el conteo de placas desde la ultima
            - Recoge la placa actual desde la lista
            - Inicializa el estado
         */
        private new void OnEnter()
        {
            PrintMessage(" - ENTER");
            placasRandom = placasController.placasRandom;
            numPlaca = placasRandom.Count - 1;
            placaActual = placasRandom[numPlaca];
            PrintMessage(" OnEnter() - Ultima placa: "+ numPlaca +" - "+placaActual.transform.position);
            PrintMessage(" OnEnter() - DONE");
            base.OnEnter();
        }
        #endregion

        #region Ejecucion del estado
        /*
            Avisa al gameController que empieza la ejecucion
            Llama a la funcion OnExecution()
            Despues espera un frame
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
            - Da paso al jugador para que pueda hacer click en las placas
         */
        public override void OnExecution()
        {
            PrintMessage(" - EXECUTION");
            playerController.StartExecute = true;
        }
        #endregion

        #region Funcionalidad de comparacion de placas (evento)
        /*
            Evento llamado cuando el jugador ha terminado de elegir una placa
            - Se comprueba si la placa elegida es la correcta en orden inverso
                a la recorrida por el character
         */
        private void ComparePlacas(GameObject placaSelected)
        {
            //Si es la ultima placa
            if (numPlaca == placasRandom.Count - 1) { CheckPlacas(true, placaSelected); }
            //Si la placa es menor a la ultima y mayor que la primera
            else if (numPlaca < placasRandom.Count - 1 && numPlaca > 0) { CheckPlacas(true, placaSelected); }
            //Si es la primera placa
            else if (numPlaca == 0) { CheckPlacas(false, placaSelected); }
        }

        /*
            Funcion que compara la placa seleccionada con la placa correspondiente
                en la posicion del array
                - Las placas se comparan en orden inverso de la lista
                - needPlacaSiguiente se usa cuando en el momento de comparar una placa,
                    sabemos si existe otra placa a comparar despues.
         */
        private void CheckPlacas(bool needPlacaSiguiente, GameObject placaSelected)
        {
            if (placasRandom[numPlaca].Equals(placaSelected))
            {
                if (logController.enabled) { PrintPlacaInfo("si", placaSelected); }
                if (needPlacaSiguiente) { placaSiguiente(); }
                else { nextTurn(); }
            }
            else
            {
                if (logController.enabled) { PrintPlacaInfo("no", placaSelected); }
                //gameControllerContext.NumRonda = 0;
                //OnExit();
            }
        }

        /*
            Avanza a la siguiente placa a comparar, como es en orden inverso 
                elegimos el indice de la placa desde la ultima de la lista
                - Con el indice elegido elegimos la placa de esa posicion
         */
        private void placaSiguiente()
        {
            if (numPlaca > 0)
            {
                numPlaca--;
                placaActual = placasRandom[numPlaca];
            }
        }

        private void nextTurn()
        {
            //gameControllerContext.NumRonda++;
            //OnExit();
        }
        #endregion

        #region Finalizacion del estado
        /*
            Indica que esta en esta funcion
            - 
            - Desasigna la funcion ComparePlacas del evento OnPlacaClicked
            - Llama al OnExit() del padre para cambiar de estado
            
         */
        private void OnExit()
        {
            PrintMessage(" - EXIT");
            PlayerController.OnPlacaClicked -= ComparePlacas;
            //base.OnExit(new GameInitializationState(gameControllerContext));
        }
        #endregion

        #region Gestion de Logs
        private void PrintPlacaInfo(string yesno, GameObject placaSelected)
        {
            PrintMessage(" ComparePlacas() - Esa "+ yesno + "es la placa " + numPlaca);
            PrintMessage(" ComparePlacas() - Placa random actual " + placasRandom[numPlaca].name);
            PrintMessage(" ComparePlacas() - Placa seleccionada " + placaSelected.name);
        }
        
        /*
            Usa el controlador de logs para imprimir un mensaje en consola
         */
        private void PrintMessage(string message)
        {
            logController.PrintInConsole(STATE_NAME + message);
        }
        #endregion
    }
}

