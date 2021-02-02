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
        private EstadisticasManager estadisticasManager;

        /*
            Obtiene el levelManager para acceder a sus funciones
         */
        private void Start()
        {
            levelManager = GetComponent<LevelManager>();
            levelManager.logController = LogController;
            estadisticasManager = GetComponent<EstadisticasManager>();
            guiController.ActualizarDatosLevel(estadisticasManager.FallosTotales, estadisticasManager.AciertosTotales);
        }

        #region Funciones level manager
        public int GetActualLevel() { return levelManager.ActualLevel; }

        public int GetBeforeLevel() { return levelManager.BeforeLevel; }

        public int GetActualRonda() { return levelManager.NumRonda; }

        public void ResetLevelOne() { levelManager.ResetLevelOne(); }

        public void UpLevel() { levelManager.UpLevel(); }

        public void DownLevel() { levelManager.DownLevel(); }

        public void UpAcierto() { levelManager.UpAcierto(); }

        public void UpRonda() { levelManager.UpRonda(); }

        public void UpFallo() { levelManager.UpFallo(); }

        public bool IsMaxAciertos(int numPlacas) { return levelManager.IsMaxAciertos(numPlacas); }

        public bool IsMaxRondas() { return levelManager.IsMaxRondas(); }

        public bool IsMaxFallos() { return levelManager.IsMaxFallos(); }
        #endregion

        #region Funciones estadisticas manager
        public void UpAciertosTotales() 
        { 
            estadisticasManager.AciertosTotales++;
            guiController.ActualizarDatosLevel(estadisticasManager.FallosTotales, estadisticasManager.AciertosTotales);
        }
        public void UpFallosTotales() 
        { 
            estadisticasManager.FallosTotales++;
            guiController.ActualizarDatosLevel(estadisticasManager.FallosTotales, estadisticasManager.AciertosTotales);
        }
        #endregion
    }
}