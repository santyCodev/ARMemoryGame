using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Utils
{    public class InputControlUtils : MonoBehaviour
    {
        public bool InputControl()
        {
            #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
                 return ClickControlUnity();
            #elif UNITY_ANDROID
                 return TapControlAndroid();
            #endif
        }

        /* Envia un evento al GUIController para avanzar a la siguiente pantalla */
        private bool ClickControlUnity()
        {
            return Input.GetMouseButtonDown(0);            
        }

        /* Envia un evento al GUIController para avanzar a la siguiente pantalla */
        private bool TapControlAndroid()
        {
            return ((Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.magnitude < 1.2f));
        }
    }
}

