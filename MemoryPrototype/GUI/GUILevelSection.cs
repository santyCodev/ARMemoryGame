using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MemoryPrototype.Gui
{
    public class GUILevelSection : MonoBehaviour
    {        
        private const string ACIERTOS = "ACIERTOS: ";
        private const string FALLOS = "FALLOS: ";
        private const int CUENTA = 800;

        [SerializeField] private TextMeshProUGUI aciertoText;
        [SerializeField] private TextMeshProUGUI falloText;

        private int cuentaAtras;
        private TimeBarControl timeBarControl;

        public delegate void GameLevelEnd();                             //Delegado para el evento
        public static event GameLevelEnd OnLevelGameEnd;                 //Evento para avisar que el juego ha terminado

        // Start is called before the first frame update
        void Awake()
        {
            aciertoText.text = ACIERTOS;
            falloText.text = FALLOS;
            cuentaAtras = CUENTA;
            timeBarControl = gameObject.GetComponent<TimeBarControl>();
            timeBarControl.SetMaxTime(CUENTA);
            ActualizarAciertosLevel(0);
            ActualizarFallosLevel(0);
        }

        /* Activacion y desactivacion de la seccion de juego */
        public void ActivateSeccionLevel() { gameObject.SetActive(true); }
        public void DesactivateSeccionLevel() { gameObject.SetActive(false); }

        /* Actualizacion de aciertos y fallos de la gui */
        public void ActualizarAciertosLevel(int numAciertos) { aciertoText.text = ACIERTOS + numAciertos.ToString(); }
        public void ActualizarFallosLevel(int numFallos) { falloText.text = FALLOS + numFallos.ToString(); }

        public void StartLevelGame() {
            ActivateSeccionLevel();
            StartCoroutine(ShowBarraAndParameters()); 
        }

        /* 
         * Corrutina que muestra los aciertos, fallos y decrementa la barra de
            cuenta atras
        */
        IEnumerator ShowBarraAndParameters()
        {
            while (cuentaAtras > 0)
            {
                cuentaAtras--;
                timeBarControl.SetTime(cuentaAtras);
                yield return null;
            }

            yield return new WaitForSeconds(1);
            DesactivateSeccionLevel();
            OnLevelGameEnd();
        }
    }
}

