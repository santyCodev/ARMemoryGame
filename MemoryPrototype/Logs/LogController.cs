using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using System.IO;

namespace MemoryPrototype.Logs
{
    public class LogController : MonoBehaviour
    {
        //Para activar/desactivar logs desde el editor
        [SerializeField] private bool logsActive;

        string path = "Assets/Logs/test.txt";

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
            if (LogsActive) {

                //Write some text to the test.txt file
                StreamWriter writer = new StreamWriter(path, true);
                writer.WriteLine(message);
                writer.Close();

                //Re-import the file to update the reference in the editor
                UnityEditor.AssetDatabase.ImportAsset(path);
                TextAsset asset = (TextAsset)Resources.Load("test");

                Debug.Log(message); 
            }
        }
    }
}

