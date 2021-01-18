using MemoryPrototype.Data;
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
            PlayerController.OnPlacaClicked += CheckPlacas;           
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
            Evento llamado cuando el jugador ha terminado de elegir una placa (OnPlacaClicked)
            - Se comprueba si la placa elegida es la correcta en orden inverso
                a la recorrida por el character
         */
        //private void ComparePlacas(GameObject placaSelected)
        //{
        //    PrintMessage(" ComparePlacas() - INICIO");
        //    //Si es la ultima placa
        //    if (numPlaca == placasRandom.Count - 1) { CheckPlacas(placaSelected); }
        //    //Si la placa es menor a la ultima y mayor que la primera
        //    else if (numPlaca < placasRandom.Count - 1 && numPlaca > 0) { CheckPlacas(placaSelected); }
        //    //Si es la primera placa
        //    else if (numPlaca == 0) { CheckPlacas(placaSelected); }
        //    PrintMessage(" ComparePlacas() - FIN");
        //}

        /*
            Funcion que compara la placa seleccionada con la placa correspondiente
                en la posicion del array
                - Las placas se comparan en orden inverso de la lista
                - Si las placas comparadas son iguales = ACIERTO:
                    - se comprueba maximos de acierto y ronda, y subida de nivel
                        - Si la comprobacion es true, cambia de turno
                    - en caso contrario, se espera la placa siguiente
                - Si las placas comparadas no son iguales = FALLO
                    - se comprueba maximo de fallo y bajada de nivel
                    - Se cambia de turno
                    
         */
        private void CheckPlacas(GameObject placaSelected)
        {
            PrintMessage(" CheckPlacas() - INICIO");
            if (placasRandom[numPlaca].Equals(placaSelected))
            {                
                PrintMessage(" CheckPlacas() - La placa "+ placaSelected.name +" y la placa "+ placasRandom[numPlaca].name+"son iguales");
                placasRandom[numPlaca].GetComponent<PlacaControl>().ChangeMaterialColor();
                if (GestionAciertos()) { //OnExit();
                }
                else { placaSiguiente(); }
            }
            else//fallo
            {
                PrintMessage(" CheckPlacas() - La placa " + placaSelected.name + " y la placa " + placasRandom[numPlaca].name + "NO son iguales");
                placasRandom[numPlaca].GetComponent<PlacaControl>().ChangeMaterialFailColor();
                GestionFallos();
                //OnExit();
            }
            PrintMessage(" CheckPlacas() - FIN");
        }

        /*
            Avanza a la siguiente placa a comparar, como es en orden inverso 
                elegimos el indice de la placa desde la ultima de la lista
                - Con el indice elegido elegimos la placa de esa posicion
         */
        private void placaSiguiente()
        {
            PrintMessage(" placaSiguiente() - INICIO");
            PrintMessage(" placaSiguiente() - Se necesita la placa siguiente del recorrido");
            if (numPlaca > 0)
            {
                numPlaca--;
                placaActual = placasRandom[numPlaca];
                PrintMessage(" placaSiguiente() - Placa siguiente: " + placaActual.name);
            }
            PrintMessage(" placaSiguiente() - FIN");
        }
        #endregion

        #region Logica de aciertos, fallos, rondas y niveles

        /*
            Se comprueba si se ha subido un acierto y si ha llegado al maximo:
                - Si ha llegado al maximo de aciertos, comprueba si se ha subido de rondas
                - Si ha llegado al maximo de rondas, se sube de nivel
         */
        private bool GestionAciertos()
        {
            bool result = false;

            if (CheckAciertos())
            {
                PrintMessage(" GestionAciertos() - Se ha llegado al maximo de aciertos");
                if (CheckRondas())
                {
                    PrintMessage(" GestionAciertos() - Se ha llegado al maximo de rondas");
                    dataController.UpLevel();
                }
                result = true;
            }           

            return result;
        }

        /*
            Se comprueba si se ha subido un fallo y si ha llegado al maximo:
                - Si ha llegado al maximo de fallos, comprueba si el nivel es mayor a 1
                - Si el nivel actual es mayor que 1, se baja de nivel
         */
        private void GestionFallos()
        {
            if (CheckFallos())
            {
                PrintMessage(" GestionFallos() - Se ha llegado al maximo de fallos");
                if (dataController.GetActualLevel() > 1)
                {
                    PrintMessage(" GestionFallos() - Se va a bajar de nivel");
                    dataController.DownLevel();
                }
            }
        }

        /*
            Sube el contador de aciertos y comprueba si se ha llegado al maximo de aciertos
            Para comprobar el maximo de aciertos: numero de placas == numero de aciertos
         */
        private bool CheckAciertos()
        {
            dataController.UpAcierto();
            return dataController.IsMaxAciertos(placasRandom.Count);
        }

        /*
            Sube el contador de rondas y comprueba si se ha llegado al maximo de rondas
            Para comprobar el maximo de rondas: numero de rondas == MAX_RONDAS
         */
        private bool CheckRondas()
        {
            dataController.UpRonda();
            return dataController.IsMaxRondas();
        }

        /*
            Sube el contador de fallos y comprueba si se ha llegado al maximo de fallos
            Para comprobar el maximo de fallos: numero de fallos == MAX_FALLOS
         */
        private bool CheckFallos()
        {
            dataController.UpFallo();
            return dataController.IsMaxFallos();
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
            PlayerController.OnPlacaClicked -= CheckPlacas;
            //base.OnExit(new GameInitializationState(gameControllerContext));
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

