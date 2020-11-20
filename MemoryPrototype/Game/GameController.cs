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
using MemoryPrototype.Player;

namespace MemoryPrototype.Game 
{
    //Clase que actua como contexto
    public class GameController : MonoBehaviour    
    {
        [SerializeField] private LogController logController;                                       //Controlador de logs
        [SerializeField] private PlacasController placasController;                                 //Controlador de placas
        [SerializeField] private CharacterController characterController;                           //Controlador de personaje      
        [SerializeField] private PlayerController playerController;                                 //Controlador del jugador
        public IState CurrentState { get; set; }                                                    //Estado actual
        public LogController LogController { get { return logController; } }                        //Controlador de Log
        public PlacasController PlacasController { get { return placasController; } }               //Controlador de Placas
        public CharacterController CharacterController { get { return characterController; } }      //Controlador de Personaje
        public PlayerController PlayerController { get { return playerController; } }      //Controlador de Personaje

        /*
            Asigna el primer estado del juego
         */
        private void Start()
        {
            CurrentState = new GameInitializationState(this);
            LogController.PrintInConsole("Current State - "+CurrentState);
        }

        /*
            Cuando el estado actual este inicializado, se ejecuta
         */
        void Update()
        {
            if (CurrentState.Initialized()){ StartCoroutine(CurrentState.StartState()); }
        }

        /*
            Cambia de estado
         */
        public void ChangeState(IState state)
        {
            CurrentState = state;
        }

    } //END CLASS
}

