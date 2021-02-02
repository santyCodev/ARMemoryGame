using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LogController logController;       //Controlador de logs

        private const string CLASS_NAME = "PLAYER CONTROLLER";      //Constante con el nombre de la clase
        public bool StartExecute { get; set; }                      //Indica que el player comienza a jugar
        public delegate void PlacaSelected(GameObject placa);       //Delegado para el evento
        public static event PlacaSelected OnPlacaClicked;           //Evento para avisar que el player ha hecho click en una placa

        #region Inicializacion
        /*
            Al tener StartExecute a false, indicamos al jugador
                que aun no puede jugar.
         */
        void Awake() { StartExecute = false; }
        #endregion

        #region Espera a jugar
        /*
            Aqui el jugador espera hasta que el GameMovementState
                le de paso para jugar, asi ya tiene todo listo
         */
        void Update() { LetPlayerClick(); }
        #endregion

        #region funcionalidad raycasting
        /*
            Funcion que se llama en el turno del jugador
            - Se llama a una funcion dependiendo del sistema
                escritorio o android
            - La funcion llamada dejara que el jugador eliga
                una placa haciendo click (desktop) o con el touch
                (android)
         */
        private void LetPlayerClick()
        {
            #if UNITY_EDITOR                  
                //This is for Unity Testing
                ClickPointRaycastUnity();

            #elif UNITY_ANDROID 
                //This is for Android Devices
                ClickPointRaycastAndroid();
            
            #endif
        }

        /**
            - Recoge boton izquierdo del raton presionado
            - Recoge el rayo desde la posicion del raton            - 
        */
        private void ClickPointRaycastUnity()
        {            
            if (Input.GetMouseButtonDown(0) && StartExecute)
            {
                StartExecute = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                
                GetPlacaForRaycastHit(ray);
            }
        }

        /**
            - Recoge el objeto touch del dedo en la pantalla
            - Crea el rayo desde la posicion del dedo
        */
        private void ClickPointRaycastAndroid()
        {

            if (StartExecute && 
                ((Input.GetTouch(0).phase == TouchPhase.Stationary) ||
                 (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.magnitude < 1.2f)))
            {
                StartExecute = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                GetPlacaForRaycastHit(ray);
            }
        }

        /**
            Evalua si el rayo a colisionado con algun objeto en la variable hit
                - Al colisionar con una placa, llama al evento OnPlacaClicked, enviandole
                    la placa al GameMovementState para comprobar si es correcto
        */
        private void GetPlacaForRaycastHit(Ray ray)
        {            
            RaycastHit hit;
            PrintMessage(" GetPlacaForRaycastHit() - ENTER");            
            if (Physics.Raycast(ray, out hit)) 
            {
                PrintMessage(" GetPlacaForRaycastHit() - Placa seleccionada: " + hit.transform.position);
                OnPlacaClicked(hit.transform.gameObject);
            }
            else { PrintMessage(" - Raycast no colisionado"); }
            PrintMessage(" GetPlacaForRaycastHit() - DONE");
        }
        #endregion

        #region Gestion de Logs
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