using MemoryPrototype.Gui;
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
        [SerializeField] private GameObject estadisticasManager;
        private EstadisticasManager estadisticas;
        public LogController LogController { get { return logController; } }                        //Controlador de Log        
        private LevelManager levelManager;                                                          //Level manager        

        /*
            Obtiene el levelManager para acceder a sus funciones
         */
        private void Start()
        {
            estadisticas = estadisticasManager.GetComponent<EstadisticasManager>();
            levelManager = GetComponent<LevelManager>();
            levelManager.logController = LogController;            
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
            estadisticas.AciertosTotales++;
            guiController.ActualizarDatosLevel(estadisticas.FallosTotales, estadisticas.AciertosTotales);
        }
        public void UpFallosTotales() 
        {
            estadisticas.FallosTotales++;
            guiController.ActualizarDatosLevel(estadisticas.FallosTotales, estadisticas.AciertosTotales);
        }
        #endregion
    }
}