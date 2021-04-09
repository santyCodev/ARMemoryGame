using MemoryPrototype.Data;
using MemoryPrototype.Gui;
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

        private DataController dataController;                            //Controlador de personaje
        private PlayerController playerController;                          //Controlador del jugador
        private PlacasController placasController;                          //Controlador de placas
        private PlacaControl placaControl;
        private GUIController guiController;                                //Controlador de GUI
        private List<GameObject> placasRandom;                              //Las placas random
        private GameObject placaActual;                                     //La placa actual a comparar
        private int numPlaca;                                         //Indice de conteo de placas
        private bool isReactionMedition;
        private PlacaControl actualPlacaControl;

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
            dataController = gameControllerContext.DataController;
            placasController = gameControllerContext.PlacasController;
            playerController = gameControllerContext.PlayerController;
            guiController = gameControllerContext.GuiController;
            PlayerController.OnPlacaClicked += CheckPlacas;
            PlayerController.OnClickedReaction += SendReactionTime;
            PlacaControl.OnPlacaAnimationFail += CheckFallosAndEndTurn;
            PlacaControl.OnPlacaAnimationSuccess += CheckRondasAndEndTurn;
            GUIController.OnBarraCuentaAtrasTerminada += StopPlayer;
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
            PrintMessage(" OnEnter() - Se espera la placa: "+ placasRandom[numPlaca].name +" - "+placaActual.transform.position);
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
            isReactionMedition = true;
            dataController.StartReactionMedition();
            dataController.StartAccuracyMedition();
            //comenzar a medir el tiempo de reaccion
        }
        #endregion

        #region Funcionalidad de comparacion de placas (evento)
        /*
            Evento que compara la placa seleccionada con la placa correspondiente
                en la posicion del array
                - Las placas se comparan en orden inverso de la lista
                - Si las placas comparadas son iguales = ACIERTO:
                    - Se marca el color de la placa
                    - Se sube el numero de aciertos
                    - Se comprueba si se necesita la siguiente placa
                        - Si es asi, se asigna la placa siguiente
                        - Si no, comprueba si ha llegado al maximo de aciertos                     
                - Si las placas comparadas no son iguales = FALLO
                    - Se sube el contador de fallos
                    - Se ejecuta la animacion de fallo
         */
        private void CheckPlacas(GameObject placaSelected)
        {
            PrintMessage(" CheckPlacas() - INICIO");
            if (placasRandom[numPlaca].Equals(placaSelected))
            {                
                PrintMessage(" CheckPlacas() - La placa "+ placaSelected.name +" y la placa "+ placasRandom[numPlaca].name+" SI son iguales");                
                placasRandom[numPlaca].GetComponent<PlacaControl>().ChangeMarkedMaterial();
                dataController.UpAcierto();
                dataController.StopAccuracyMedition();
                dataController.UpAciertosTotales();
                if(NeedPlacaSiguiente())
                { 
                    SetPlacaSiguiente();
                    playerController.StartExecute = true;
                    dataController.StartAccuracyMedition();
                }
                else { GestionAciertos(); }              
            }
            else//fallo
            {
                PrintMessage(" CheckPlacas() - La placa " + placaSelected.name + " y la placa " + placasRandom[numPlaca].name + " NO son iguales");                
                dataController.UpFallo();
                dataController.UpFallosTotales();
                placasRandom[numPlaca].GetComponent<PlacaControl>().FailAnimation();                 
            }
            PrintMessage(" CheckPlacas() - FIN");            
        }

        /*
            Avanza a la siguiente placa a comparar, como es en orden inverso 
                elegimos el indice de la placa desde la ultima de la lista
                - Con el indice elegido elegimos la placa de esa posicion
         */
        private void SetPlacaSiguiente()
        {
            PrintMessage(" SetPlacaSiguiente() - INICIO");
            PrintMessage(" SetPlacaSiguiente() - Placa actual = " + placaActual.name);
            if (numPlaca > 0)
            {
                numPlaca--;
                placaActual = placasRandom[numPlaca];
                PrintMessage(" SetPlacaSiguiente() - Placa siguiente: " + placaActual.name);
            }
            PrintMessage(" SetPlacaSiguiente() - FIN");
        }

        /*
            Si es la ultima placa o es la placa inferior a la ultima pero superior a la primera
                devuelve true
            En caso contrario, si es la primera placa
                devuevle false
         */
        private bool NeedPlacaSiguiente() { return (numPlaca == placasRandom.Count - 1) || (numPlaca < placasRandom.Count - 1 && numPlaca > 0); }
        #endregion

        #region Funcionalidad de envio de tiempo de reaccion (evento)
        private void SendReactionTime()
        {
            if (isReactionMedition)
            {
                isReactionMedition = false;
                dataController.StopReactionMedition();
            }            
        }
        #endregion

        #region Logica de aciertos, fallos, rondas y niveles
        /*
            Se comprueba si ha llegado al maximo de aciertos
                - Si es correcto ejecuta la animacion de acierto
         */
        private void GestionAciertos()
        {            
            PrintMessage(" GestionAciertos() - INICIO");
            if (dataController.IsMaxAciertos(placasRandom.Count))
            {
                PrintMessage(" GestionAciertos() - Se ha llegado al maximo de aciertos");
                placasRandom[numPlaca].GetComponent<PlacaControl>().SuccessAnimation();
            }
            PrintMessage(" GestionAciertos() - FIN");
        }

        /*
            Evento que comprueba si ha llegado al maximo de rondas
                - Si ha llegado al maximo, sube de nivel
                - Si no, sube el contador de rondas
                - Al final de todo sale de este turno
         */
        private void CheckRondasAndEndTurn()
        {
            PrintMessage(" CheckEndTurn() - INICIO");
            bool isUpLevel = false;
            if (dataController.IsMaxRondas())
            {
                PrintMessage(" CheckEndTurn() - Se ha llegado al maximo de rondas");
                placasController.NumPlacasRandom++;
                dataController.UpLevel();
                isUpLevel = true;
            }

            if (!isUpLevel) { dataController.UpRonda(); }
            PrintMessage(" CheckEndTurn() - FIN");
            OnExit();
        }

        /*
            Evento que comprueba si ha llegado al maximo de fallos
                - Si es correcto se comprueba que el nivel actual sea mayor que 1
                    - Si es correcto baja el nivel actual
                    - Si es el nivel 1, resetea el nivel 1
                - Al final de todo, termina el turno
         */
        private void CheckFallosAndEndTurn()
        {
            PrintMessage(" GestionFallos() - INICIO");
            if (dataController.IsMaxFallos())
            {
                PrintMessage(" GestionFallos() - Se ha llegado al maximo de fallos");
                if (dataController.GetActualLevel() > 1)
                {
                    PrintMessage(" GestionFallos() - Se va a bajar de nivel");
                    placasController.NumPlacasRandom--;
                    dataController.DownLevel();
                }
                else if(dataController.GetActualLevel() == 1)
                {
                    dataController.ResetLevelOne();
                }
            }
            PrintMessage(" GestionFallos() - FIN");
            OnExit();
        }
        #endregion

        #region Evento stopPlayer
        private void StopPlayer()
        {
            playerController.StopPlayer = true;
            OnExit();
        }
        #endregion

        #region Finalizacion del estado
        /*
            - Desasigna la funcion CheckPlacas del evento OnPlacaClicked
            - Desasigna la funcion CheckFallosAndEndTurn del evento OnPlacaAnimationFail
            - Desasigna la funcion CheckRondasAndEndTurn del evento OnPlacaAnimationSuccess
            - Llama al OnExit() del padre para cambiar de estado
            
         */
        private void OnExit()
        {
            PrintMessage(" - EXIT");
            playerController.StartExecute = false;
            PlayerController.OnPlacaClicked -= CheckPlacas;
            PlayerController.OnClickedReaction -= SendReactionTime;
            PlacaControl.OnPlacaAnimationFail -= CheckFallosAndEndTurn;
            PlacaControl.OnPlacaAnimationSuccess -= CheckRondasAndEndTurn;
            GUIController.OnBarraCuentaAtrasTerminada -= StopPlayer;
            if (playerController.StopPlayer)
            {
                dataController.EndSession();
                gameControllerContext.CurrentState = null;
            }
            else
            {
                placasController.ChangeOriginalMaterial();
                base.OnExit(new GameInitializationState(gameControllerContext));
            }            
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
    }
}

