﻿using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Placas
{
    public class PlacasController : MonoBehaviour
    {
        [SerializeField] private LogController logController;       //Controlador de logs
        [SerializeField] private GameObject placasParent;           //parent de las placas

        private const int NUM_PLACAS_INITIAL = 3;
        private const string DEFAULT_TAG = "Plate";
        private const string MARKED_TAG = "PlateMarked";

        private Transform[] placas;                                 //Array que contendra a todas las placas        
        public int NumPlacasRandom { get; set; }                    //Numero de placas random a recoger
        public GameObject[] PlacasRandom { get; set; }              //Array con las placas random elegidas
        private void Awake()
        {
            placas = GetPlacasFromParent();              
            SetDefaultTags();
            InitializePlacasRandom();
        }

        #region SetTags

        /*
            Recorre el array de placas y asigna en cada una de ellas el tag default
         */
        private void SetDefaultTags()
        {
            foreach (var placa in placas) { placa.gameObject.tag = DEFAULT_TAG; }
        }

        /*
            Asigna la tag MARKED_TAG a las placas random
         */
        public void SetMarkedTag()
        {
            SetRandomTag(MARKED_TAG);
        }

        /*
            Asigna de nuevo la DEFAULT_TAG a las placas que fueron cambiadas antes por la MARKED_TAG
         */
        public void SetRandomTagsToDefault()
        {
            SetRandomTag(DEFAULT_TAG);            
        }

        /*
            Asigna a las placas del array de placas random el tagType
         */
        private void SetRandomTag(string tagType)
        {
            foreach (var placa in PlacasRandom) { placa.tag = tagType; }
        }

        public bool CheckIfMarkedTag() 
        {
            foreach (var placa in PlacasRandom) { if (placa.CompareTag(MARKED_TAG)) { return true; }}
            return false;
        }
        #endregion

        #region InitializePlacas

        /* 
            Recoge las placas que son hijas del parent en un array de Transforms

         */
        private Transform[] GetPlacasFromParent()
        {
            return placasParent.GetComponentsInChildren<Transform>();
        }

        /*
            Se instancia el array de placas random con el numero de placas que contendra 
         */
        public void InstantiatePlacasRandom(int numPlacas)
        {
            PlacasRandom = new GameObject[numPlacas];
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
        public void SetRandomPlacas()
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
            Compara la placa anterior con la actual si no son iguales
            - La placa actual se guarda en el array de placas random
            - Despues se asigna la actual a la anterior
            - Sumamos index++ para seguir asignando placas
         */
        private int CompareIfPlacasEquals(int index, Transform actualPlacaRandom, GameObject lastPlacaRamdom)
        {
            if (lastPlacaRamdom == null || !actualPlacaRandom.Equals(lastPlacaRamdom.transform))
            {
                logController.PrintInConsole("Las Posiciones de las placas no son iguales");
                PlacasRandom[index] = actualPlacaRandom.gameObject;
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
            for (int i = 0; i < PlacasRandom.Length; i++)
            {
                logController.PrintInConsole("Posicion random: " + PlacasRandom[i].transform.position);
            }
        }

        #endregion

        #region Placas Material Color

        /*
            Devuelve a las placas aleatorias su color original
         */
        public void SetOriginalMaterialColor()
        {
            logController.PrintInConsole("SetOriginalMaterialColor() - Devolviendo el color a las placas");
            foreach (var placa in PlacasRandom)
            {
                placa.GetComponent<PlacaControl>().SetOriginalMaterialColor();
            }
        }

        #endregion
    }//END CLASS
}

