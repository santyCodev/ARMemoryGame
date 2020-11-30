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

        #region State Functions

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
        }
        
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
            OnExit();
        }

        /*
            Funcion de ejecucion del estado
            - Checkea si se puede bajar o subir de nivel
            - Inicializacion de placas
            - Inicializacion del personaje
         */
        public override void OnExecution()
        {
            logController.PrintInConsole(STATE_NAME + " - EXECUTION");            
            PlacasInitialization();
            CharacterInitialization();
        }

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

        #region Ronda y Fase

        #endregion

        #region PlacasInitialization

        /*
            Iniclaliza las placas para el turno
            - Instancia las placas random
            - Asigna las placas random
            - Asigna los tags a las placas random
            - Indica que la inicializacion ha terminado
         */
        private void PlacasInitialization()
        {            
            InitializePlacasRandom();
            placasController.SetRandomPlacas();
            placasController.SetMarkedTag();
            logController.PrintInConsole(STATE_NAME + " Placas Initialization - DONE");
        }
        
        /*
            Inicializa las placas random en funcion del numero de rondas
            - Si es el primer nivel, se inicializa con el numero de placas default
            - Si es un nivel superior, se inicializa con el numero de placas modificado
         */
        private void InitializePlacasRandom()
        {
            if(dataController.GetLevel() == 1) { placasController.InitializePlacasRandom(); }
            else if(dataController.GetLevel() > 1) { placasController.InitializePlacasRandom(placasController.NumPlacasRandom); }
            logController.PrintInConsole(STATE_NAME + " - Nueva placas random, longitud: " + placasController.PlacasRandom.Length);
        }
        #endregion

        #region CharacterInitialization

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

    }//END CLASS
}
