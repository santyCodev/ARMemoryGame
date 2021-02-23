using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace MemoryPrototype.Logs
{
    public class LogController : MonoBehaviour
    {
        //Para activar/desactivar logs desde el editor
        [SerializeField] private bool logsActive;

        //Get y Set del atributo logsActive
        public bool LogsActive
        {
            get { return logsActive; }
            set { logsActive = value; }
        }

        /* 
            Escribe un mensaje en consola
            - La condicion para escribir es que este activado
                el atributo logsActive desde el editor
         */
        public void PrintInConsole(string message)
        {
            if (LogsActive) { Debug.Log(message); }
        }
    }
}

