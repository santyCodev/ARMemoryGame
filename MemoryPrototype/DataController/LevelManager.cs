using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Data
{
    public class LevelManager : MonoBehaviour
    {
        private const string CLASS_NAME = "LEVEL MANAGER";                                  //Constante con el nombre de la clase

        private const int MAX_RONDAS = 3;                                                           //Numero maximo de rondas
        private const int MAX_FALLOS = 2;                                                         //Numero de intentos antes de bajar de nivel
        private LogController logController;                                                        //Controlador de logs
        public int NumRonda { get; set; }                                                           //Numero de rondas en un turno
        public int NumLevel { get; set; }                                                            //Numero de fases del juego (cada fase son 3 rondas)
        public int NumFallos { get; set; }
        public int NumAciertos { get; set; }
        private void Start()
        {
            logController = GetComponent<DataController>().LogController;
            InitializationData();
        }

        private void InitializationData()
        {
            NumLevel = 1;
            SetLevel(" Inicializacion ronda y fase ");
        }

        public void UpLevel()
        {
            NumLevel++;
            SetLevel(" Subida de fase ");
        }

        public void DownLevel()
        {
            NumLevel--;
            SetLevel(" Bajada de level ");
        }        

        public void StayLevel()
        {
            SetLevel(" Sigue fallando, manteniendose en nivel inicial ");
        }

        private void SetLevel(string message)
        {
            logController.PrintInConsole(CLASS_NAME + message);
            NumRonda = 1;
            NumFallos = 0;
            NumAciertos = 0;
        }

        public bool IsSuperiorLevel()
        {
            logController.PrintInConsole(CLASS_NAME + " Check if superior level ");
            return NumRonda > MAX_RONDAS;
        }

        public bool HasPassedMaxIntentos()
        {
            logController.PrintInConsole(CLASS_NAME + " Check if passed max intentos ");
            return NumFallos > MAX_FALLOS;
        }

        public bool IsLevelOne()
        {
            return NumLevel == 1;
        }

        public void PrintData()
        {
            logController.PrintInConsole(CLASS_NAME + " Level: " + NumLevel);
            logController.PrintInConsole(CLASS_NAME + " Ronda: " + NumRonda);
            logController.PrintInConsole(CLASS_NAME + " Intentos: " + NumFallos);
        }
    }
}
