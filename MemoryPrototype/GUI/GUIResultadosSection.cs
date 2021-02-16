using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Gui
{
    public class GUIResultadosSection : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        /* Metodos de activacion y desactivacion de la seccion resultados */
        public void ActivateResultados() { gameObject.SetActive(true); }
        public void DesactivateResultados() { gameObject.SetActive(false); }

    }
}

