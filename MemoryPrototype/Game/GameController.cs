﻿using System;
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
using MemoryPrototype.Data;
using MemoryPrototype.Gui;

namespace MemoryPrototype.Game 
{
    //Clase que actua como contexto
    public class GameController : MonoBehaviour    
    {
        private const string CLASS_NAME = "GAME CONTROLLER";                                        //Constante con el nombre de la clase

        [SerializeField] private LogController logController;                                       //Controlador de logs
        [SerializeField] private PlacasController placasController;                                 //Controlador de placas
        [SerializeField] private CharacterController characterController;                           //Controlador de personaje      
        [SerializeField] private PlayerController playerController;                                 //Controlador del jugador
        [SerializeField] private DataController dataController;                                     //Controlador de datos
        [SerializeField] private GUIController guiController;                                       //Controlador de GUI

        public IState CurrentState { get; set; }                                                    //Estado actual
        public LogController LogController { get { return logController; } }                        //Controlador de Log
        public PlacasController PlacasController { get { return placasController; } }               //Controlador de Placas
        public CharacterController CharacterController { get { return characterController; } }      //Controlador de Personaje
        public PlayerController PlayerController { get { return playerController; } }               //Controlador de Personaje            
        public DataController DataController { get { return dataController; } }                     //Controlador de datos            
        public GUIController GuiController { get { return guiController; } }                        //Controlador de datos

        #region Start, Update y cambio de estado

        /*  
           Dependiendo de la variable isExitInfo persistente:
            - si es true va directamente al juego desde la cuenta atras
            - si es false va a la pantalla de titulo
         */
        private void Start()
        {
            CurrentState = null;
            if (DataController.GetIsExitInfo()) { guiController.ActivarCuentaAtrasGame(); }
            else { guiController.ActivatePantallaTitle(); }
            DataController.SaveIsExitInfo(false);
        }

        /* Cuando el estado actual este inicializado, se ejecuta */
        void Update()
        {
            if (CurrentState != null)
            {
                if (CurrentState.Initialized()) { StartCoroutine(CurrentState.StartState()); }            
            }
        }

        /* Cambia de estado */
        public void ChangeState(IState state)
        {
            CurrentState = state;
        }
        #endregion
          
        #region Evento para activar primer estado
        /* Suscribe el evento de GUI CuentaAtrasEnd */
        private void OnEnable()
        {
            GUIController.OnCuentaAtrasTerminada += CuentaAtrasEnd;
        }

        /* Da de baja al evento de GUI CuentaAtrasEnd */
        private void OnDisable()
        {
            GUIController.OnCuentaAtrasTerminada -= CuentaAtrasEnd;
        }

        /* 
         * Evento llamado cuando termina la cuenta atras de GO
         *  - Asigna el estado de inicializacion para comenzar el juego
         */
        public void CuentaAtrasEnd()
        {
            guiController.StartLevelGame();
            CurrentState = new GameInitializationState(this);
            LogController.PrintInConsole(CLASS_NAME + " Inicializando estado - " + CurrentState);
        }
        #endregion

        #region Funciones llamadas desde el evento OnTargetFound y OnTargetLost
        public void OnImageTargetFound()
        {
            guiController.DesactivateSeccionPatron();
        }

        public void OnImageTargetLost()
        {
            guiController.ActivateSeccionPatron();
        }
        #endregion
    } //END CLASS
}

