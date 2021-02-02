using MemoryPrototype.Gui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Game.States
{
    public class ResultsState : State
    {
        private const string STATE_NAME = "RESULTS STATE";
        private static ResultsState resultStateInstance;
        private GUIController guiController;                                    //Controlador de GUI

        #region Inicializacion del estado
        private ResultsState(GameController context) : base(context)
        {
            guiController = gameControllerContext.GuiController;
            OnEnter();
        }

        public static ResultsState GetResultState(GameController context)
        {
            if(resultStateInstance == null)
            {
                resultStateInstance = new ResultsState(context);
            }

            return resultStateInstance;
        }

        private new void OnEnter()
        {            
            base.OnEnter();
        }

        public override IEnumerator StartState()
        {
            IsInitialized = false;
            OnExecution();
            yield return null;
        }
        #endregion

        #region Ejecucion del estado
        public override void OnExecution()
        {
            guiController.ActivateResultados();
        }
        #endregion

        #region Finalizacion del estado
        private void OnExit()
        {
            
        }
        #endregion
    }
}
