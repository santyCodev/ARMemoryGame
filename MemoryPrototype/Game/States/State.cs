using System.Collections;
using MemoryPrototype.Logs;
using MemoryPrototype.StatePattern;
using UnityEngine;

namespace MemoryPrototype.Game.States
{
    public class State : IState
    {
        protected GameController gameControllerContext;                 //Contexto del patron state
        protected LogController logController;                          //Controlador de logs

        protected bool IsInitialized { get; set; }                      //Indica la inicializacion del estado

        /*
            Constructor del estado
            - Asigna el contexto, que es el GameController
            - Asigna el log controler desde el gameController
         */
        public State(GameController context)
        {
            gameControllerContext = context;
            logController = gameControllerContext.LogController;
        }

        /*
            Devuelve si el estado esta inicializado o no
         */
        public bool Initialized()
        {
            return IsInitialized;
        }

        /*
            Pone en marcha el estado
            - no hace nada
            - se delega la funcinoalidad a cada estado particular
         */
        public virtual IEnumerator StartState()
        {
            throw new System.NotImplementedException();
        }
        
        /*
            Ejecuta la funcionalidad del estado
            - No hace nada
            - Se delega la funcinoalidad a cada estado particular
         */
        public virtual void OnExecution()
        {
            throw new System.NotImplementedException();
        }

        /*
            Inicializa el estado
         */
        public void OnEnter()
        {
            IsInitialized = true;
        }

        /*
            Se encarga de cambiar el estado usando el gameController
         */
        public void OnExit(State state)
        {
            gameControllerContext.ChangeState(state);
        }
    }
}

