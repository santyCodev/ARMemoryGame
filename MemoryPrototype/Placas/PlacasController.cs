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
        public GameObject[] PlacasRandom { get; set; }              //Array con las placas random elegidas

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
            placas = GetPlacasFromParent();              
            SetDefaultTags();
            InitializePlacasRandom();
            SetRandomPlacas();
            SetMarkedTag();
            PrintMessage(" PlacasInitialization - Awake() - DONE");
        }

        /* 
            Recoge las placas que son hijas del parent como un array de Transforms
         */
        private Transform[] GetPlacasFromParent()
        {
            PrintMessage(" GetPlacasFromParent() - DONE ");
            return placasParent.GetComponentsInChildren<Transform>();
        }

        /*
            Recorre el array de placas y asigna en cada una de ellas el tag default
         */
        private void SetDefaultTags()
        {
            foreach (var placa in placas) { placa.gameObject.tag = DEFAULT_TAG; }
            PrintMessage(" SetDefaultTags() - DONE ");
        }

        /*
           Esta funcion inicializa por primera vez el array de placas random con el
            valor inicial dado por la constante NUM_PLACAS_INITIAL            
         */
        public void InitializePlacasRandom()
        {
            InitializePlacasRandom(NUM_PLACAS_INITIAL);
            PrintMessage(" InitializePlacasRandom(NUM_PLACAS_INITIAL) - DONE ");
        }

        /*
            Metodo sobrecargado para inicializar las placas random con un valor que
                pasa como parametro, para niveles donde hay mas numero de placas
         */
        public void InitializePlacasRandom(int numPlacas)
        {
            NumPlacasRandom = numPlacas;
            InstantiatePlacasRandom(NumPlacasRandom);
            PrintMessage(" InitializePlacasRandom(" + numPlacas + ") - DONE ");
        }

        /*
            Se instancia el array de placas random con el numero de placas que contendra 
         */
        private void InstantiatePlacasRandom(int numPlacas)
        {
            PlacasRandom = new GameObject[numPlacas];
            PrintMessage(" InstantiatePlacasRandom(" + numPlacas + ") - DONE");
        }

        #endregion

        #region Gestion de Tags
        /*
            Asigna la tag MARKED_TAG a las placas random
         */
        public void SetMarkedTag()
        {
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
            foreach (var placa in PlacasRandom) { placa.tag = tagType; }
            PrintMessage(" SetRandomTag(" + tagType + ") - DONE");
        }

        /*
            Comprueba si las placas random tienen el tag MARKED
         */
        public bool CheckIfMarkedTag() 
        {
            int countPlacas = 0;
            foreach (var placa in PlacasRandom) { if (placa.CompareTag(MARKED_TAG)) {countPlacas++;} }
            PrintMessage(" CheckIfMarkedTag() - num marked tags = " + countPlacas + " - DONE");
            return countPlacas == PlacasRandom.Length;
        }
        #endregion
        
        #region Eleccion de placas aleatorias

        /*
            Elige de manera aleaoria un numero de placas del tablero
            - Se asignaran tantas placas como valor tengamos en NumPlacasRandom
            - Con el valor index, controlamos que no sobrepasemos NumPlacasRandom
         */
        public void SetRandomPlacas()
        {            
            int index = 0;
            Transform actualPlacaRandom;
            GameObject lastPlacaRamdom = null;

            while (index < NumPlacasRandom)
            {
                actualPlacaRandom = GetPlacaRandom();
                PrintMessage(" SetRandomPlacas() - Posicion placa actual: " + actualPlacaRandom.transform.position);
                if (logController.LogsActive) { PrintLastPlacaRandom(lastPlacaRamdom); }
                index = CompareIfPlacasEquals(index, actualPlacaRandom, lastPlacaRamdom);                
            }

            if (logController.LogsActive) { PrintPlacasRandom(); }
            PrintMessage(" SetRandomPlacas() - DONE");
        }

        /*
            Devuelve desde las placas random devuelve una placa aleatoria
         */
        private Transform GetPlacaRandom()
        {
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
            if (lastPlacaRandom == null || !actualPlacaRandom.Equals(lastPlacaRandom.transform))
            {
                PrintMessage(" CompareIfPlacasEquals() - Placa actual = " + actualPlacaRandom.position+ "- Placa anterior = " + lastPlacaRandom.transform.position+ " - No son iguales");
                PlacasRandom[index] = actualPlacaRandom.gameObject;
                lastPlacaRandom = actualPlacaRandom.gameObject;
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
            foreach (var placa in PlacasRandom) { placa.GetComponent<PlacaControl>().SetOriginalMaterialColor(); }
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
            for (int i = 0; i < PlacasRandom.Length; i++)
            {
                stringToPrint += "Posicion random: " + PlacasRandom[i].transform.position + "\n";
            }
            PrintMessage(stringToPrint);
        }

        /*
            Imprime en consola la placa anterior
         */
        private void PrintLastPlacaRandom(GameObject lastPlacaRamdom)
        {
            string stringToPrint = "";
            if (lastPlacaRamdom == null) { stringToPrint = "Posicion placa anterior: null"; }
            else { stringToPrint = "Posicion placa anterior: " + lastPlacaRamdom.transform.position; }
            PrintMessage(stringToPrint);
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

