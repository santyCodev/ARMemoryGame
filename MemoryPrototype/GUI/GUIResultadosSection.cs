using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MemoryPrototype.Data;
using MemoryPrototype.Utils;
using Audio;

namespace MemoryPrototype.Gui
{
    public class GUIResultadosSection : MonoBehaviour
    {
        private const float MAX_SCALE = 1.2f;
        private const float LERP_SPEED_CONST = 2f;                      //Constante de velocidad de la interpolacion lineal

        [SerializeField] private TextMeshProUGUI gameEndDesc;
        [SerializeField] private TextMeshProUGUI gameEndText;
        [SerializeField] private TextMeshProUGUI gameEndAciertos;
        [SerializeField] private TextMeshProUGUI gameEndFallos;
        [SerializeField] private Transform jugarTextTransform;
        [SerializeField] private Transform salirTextTransform;
        [SerializeField] private CorroutinesUtils corroutinesUtils;
        [SerializeField] private AudioClip endGameSound;

        private AudioController audioController;
        public delegate void EndResultados();                             //Delegado para el evento
        public static event EndResultados OnEndResultados;               //Evento para avisar que ha terminado de crecer las barras

        private void Start()
        {
            audioController = GetComponent<AudioController>();
            audioController.PlayOneShotSound(endGameSound);
        }

        /* Metodos de activacion y desactivacion de la seccion resultados */
        public void ActivateResultados() { gameObject.SetActive(true); }
        public void DesactivateResultados() { gameObject.SetActive(false); }

        public void SetDescription(int aciertosSesion, int fallosSesion, float aciertosPerc, float fallosPerc)
        {
            if (float.IsNaN(aciertosPerc)) { aciertosPerc = 0.0f; }
            if (float.IsNaN(fallosPerc)) { fallosPerc = 0.0f; } 

            if ((aciertosSesion >= fallosSesion) && (aciertosSesion != 0 & fallosSesion != 0)) { gameEndDesc.text = "Felicidades, tienes buena memoria! \n"; }
            else { gameEndDesc.text = "Esta bien, pero te falta entrenar mas! \n"; }

            gameEndDesc.text += "En esta partida haz memorizado " + aciertosSesion + " bloques y fallado "+ fallosSesion +
                              ", tu porcentaje de bloques memorizados es de un " + aciertosPerc.ToString("0") + "% " +
                              " y tu porcentaje de fallos es de un " + fallosPerc.ToString("0") + "%.";     
           
        }

        public void SetAciertosFallos(int aciertosSesion, int fallosSesion)
        {
            StartCoroutine(ShowAciertosFallosNumber(aciertosSesion, gameEndAciertos));
            StartCoroutine(ShowAciertosFallosNumber(fallosSesion, gameEndFallos));
            StartCorroutineText(jugarTextTransform);
            StartCorroutineText(salirTextTransform);
        }

        /* 
         * Corrutina que muestra los aciertos, fallos y decrementa la barra de
            cuenta atras
        */
        IEnumerator ShowAciertosFallosNumber(int maximo, TextMeshProUGUI aciertoFalloText)
        {
            int contador = 0;
            while (contador <= maximo)
            {                
                aciertoFalloText.text = contador.ToString();
                contador++;
                yield return null;
            }

            yield return new WaitForSeconds(1);
            OnEndResultados();
        }

        private void StartCorroutineText(Transform textTransform)
        {
            Vector3 initialScale = textTransform.localScale;
            Vector3 targetScale = new Vector3(MAX_SCALE, MAX_SCALE, textTransform.localScale.z);
            StartCoroutine(corroutinesUtils.TextScaleAnimation(initialScale, targetScale, LERP_SPEED_CONST, textTransform));
        }
    }   
}

