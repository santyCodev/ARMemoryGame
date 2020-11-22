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
            OnExit();
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
            InitialzieRondaAndFase();
            UpFase();
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
            base.OnExit(new GameMovementState(gameControllerContext));
        }

        #endregion
        

        private void InitialzieRondaAndFase()
        {
            if (gameControllerContext.NumRonda == 0 && gameControllerContext.NumFase == 0)
            {
                gameControllerContext.NumRonda = 1;
                gameControllerContext.NumFase = 1;
            }
        }

        private void UpFase() { 
            //Sube de fase
            if(gameControllerContext.NumRonda > gameControllerContext.MaxRondas)
            {
                logController.PrintInConsole(STATE_NAME + " Subida de fase ");
                gameControllerContext.NumFase++;
                gameControllerContext.NumRonda = 1;
                placasController.NumPlacasRandom++;
                placasController.InstantiatePlacasRandom(placasController.NumPlacasRandom);
                logController.PrintInConsole(STATE_NAME + " - NumFase: "+ gameControllerContext.NumFase);
                logController.PrintInConsole(STATE_NAME + " - NumRonda: " + gameControllerContext.NumRonda);
                logController.PrintInConsole(STATE_NAME + " - Nueva placas random, longitud: " + placasController.PlacasRandom.Length);
            }
        }

        #region PlacasInitialization

        /*
            Iniclaliza las placas para el turno
            - Asigna las placas random
            - Asigna los tags a las placas random
            - Indica que la inicializacion ha terminado
         */
        private void PlacasInitialization()
        {
            if (placasController.PlacasRandom[0] != null) { placasController.SetRandomTagsToDefault(); }
            placasController.SetRandomPlacas();
            placasController.SetMarkedTag();
            logController.PrintInConsole(STATE_NAME + " Placas Initialization - DONE");
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
