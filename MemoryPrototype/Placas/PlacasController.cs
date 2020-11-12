using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Placas
{
    public class PlacasController : MonoBehaviour
    {
        private const int NUM_PLACAS_INITIAL = 3;
        private const string DEFAULT_TAG = "Plate";
        private const string MARKED_TAG = "PlateMarked";

        private Transform[] placas;                         //Array que contendra a todas las placas
        private GameObject[] posicionesPlacasRandom;        //Array con las placas random elegidas
        private int NumPlacasRandom { get; set; }           //Numero de placas random a recoger
        
        public LogController logController;                 //Controlador de logs
        public GameObject placasParent;                     //parent de las placas       

        

        private void Awake()
        {            
            placas = placasParent.GetComponentsInChildren<Transform>();          //Recoge todas las placas hijas    
            SetDefaultTags();
            InitializePlacasRandom();
            GetRandomPlacas();
            setMarkedTag();
        }

        void Start()
        {
                                                      
        }

        #region SetTags

        /*
            Recorre el array de placas y asigna en cada una de ellas el tag default
         */
        private void SetDefaultTags()
        {
            foreach (var placa in placas) { placa.gameObject.tag = DEFAULT_TAG; }
        }

        public void setMarkedTag()
        {
            setRandomTag(MARKED_TAG);
        }

        public void setRandomTagsToDefault()
        {
            setRandomTag(DEFAULT_TAG);            
        }

        private void setRandomTag(string tagType)
        {
            foreach (var placa in posicionesPlacasRandom) { placa.tag = tagType; }
        }

        #endregion

        #region InitializePlacas

        /*
            Se instancia el array de placas random con el numero de placas que contendra 
         */
        public void InstantiatePlacasRandom(int numPlacas)
        {
            posicionesPlacasRandom = new GameObject[numPlacas];
        }

        /*
           Esta funcion inicializa por primera vez el array de placas random con el
            valor inicial dado por la constante NUM_PLACAS_INITIAL            
         */
        private void InitializePlacasRandom()
        {
            NumPlacasRandom = NUM_PLACAS_INITIAL;
            InstantiatePlacasRandom(NUM_PLACAS_INITIAL);
        }
        
        #endregion

        #region GetRandomPlacas

        /*
            Elige de manera aleaoria un numero de placas del tablero
            - Se asignaran tantas placas como valor tengamos en NumPlacasRandom
            - Con el valor index, controlamos que no sobrepasemos NumPlacasRandom
         */
        public void GetRandomPlacas()
        {            
            int index = 0;
            Transform actualPlacaRandom;
            GameObject lastPlacaRamdom = null;

            while (index < NumPlacasRandom)
            {
                actualPlacaRandom = GetActualPlacaRandom();
                logController.PrintInConsole("Posicion placa actual: " + actualPlacaRandom.transform.position);

                if (logController.LogsActive) { PrintLastPlacaRandom(lastPlacaRamdom); }

                index = CompareIfPlacasEquals(index, actualPlacaRandom, lastPlacaRamdom);                
            }

            if (logController.LogsActive) { PrintPlacasRandom(); }
        }

        /*
            Devuelve una placa aleatoria como actual desde el array de placas
         */
        private Transform GetActualPlacaRandom()
        {
            int randomPosition;
            randomPosition = UnityEngine.Random.Range(1, placas.Length);
            return placas[randomPosition];
        }

        /*
            Imprime en consola la placa anterior
         */
        private void PrintLastPlacaRandom(GameObject lastPlacaRamdom)
        {
            if (lastPlacaRamdom == null) { logController.PrintInConsole("Posicion placa anterior: null"); }
            else { logController.PrintInConsole("Posicion placa anterior: " + lastPlacaRamdom.transform.position); }
        }

        /*
            Compara la placa anterior conla actual si no son iguales
            - La placa actual se guarda en el array de placas random
            - Despues se asigna la actual a la anterior
            - Sumamos index++ para seguir asignando placas
         */
        private int CompareIfPlacasEquals(int index, Transform actualPlacaRandom, GameObject lastPlacaRamdom)
        {
            if (lastPlacaRamdom == null || !actualPlacaRandom.Equals(lastPlacaRamdom.transform))
            {
                logController.PrintInConsole("Las Posiciones de las placas no son iguales");
                posicionesPlacasRandom[index] = actualPlacaRandom.gameObject;
                lastPlacaRamdom = actualPlacaRandom.gameObject;
                index++;
            }
            return index;
        }

        /*
            Imprime en consola las placas / posiciones random despues completar la eleccion
         */
        private void PrintPlacasRandom()
        {
            for (int i = 0; i < posicionesPlacasRandom.Length; i++)
            {
                logController.PrintInConsole("Posicion random: " + posicionesPlacasRandom[i].transform.position);
            }
        }

        #endregion

        
    }//END CLASS
}

