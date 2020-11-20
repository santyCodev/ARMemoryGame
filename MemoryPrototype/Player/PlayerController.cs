using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Player
{
    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            #if UNITY_EDITOR
                //This is for Unity Testing
                UpdateLabelNameUnity();
            
            #elif UNITY_ANDROID
                //This is for Android Devices
                UpdateLabelNameAndroid();
            
            #endif
        }

        /**
        - Recoge boton izquierdo del raton presionado
        - Crea el rayo desde la posicion del raton
        - Evalua si el rayo a colisionado con algun objeto en la variable hit
        - Si es asi, activa el label y escribe el nombre del objeto en el hit
        */
        private void UpdateLabelNameUnity()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                SetLabelNameForRaycastHit(ray);
            }
        }

        /**
        - Recoge el objeto touch del dedo en la pantalla
        - Crea el rayo desde la posicion del dedo
        - Evalua si el rayo a colisionado con algun objeto en la variable hit
        - Si es asi, activa el label y escribe el nombre del objeto en el hit
    */
        private void UpdateLabelNameAndroid()
        {

            if ((Input.GetTouch(0).phase == TouchPhase.Stationary) ||
               (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.magnitude < 1.2f))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                SetLabelNameForRaycastHit(ray);
            }
        }

        /**
            Evalua si el rayo a colisionado con algun objeto en la variable hit
            
        */
        private void SetLabelNameForRaycastHit(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Raycast colisionado con "+hit.transform.name);
            }
            else
            {
                Debug.Log("Raycast no colisionado");
            }
        }
    }
}

