using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MemoryPrototype.Data;

namespace MemoryPrototype.Gui
{
    public class GUIResultadosSection : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameEndDesc;
        [SerializeField] private TextMeshProUGUI gameEndText;
        [SerializeField] private TextMeshProUGUI gameEndAciertos;
        [SerializeField] private TextMeshProUGUI gameEndFallos;
        [SerializeField] private TextMeshProUGUI timePrecisionText;
        [SerializeField] private TextMeshProUGUI timeReactionText;
        [SerializeField] private GameObject accuracySlider;
        [SerializeField] private GameObject reactionSlider;
        private TimeBarControl accuracyBarControl;
        private TimeBarControl reactionBarControl;

        public delegate void EndResultados();                             //Delegado para el evento
        public static event EndResultados OnEndResultados;               //Evento para avisar que ha terminado de crecer las barras

        public void Awake()
        {
            accuracyBarControl = accuracySlider.GetComponent<TimeBarControl>();
            reactionBarControl = reactionSlider.GetComponent<TimeBarControl>();
            accuracyBarControl.SetMaxTimePercent(100);
            //reactionBarControl.SetMaxTime(0);
        }

        /* Metodos de activacion y desactivacion de la seccion resultados */
        public void ActivateResultados() { gameObject.SetActive(true); }
        public void DesactivateResultados() { gameObject.SetActive(false); }

        public void SetDescription(float recordAciertos, float recordReaction, float recordPrecision)
        {
            gameEndDesc.text = "En este juego tu record en aciertos es de " + recordAciertos +
                              ", tu record de tiempo de reaccion es de " + recordReaction.ToString("0.00") + " segundos " +
                              "y tu record de precision es del " + recordPrecision.ToString("0") + " %.";                
        }

        public void SetAciertosFallos(float aciertosSesion, float fallosSesion)
        {
            gameEndAciertos.text = "ACIERTOS = " + aciertosSesion;
            gameEndFallos.text = "FALLOS = " + fallosSesion;
        }

        public void SetMediasSlider(float mediaReaction, float percentPrecision)
        {
            StartCoroutine(ShowBarraAndParameters(mediaReaction, reactionBarControl, 0.05f, true));
            StartCoroutine(ShowBarraAndParameters(percentPrecision, accuracyBarControl, 1.0f, false));
        }

        /* 
         * Corrutina que muestra los aciertos, fallos y decrementa la barra de
            cuenta atras
        */
        IEnumerator ShowBarraAndParameters(float maximo, TimeBarControl timeBarControl, float time, bool isReaction)
        {
            float contador = 0.0f;
            while (contador < maximo)
            {
                contador += time;
                timeBarControl.SetTimeFloat(contador);
                SetTimeNumberGUI(isReaction, contador);
                yield return null;
            }

            yield return new WaitForSeconds(1);
            OnEndResultados();
        }

        private void SetTimeNumberGUI(bool isReaction, float time)
        {
            if (isReaction) { timeReactionText.text = "Tiempo reaccion: " + time.ToString("0.00") + " segundos"; }
            else { timePrecisionText.text = "Promedio precision: " + time.ToString("0") + " %"; }
        }
    }   
}

