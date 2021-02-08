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
        [SerializeField] private GameObject guiController;                                       //Controlador de GUI   
        private GUIController gui;
        public LogController LogController { get { return logController; } }                        //Controlador de Log        
        private LevelManager levelManager;                                                          //Level manager        

        /*
            Obtiene el levelManager para acceder a sus funciones
         */
        private void Start()
        {            
            gui = guiController.GetComponent<GUIController>();
            levelManager = GetComponent<LevelManager>();
            levelManager.logController = LogController;
            gui.ActualizarAciertosLevel(EstadisticasManager.instance.AciertosTotales);
            gui.ActualizarFallosLevel(EstadisticasManager.instance.FallosTotales);
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
            EstadisticasManager.instance.AciertosTotales++;
            gui.ActualizarAciertosLevel(EstadisticasManager.instance.AciertosTotales);
        }
        public void UpFallosTotales() 
        {            
            EstadisticasManager.instance.FallosTotales++;
            gui.ActualizarFallosLevel(EstadisticasManager.instance.FallosTotales);
        }
        #endregion
    }
}