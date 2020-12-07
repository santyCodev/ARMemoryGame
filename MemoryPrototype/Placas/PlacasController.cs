using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Placas
{
    public class PlacasController : MonoBehaviour
    {
        [SerializeField] private LogController logController;       //Controlador de logs
        [SerializeField] private GameObject placasParent;           //parent de las placas

        private const string CLASS_NAME = "PLACAS CONTROLLER";      //Constante nombre de clase
        private const int NUM_PLACAS_INITIAL = 3;                   //Numero de placas inicial
        private const string DEFAULT_TAG = "Plate";                 //Tag inicial para las placas
        private const string MARKED_TAG = "PlateMarked";            //Tag para las placas elegidas como random

        private Transform[] placas;                                 //Array que contendra a todas las placas        
        public int NumPlacasRandom { get; set; }                    //Numero de placas random a recoger

        public List<GameObject> placasRandom;                        //Array con las placas random elegidas

        #region Inicializacion de placas

        /*
            Inicializa los datos para comenzar con el nivel 1 del juego
            - Recoge el array de placas desde el gameObject parent
            - A todas las placas les asigna el tag por defecto
            - Instancia el array de placas random
            - Asigna al array las placas elegidas aleatoriamente
            - Asigna a las placas random el tag correspondiente
         */
        private void Awake()
        {
            PrintMessage(" PlacasInitialization - Awake() - ENTER");
            SetNumPlacasRandom();
            placas = GetPlacasFromParent();              
            SetDefaultTags();
            InitializePlacasRandom();
            PrintMessage(" PlacasInitialization - Awake() - DONE");
        }

        /* 
            Recoge las placas que son hijas del parent como un array de Transforms
         */
        private Transform[] GetPlacasFromParent()
        {
            PrintMessage(" GetPlacasFromParent() - ENTER ");
            PrintMessage(" GetPlacasFromParent() - DONE ");
            return placasParent.GetComponentsInChildren<Transform>();
        }

        /*
            Recorre el array de placas y asigna en cada una de ellas el tag default
         */
        private void SetDefaultTags()
        {
            PrintMessage(" SetDefaultTags() - ENTER ");
            foreach (var placa in placas) { placa.gameObject.tag = DEFAULT_TAG; }
            PrintMessage(" SetDefaultTags() - DONE ");
        }

        /*
           Esta funcion inicializa la lista de placaas random
            - Instancia la lista de placas
            - Asigna el numero de placas random a recoger
            - Recoge el numero random de placas
            - Marca las placas random con el tag de marcado
         */
        public void InitializePlacasRandom()
        {
            PrintMessage(" InitializePlacasRandom() - ENTER ");
            InstantiatePlacasRandom();            
            SetRandomPlacas();
            SetMarkedTag();
            PrintMessage(" InitializePlacasRandom() - DONE ");
        }        

        /*
            Se instancia el una lista que contendra las Placas 
         */
        private void InstantiatePlacasRandom()
        {
            PrintMessage(" InstantiatePlacasRandom() - ENTER");
            placasRandom = new List<GameObject>();
            PrintMessage(" InstantiatePlacasRandom() - DONE");
        }

        public void SetNumPlacasRandom()
        {
            PrintMessage(" InstantiatePlacasRandom() - ENTER");
            SetNumPlacasRandom(NUM_PLACAS_INITIAL);
            PrintMessage(" InstantiatePlacasRandom() - DONE");
        }

        public void SetNumPlacasRandom(int numPlacas)
        {
            PrintMessage(" InstantiatePlacasRandom(" + numPlacas + ") - ENTER");
            NumPlacasRandom = numPlacas;
            PrintMessage(" InstantiatePlacasRandom(" + numPlacas + ") - DONE");
        }

        #endregion

        #region Gestion de Tags
        /*
            Asigna la tag MARKED_TAG a las placas random
         */
        public void SetMarkedTag()
        {
            PrintMessage(" SetMarkedTag() - ENTER");
            SetRandomTag(MARKED_TAG);
            PrintMessage(" SetMarkedTag() - DONE");
        }

        /*
            Asigna de nuevo la DEFAULT_TAG a las placas que fueron cambiadas antes por la MARKED_TAG
         */
        public void SetRandomTagsToDefault()
        {
            SetRandomTag(DEFAULT_TAG);
            PrintMessage(" SetRandomTagsToDefault() - DONE");
        }

        /*
            Asigna a las placas random el tagType, que puede ser DEFAULT o MARKED
         */
        private void SetRandomTag(string tagType)
        {
            PrintMessage(" SetRandomTag(" + tagType + ") - ENTER");
            foreach (var placa in placasRandom) { placa.tag = tagType; }
            PrintMessage(" SetRandomTag(" + tagType + ") - DONE");
        }

        /*
            Comprueba si las placas random tienen el tag MARKED
         */
        public bool CheckIfMarkedTag() 
        {
            int countPlacas = 0;
            foreach (var placa in placasRandom) { if (placa.CompareTag(MARKED_TAG)) {countPlacas++;} }
            PrintMessage(" CheckIfMarkedTag() - num marked tags = " + countPlacas + " - DONE");
            return countPlacas == placasRandom.Count;
        }
        #endregion
        
        #region Eleccion de placas aleatorias

        /*
            Elige de manera aleaoria un numero de placas del tablero
            - Se asignaran tantas placas como valor tengamos en NumPlacasRandom
            - Con el valor index, controlamos que no sobrepasemos NumPlacasRandom
            - Dos placas elegidas no pueden ser iguales
         */
        public void SetRandomPlacas()
        {
            PrintMessage(" SetRandomPlacas() - ENTER");
            int index = 0;
            Transform actualPlacaRandom;
            GameObject lastPlacaRamdom = null;

            PrintMessage(" SetRandomPlacas() - index = "+index+", NumPlacasRandom = "+NumPlacasRandom);
            while (index < NumPlacasRandom)
            {
                actualPlacaRandom = GetPlacaRandom();
                PrintMessage(" SetRandomPlacas() - Posicion placa actual: " + actualPlacaRandom.transform.position);                
                if (logController.LogsActive) { PrintLastPlacaRandom(lastPlacaRamdom); }
                index = CompareIfPlacasEquals(index, actualPlacaRandom, lastPlacaRamdom);
                lastPlacaRamdom = actualPlacaRandom.gameObject;
                PrintMessage(" SetRandomPlacas() - index = " + index + ", NumPlacasRandom = " + NumPlacasRandom);
            }

            if (logController.LogsActive) { PrintPlacasRandom(); }
            PrintMessage(" SetRandomPlacas() - DONE");
        }

        /*
            Devuelve desde el array de placas principal una placa de una posicion random
         */
        private Transform GetPlacaRandom()
        {
            PrintMessage(" GetPlacaRandom() - ENTER");
            int randomPosition;
            randomPosition = UnityEngine.Random.Range(1, placas.Length);            
            PrintMessage(" GetPlacaRandom() - DONE");
            return placas[randomPosition];
        }

        /*
            Compara la placa anterior con la actual si no son iguales
            - La placa actual se guarda en el array de placas random
            - Despues se asigna la actual a la anterior
            - Sumamos index++ para seguir asignando placas
         */
        private int CompareIfPlacasEquals(int index, Transform actualPlacaRandom, GameObject lastPlacaRandom)
        {
            
            PrintMessage(" CompareIfPlacasEquals() - ENTER");
            if (lastPlacaRandom == null || !actualPlacaRandom.Equals(lastPlacaRandom.transform))
            {
                PrintMessage(" CompareIfPlacasEquals() - Las placas no son iguales");
                placasRandom.Add(actualPlacaRandom.gameObject);
                index++;
            }
            else { PrintMessage(" CompareIfPlacasEquals() - Las placas son iguales"); }

            PrintMessage(" CompareIfPlacasEquals() - DONE");
            return index;
        }
        #endregion

        #region Placas Material Color

        /*
            Devuelve a las placas aleatorias su color original
         */
        public void SetOriginalMaterialColor()
        {            
            foreach (var placa in placasRandom) { placa.GetComponent<PlacaControl>().SetOriginalMaterialColor(); }
            PrintMessage(" SetOriginalMaterialColor() - DONE");
        }

        #endregion

        #region gestion de logs
        /*
            Imprime en consola las placas / posiciones random despues completar la eleccion
         */
        private void PrintPlacasRandom()
        {
            string stringToPrint = "";
            foreach (var placa in placasRandom)
            {
                stringToPrint += "Posicion random: " + placa.transform.position + "\n";
            }
            PrintMessage(stringToPrint);
        }

        /*
            Imprime en consola la placa anterior
         */
        private void PrintLastPlacaRandom(GameObject lastPlacaRamdom)
        {
            PrintMessage(" PrintLastPlacaRandom() - ENTER");
            string stringToPrint = " PrintLastPlacaRandom() - "; 
            if (lastPlacaRamdom == null) { stringToPrint += " Posicion placa anterior: null"; }
            else { stringToPrint += " Posicion placa anterior: " + lastPlacaRamdom.transform.position; }
            PrintMessage(stringToPrint);
            PrintMessage(" PrintLastPlacaRandom() - DONE");
        }        

        /*
            Usa el controlador de logs para imprimir un mensaje en consola
         */
        private void PrintMessage(string message)
        {
            logController.PrintInConsole(CLASS_NAME + message);
        }
        #endregion
    }//END CLASS
}

