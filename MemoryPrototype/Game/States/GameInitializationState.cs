using MemoryPrototype.Data;
using MemoryPrototype.Placas;
using System.Collections;
using UnityEngine;
using CharacterController = MemoryPrototype.Character.CharacterController;

namespace MemoryPrototype.Game.States
{
    public class GameInitializationState : State
    {                        
        private const string STATE_NAME = "GAME INITIALIZACION STATE";              //Constante con el nombre de la clase

        private PlacasController placasController;                                  //Controlador de placas
        private CharacterController characterController;                            //Controlador de personaje
        private DataController dataController;                            //Controlador de personaje

        #region Inicializacion del estado

        /*
            Constructor
                - Recoge desde el gameController, el controlador de placas y del character
                - Llama al constructor de la clase padre
                - Llama a la funcion OnEnter para inicializar el estado
         */
        public GameInitializationState(GameController context) : base(context)        
        {           
            placasController = gameControllerContext.PlacasController;
            characterController = gameControllerContext.CharacterController;
            dataController = gameControllerContext.DataController;
            OnEnter();
            PrintMessage(" Inicializacion = "+base.IsInitialized+"- DONE");
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
            Despues espera un frame y llama a OnExit()
         */
        public override IEnumerator StartState()
        {
            IsInitialized = false;
            OnExecution();
            yield return null;
            OnExit();
        }

        #endregion

        #region Ejecucion del estado
        /*
            Funcion de ejecucion del estado
            - Checkea si se puede bajar o subir de nivel
            - Inicializacion de placas
            - Inicializacion del personaje
         */
        public override void OnExecution()
        {
            PrintMessage(" - EXECUTION");            
            PlacasRandomInitialization();
            CharacterInitialization();
        }

        /*
            Iniclaliza las placas random para el turno
            - Si el nivel actual es mayor que 1, inicializa con el numero de placas dado por el nivel
            - Si el nivel actual es 1 pero ha bajado de nivel, se inicializa con el numero de placas default
            - Si no se cumple ninguna condicino, significa que es el comienzo del juego, entonces ya estara
                inicializado en el controlador de placas
         */
        private void PlacasRandomInitialization()
        {
            if (dataController.GetActualLevel() > 1) 
            { 
                placasController.InitializePlacasRandom(dataController.GetActualLevel()); 
            }
            else if (dataController.GetActualLevel() == 1 && dataController.GetBeforeLevel() > 1)
            {
                placasController.InitializePlacasRandom();
            }
            PrintMessage(" PlacasRandomInitialization() - NivelActual = "+ dataController.GetActualLevel());
        }

        /*
            Inicializacion del personaje
            - Prepara al personaje para su movimiento
            - Indica que la inicializacion ha finalizado
         */
        private void CharacterInitialization()
        {
            if (!characterController.GetIsActive())
            {
                characterController.SetActiveCharacter(true);
            }
            characterController.PrepareForMovement(placasController.PlacasRandom);
            logController.PrintInConsole(STATE_NAME + " Character Initialization - DONE");
        }

        #endregion

        #region Terminacion del estado
        /*
            Indica que esta en esta funcion
            Llama al OnExit() del padre para cambiar de estado
         */
        private void OnExit()
        {
            logController.PrintInConsole(STATE_NAME + " - EXIT");
            //base.OnExit(new GameMovementState(gameControllerContext));
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
