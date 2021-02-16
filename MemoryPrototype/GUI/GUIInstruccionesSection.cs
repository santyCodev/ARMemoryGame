using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryPrototype.Gui
{
    public class GUIInstruccionesSection : MonoBehaviour
    {
        #region Seccion Instrucciones
        /* Activacion y desactivacion de la seccion instrucciones */
        public void ActivatePageInstructions() { gameObject.SetActive(true); }
        public void DesactivatePageInstructions() { gameObject.SetActive(false); }
        #endregion
    }
}

