using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MemoryPrototype.Gui
{
    public class GUIController : MonoBehaviour
    {
        [SerializeField] private GameObject seccionTitle;
        [SerializeField] private GameObject seccionInstrucciones;
        [SerializeField] private GameObject seccionLevel;
        [SerializeField] private GameObject seccionResultados;

        //instrucciones
        [SerializeField] private TextMeshProUGUI cuentaAtrasGOText;
        private int cuentaAtrasGo;

        //juego / incluyen los metodos actualizarAciertos/FallosLevel()
        private const string ACIERTOS = "ACIERTOS: ";
        private const string FALLOS = "FALLOS: ";
        private const int CUENTA = 700;
        [SerializeField] private TextMeshProUGUI aciertoText;
        [SerializeField] private TextMeshProUGUI falloText;
        private int cuentaAtras;
        private TimeBarControl timeBarControl;

        //Resultados

        public delegate void GoNextState();                             //Delegado para el evento
        public static event GoNextState OnCuentaAtrasTerminada;         //Evento para avisar que la cuenta de GO atras ha terminado
        public delegate void EndGame();                                 //Delegado para el evento
        public static event EndGame OnBarraCuentaAtrasTerminada;        //Evento para avisar que la cuenta atras de barra ha terminado

        // Start is called before the first frame update
        void Awake()
        {
            aciertoText.text = ACIERTOS;
            falloText.text = FALLOS;
            cuentaAtrasGo = 3;
            cuentaAtras = CUENTA;
            timeBarControl = seccionLevel.GetComponent<TimeBarControl>();
            timeBarControl.SetMaxTime(CUENTA);
            ActualizarAciertosLevel(0);
            ActualizarFallosLevel(0);
        }

        #region Seccion Titulo
        /* Activacion y desactivacion de la seccion de titulo*/
        public void ActivatePageTitle() { seccionTitle.SetActive(true); }
        public void DesactivatePageTitle() { seccionTitle.SetActive(false); }

        /* 
         * Evento de boton llamado con el boton start
         *  - Desactiva la pantalla de titulo
         *  - Activa la pagina de instrucciones     
         */
        public void ButtonStartAction()
        {
            DesactivatePageTitle();
            ActivatePageInstructions();
        }
        #endregion

        #region Seccion Instrucciones
        /* Activacion y desactivacion de la seccion instrucciones */
        public void ActivatePageInstructions() { seccionInstrucciones.SetActive(true); }
        public void DesactivatePageInstructions() { seccionInstrucciones.SetActive(false); }

        /* 
         * Evento de boton llamado con el boton Go de las instrucciones
         *  - Desactiva la pagina de instrucciones
         *  - Arranca la corrutina para la cuentra atras y GO
         */
        public void ButtonGoAction()
        {
            DesactivatePageInstructions();
            StartCoroutine(CuentaAtrasGo());
        }

        /* 
         * Corrutina para mostrar en Gui la cuenta atras y Go
         *  - Activa el texto cuentaAtrasGo
         *  - Hace la cuenta atras hasta que llega a cero
         *  - Muestra la palabra GO
         *  - Envia el evento para comenzar el primer estado
         */
        IEnumerator CuentaAtrasGo()
        {
            cuentaAtrasGOText.gameObject.SetActive(true);

            while (cuentaAtrasGo > 0)
            {
                cuentaAtrasGOText.text = cuentaAtrasGo.ToString();
                cuentaAtrasGo--;
                yield return new WaitForSeconds(1);
            }

            cuentaAtrasGOText.text = "GO";
            yield return new WaitForSeconds(1);
            cuentaAtrasGOText.gameObject.SetActive(false);
            OnCuentaAtrasTerminada();
        }
        #endregion

        #region Seccion Juego
        /* Activacion y desactivacion de la seccion de juego */
        public void ActivateSeccionLevel() { seccionLevel.SetActive(true); }
        public void DesactivateSeccionLevel() { seccionLevel.SetActive(false); }

        /* Actualizacion de aciertos y fallos de la gui */
        public void ActualizarAciertosLevel(int numAciertos) { aciertoText.text = ACIERTOS + numAciertos.ToString(); }
        public void ActualizarFallosLevel(int numFallos) { falloText.text = FALLOS + numFallos.ToString(); }

        /* 
         *  Metodo que se llama cuando comienza el juego
         *  - Inicia la corrutina para mostrar los parametros 
         *      de aciertos y fallos y para mostrar la barra de tiempo de juego
         *  
         */
        public void StartLevelGame()
        {
            StartCoroutine(ShowBarraAndParameters());
        }

        /* 
         * Corrutina que muestra los aciertos, fallos y decrementa la barra de
            cuenta atras
        */
        IEnumerator ShowBarraAndParameters()
        {
            ActivateSeccionLevel();

            while (cuentaAtras > 0)
            {
                cuentaAtras--;
                timeBarControl.SetTime(cuentaAtras);
                yield return null;
            }
            
            yield return new WaitForSeconds(1);
            DesactivateSeccionLevel();
            OnBarraCuentaAtrasTerminada();
        }
        #endregion

        #region Seccion Resultados
        /* Metodos de activacion y desactivacion de la seccion resultados */
        public void ActivateResultados() { seccionResultados.SetActive(true); }
        public void DesactivateResultados() { seccionResultados.SetActive(false); }

        /* 
        * Hace un reload de toda la scene, para empezar con todo en estado inicial
        */
        public void ButtonExitAction()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion
    }
}

