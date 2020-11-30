using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Data
{
    public class DataController : MonoBehaviour
    {
        private const string CLASS_NAME = "DATA CONTROLLER";                                        //Constante con el nombre de la clase
        
        [SerializeField] private LogController logController;                                       //Controlador de logs
        public LogController LogController { get { return logController; } }                        //Controlador de Log
        private LevelManager levelManager;                                                          //Level manager

        /*
            Obtiene el levelManager para acceder a sus funciones
         */
        private void Start()
        {
            levelManager = GetComponent<LevelManager>();
        }

        /*
            Checkea si se puede bajar o subir de nivel
         */
        public void CheckIfDownOrUpLevel()
        {
            if (levelManager.IsSuperiorLevel()) { levelManager.UpLevel(); }
            else if (levelManager.HasPassedMaxIntentos())
            {
                if (!levelManager.IsLevelOne()) { levelManager.DownLevel(); }
                else { levelManager.StayLevel(); }                 
            }
            levelManager.PrintData();            
        }

        public int GetLevel() { return levelManager.NumLevel; }
        /*
            Sube el numero de rondas
         */
        public void UpRonda() { levelManager.NumRonda++; }

        public void UpFallos() { levelManager.NumFallos++; }

        public void UpAciertos() { levelManager.NumAciertos++; }
        
    }
}