using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Player
{
    public class PlayerController : MonoBehaviour
    {
        public bool StartExecute { get; set; }                      //Indica que el player comienza a jugar
        public delegate bool PlacaSelected(GameObject placa);                       //Delegado para el evento
        public static event PlacaSelected OnPlacaClicked;           //Evento para avisar que el player ha hecho click en una placa


        // Start is called before the first frame update
        void awake()
        {
            StartExecute = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (StartExecute) { LetPlayerClick(); }
        }

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
        - Crea el rayo desde la posicion del raton
        - Evalua si el rayo a colisionado con algun objeto en la variable hit
        - Si es asi, activa el label y escribe el nombre del objeto en el hit
        */
        private void ClickPointRaycastUnity()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                GetPlacaForRaycastHit(ray);
            }
        }

        /**
        - Recoge el objeto touch del dedo en la pantalla
        - Crea el rayo desde la posicion del dedo
        - Evalua si el rayo a colisionado con algun objeto en la variable hit
        - Si es asi, activa el label y escribe el nombre del objeto en el hit
    */
        private void ClickPointRaycastAndroid()
        {

            if ((Input.GetTouch(0).phase == TouchPhase.Stationary) ||
               (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.magnitude < 1.2f))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                GetPlacaForRaycastHit(ray);
            }
        }

        /**
            Evalua si el rayo a colisionado con algun objeto en la variable hit
            
        */
        private void GetPlacaForRaycastHit(Ray ray)
        {            
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))            {

                StartExecute = OnPlacaClicked(hit.transform.gameObject);
            }
            else
            {
                Debug.Log("Raycast no colisionado");
            }
        }
    }
}

