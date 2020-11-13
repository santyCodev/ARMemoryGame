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
            if (CurrentState.Initialized())
            {
                StartCoroutine(CurrentState.StartState());
            }
        }

        public void ChangeState(IState state)
        {
            CurrentState = state;
        }

    } //END CLASS
}

