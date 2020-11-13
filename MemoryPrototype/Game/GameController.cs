using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

using MemoryPrototype.StatePattern;
using MemoryPrototype.Placas;
using MemoryPrototype.Game.States;
using MemoryPrototype.Logs;
using CharacterController = MemoryPrototype.Character.CharacterController;

namespace MemoryPrototype.Game 
{
    //Clase que actua como contexto
    public class GameController : MonoBehaviour    
    {
        [SerializeField] private LogController logController;                       //Controlador de logs
        [SerializeField] private PlacasController placasController;                 //Controlador de placas
        [SerializeField] private CharacterController characterController;           //Controlador de personaje      

        public IState CurrentState { get; set; }                                    //Estado actual
        public LogController LogController { get { return logController; } }
        public PlacasController PlacasController { get { return placasController; } }                         
        public CharacterController CharacterController { get { return characterController; } }

        private void Start()
        {
            CurrentState = new GameInitializationState(this);
            LogController.PrintInConsole("Current State - "+CurrentState);
        }

        // Update is called once per frame
        void Update()
        {
            if (CurrentState.IsRunning())
            {
                StartCoroutine(CurrentState.StartState());
            }

            //Turno del game controller
            /*if (characterController.CharacterStarted)
            {
                logController.PrintInConsole("GameController: Ha comenzado el recorrido del personaje - Esperando a que termine");
            }
            else
            {
                logController.PrintInConsole("GameController: El personaje aun no ha empezado");
            }

            if (characterController.CharacterEnded)
            {
                characterController.CharacterEnded = false;
                logController.PrintInConsole("GameController: El personaje ha terminado el recorrido");
                for (int i = 0; i < placasController.PosicionesPlacasRandom.Length; i++)
                {
                    placasController.PosicionesPlacasRandom[i].GetComponent<PlacaControl>().SetOriginalMaterialColor();
                }
            }*/
        }

        public void ChangeState(IState state)
        {
            CurrentState = state;
        }

    } //END CLASS
}

