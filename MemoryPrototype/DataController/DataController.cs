using MemoryPrototype.Logs;
using MemoryPrototype.Placas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Data
{
    public class DataController : MonoBehaviour
    {
        private const string CLASS_NAME = "DATA CONTROLLER";                                        //Constante con el nombre de la clase

        [SerializeField] private LogController logController;                                       //Controlador de logs
        [SerializeField] private GUIController guiController;                                       //Controlador de GUI
        public LogController LogController { get { return logController; } }                        //Controlador de Log
        
        private LevelManager levelManager;                                                          //Level manager

        /*
            Obtiene el levelManager para acceder a sus funciones
         */
        private void Start()
        {
            levelManager = GetComponent<LevelManager>();
            levelManager.logController = LogController;
            guiController.ActualizarDatosLevel(levelManager.NumFallos, levelManager.NumAciertos, GetActualRonda(), GetActualLevel());
        }

        public int GetActualLevel() { return levelManager.ActualLevel; }

        public int GetBeforeLevel() { return levelManager.BeforeLevel; }

        public int GetActualRonda() { return levelManager.NumRonda; }

        public void ResetLevelOne() 
        { 
            levelManager.ResetLevelOne();
            guiController.ActualizarDatosLevel(levelManager.NumFallos, levelManager.NumAciertos, GetActualRonda(), GetActualLevel());
        }

        public void UpLevel()
        {
            levelManager.UpLevel();
            guiController.ActualizarDatosLevel(levelManager.NumFallos, levelManager.NumAciertos, GetActualRonda(), GetActualLevel() );
        }

        public void DownLevel() 
        { 
            levelManager.DownLevel();
            guiController.ActualizarDatosLevel(levelManager.NumFallos, levelManager.NumAciertos, GetActualRonda(), GetActualLevel());
        }

        public void UpAcierto() 
        { 
            levelManager.UpAcierto();
            guiController.ActualizarDatosLevel(levelManager.NumFallos, levelManager.NumAciertos, GetActualRonda(), GetActualLevel());
        }

        public void UpRonda() 
        { 
            levelManager.UpRonda();
            guiController.ActualizarDatosLevel(levelManager.NumFallos, levelManager.NumAciertos, GetActualRonda(), GetActualLevel());
        }

        public void UpFallo() 
        { 
            levelManager.UpFallo();
            guiController.ActualizarDatosLevel(levelManager.NumFallos, levelManager.NumAciertos, GetActualRonda(), GetActualLevel());
        }

        public bool IsMaxAciertos(int numPlacas) { return levelManager.IsMaxAciertos(numPlacas); }

        public bool IsMaxRondas() { return levelManager.IsMaxRondas(); }

        public bool IsMaxFallos() { return levelManager.IsMaxFallos(); }
    }
}