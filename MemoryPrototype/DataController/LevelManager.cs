using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Data
{
    public class LevelManager : MonoBehaviour
    {
        private const string CLASS_NAME = "LEVEL MANAGER";      //Constante con el nombre de la clase

        private const int MAX_RONDAS = 3;                       //Numero maximo de rondas
        private const int MAX_FALLOS = 3;                       //Numero maximo de fallos
        
        private LogController logController;                    //Controlador de logs
        private int NumRonda { get; set; }                      //Numero de rondas en un nivel
        public int ActualLevel { get; set; }                    //Nivel Actual del juego
        public int BeforeLevel { get; set; }                    //Nivel anterior del juego
        private int NumFallos { get; set; }                     //Numero de aciertos
        private int NumAciertos { get; set; }                   //Numero de fallos

        #region Inicializacion de datos

        private void Start()
        {
            logController = GetComponent<DataController>().LogController;
            InitializationData();
        }

        private void InitializationData()
        {
            ActualLevel = 1;
            BeforeLevel = ActualLevel;
            SetLevel(" Inicializacion de datos: ");
            PrintData();
        }

        #endregion

        #region Gestion de datos
        /*
            Aumenta en 1 el numero de aciertos
         */
        public void UpAcierto() 
        {
            NumAciertos++;
            PrintData("El jugador ha acertado: ");
        }

        /*
            Aumenta en 1 el numero de fallos
            - Cuando aumenta un fallo se reinician los aciertos
         */
        public void UpFallo()
        {
            NumFallos++;
            NumAciertos = 0;
            PrintData("El jugador ha fallado: ");
        }

        /*
            Aumenta en 1 el numero de rondas
            - Cuando se sube de ronda se reinician los aciertos
         */
        public void UpRonda()
        {
            NumRonda++;
            NumAciertos = 0;
            PrintData("Subida de ronda: ");
        }

        /*
            Aumenta el nivel de juego
            - Cuando se sube de nivel, se reinician los demas datos
         */
        public void UpLevel()
        {
            BeforeLevel = ActualLevel;
            ActualLevel++;
            SetLevel(" Subida de nivel: ");
        }

        /*
            Bajada de nivel
            - Cuando baja el nivel, se reinician los demas datos
         */
        public void DownLevel()
        {
            BeforeLevel = ActualLevel;
            ActualLevel--;            
            SetLevel(" Bajada de nivel: ");            
        }        

        /*
            
         */
        private void SetLevel(string message)
        {            
            NumRonda = 1;
            NumFallos = 0;
            NumAciertos = 0;
            PrintData(message);
        }

        #endregion

        #region Comprobacion de datos
        /*
            Comprueba si ha llegado al maximo de fallos
         */
        public bool IsMaxFallos()
        {
            PrintMessage(" Check if max fallos: "+ (NumFallos == MAX_FALLOS));
            return NumFallos == MAX_FALLOS;
        }

        /*
            Comprueba si ha llegado al maximo de aciertos
            - El numero de aciertos viene dado por el numero
                de placas de ese nivel actual
         */
        public bool IsMaxAciertos(int maxAciertos)
        {
            PrintMessage(" Check if max aciertos: " + (NumAciertos == maxAciertos));
            return NumAciertos == maxAciertos;
        }

        /*
            Comprueba si ha llegado al maximo de rondas
         */
        public bool IsMaxRondas()
        {
            PrintMessage(" Check if max rondas: " + (NumRonda == MAX_RONDAS));
            return NumRonda == MAX_RONDAS;
        }
        #endregion

        #region Gestion de logs

        /*
            Imprime las propiedades por consola
         */
        private void PrintData(string message = null)
        {
            string stringToPrint = "";

            if(message != null) { stringToPrint = message; }

            PrintMessage(stringToPrint + "\n" +
                            " Level : " + ActualLevel + "\n" +
                            " Ronda : " + NumRonda + "\n" +
                            " Aciertos : " + NumAciertos + "\n" +
                            " Fallos : " + NumFallos);
        }

        /*
            Usa el controlador de logs para imprimir un mensaje en consola
         */
        private void PrintMessage(string message) 
        {
            logController.PrintInConsole(CLASS_NAME + message);
        }
        #endregion
    }
}
