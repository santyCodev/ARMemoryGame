using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MemoryPrototype.Data;

namespace MemoryPrototype.Gui
{
    public class GUIController : MonoBehaviour
    {
        [SerializeField] private GameObject seccionTitle;
        [SerializeField] private GameObject seccionInstrucciones;
        [SerializeField] private GameObject seccionLevel;
        [SerializeField] private GameObject seccionResultados;

        private GUIInstruccionesSection guiInstrucciones;
        private GUILevelSection guiLevelGame;
        private GUIResultadosSection guiResultados;

        public delegate void GoNextState();                             //Delegado para el evento
        public static event GoNextState OnCuentaAtrasTerminada;         //Evento para avisar que la cuenta de GO atras ha terminado
        public delegate void EndGame();                                 //Delegado para el evento
        public static event EndGame OnBarraCuentaAtrasTerminada;        //Evento para avisar que la cuenta atras de barra ha terminado
        public delegate void EndResultadosGUI();                                 //Delegado para el evento
        public static event EndResultadosGUI OnFinResultados;        //Evento para avisar que la cuenta atras de barra ha terminado

        [SerializeField] private TextMeshProUGUI cuentaAtrasGOText;
        private int cuentaAtrasGo;
        private int contadorFinResultados;

        // Start is called before the first frame update
        void Start()
        {
            guiInstrucciones = seccionInstrucciones.GetComponent<GUIInstruccionesSection>();
            guiLevelGame = seccionLevel.GetComponent<GUILevelSection>();
            guiResultados = seccionResultados.GetComponent<GUIResultadosSection>();
            cuentaAtrasGo = 3;
            contadorFinResultados = 0;
        }

        /* Suscribe el evento de GUI CuentaAtrasEnd */
        private void OnEnable()
        {
            GUILevelSection.OnLevelGameEnd += EndLevelGame;
            GUIResultadosSection.OnEndResultados += EndResultados;
        }

        /* Da de baja al evento de GUI CuentaAtrasEnd */
        private void OnDisable()
        {
            GUILevelSection.OnLevelGameEnd -= EndLevelGame;
            GUIResultadosSection.OnEndResultados -= EndResultados;
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
            guiInstrucciones.ActivatePageInstructions();
        }
        #endregion

        #region Seccion Instrucciones
        /* 
         * Evento de boton llamado con el boton Go de las instrucciones
         *  - Desactiva la pagina de instrucciones
         *  - Arranca la corrutina para la cuentra atras y GO
         */
        public void GoToGame()
        {
            guiInstrucciones.DesactivatePageInstructions();
            StartCuentaAtrasGO();
        }        
        #endregion

        #region Cuenta atras GO
        public void StartCuentaAtrasGO() { StartCoroutine(CuentaAtrasGo()); }

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
        /* 
         *  Metodo que se llama cuando comienza el juego
         *  - Inicia la corrutina para mostrar los parametros 
         *      de aciertos y fallos y para mostrar la barra de tiempo de juego
         */
        public void StartLevelGame() { guiLevelGame.StartLevelGame(); }

        public void ActualizarAciertosLevel(int aciertos) { guiLevelGame.ActualizarAciertosLevel(aciertos); }

        public void ActualizarFallosLevel(int fallos) { guiLevelGame.ActualizarFallosLevel(fallos); }

        public void EndLevelGame() { OnBarraCuentaAtrasTerminada(); }
        #endregion

        #region Seccion Resultados
        /* Metodos de activacion y desactivacion de la seccion resultados */
        public void ActivateResultados() { guiResultados.ActivateResultados(); }
        public void DesactivateResultados() { guiResultados.DesactivateResultados(); }

        public void SetResultParameters(float recordAciertos, float aciertosSesion, float fallosSesion, float mediaReaction,
                                        float percentPrecision, float recordReaction, float recordPrecision)
        {
            guiResultados.SetDescription(recordAciertos, recordReaction, recordPrecision);
            guiResultados.SetAciertosFallos(aciertosSesion, fallosSesion);
            guiResultados.SetMediasSlider(mediaReaction, percentPrecision);
        }

        public void EndResultados()
        {
            contadorFinResultados++;
            if (contadorFinResultados == 2) { OnFinResultados(); }
        }

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

