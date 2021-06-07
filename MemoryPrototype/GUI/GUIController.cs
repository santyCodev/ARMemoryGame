using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MemoryPrototype.Data;
using Audio;

namespace MemoryPrototype.Gui
{
    public class GUIController : MonoBehaviour
    {
        [SerializeField] private GameObject pantallaTitle;
        [SerializeField] private GameObject pantallaInstrucciones;
        [SerializeField] private GameObject pantallaGame;
        [SerializeField] private GameObject pantallaResultados;
        [SerializeField] private GameObject pantallaBuscarTarget;
        [SerializeField] private TextMeshProUGUI cuentaAtrasGOText;
        [SerializeField] private AudioClip countDownSound;
        [SerializeField] private AudioClip newGameSound;
        [SerializeField] private AudioClip normalButtonSound;

        private GameObject pantallaActual;
        private AudioController audioController;
        private GUILevelSection guiLevelGame;
        private GUIResultadosSection guiResultados;
        private GUIBuscarPatron guiBuscarPatron;

        public delegate void GoNextState();                             //Delegado para el evento
        public static event GoNextState OnCuentaAtrasTerminada;         //Evento para avisar que la cuenta de GO atras ha terminado
        public delegate void EndGame();                                 //Delegado para el evento
        public static event EndGame OnBarraCuentaAtrasTerminada;        //Evento para avisar que la cuenta atras de barra ha terminado
        public delegate void EndResultadosGUI();                                 //Delegado para el evento
        public static event EndResultadosGUI OnFinResultados;        //Evento para avisar que la cuenta atras de barra ha terminado
        public delegate void ExitApplication(bool isExit);                                 //Delegado para el evento
        public static event ExitApplication OnExitApp;        //Evento para avisar que la cuenta atras de barra ha terminado

        private int cuentaAtrasGo;
        private int contadorFinResultados;

        public bool GoToInstructions { get; set; }

        #region Inicializacion controlador
        void Start()
        {
            guiLevelGame = pantallaGame.GetComponent<GUILevelSection>();
            guiResultados = pantallaResultados.GetComponent<GUIResultadosSection>();
            guiBuscarPatron = pantallaBuscarTarget.GetComponent<GUIBuscarPatron>();
            audioController = GetComponent<AudioController>();
            cuentaAtrasGo = 3;
            contadorFinResultados = 0;
        }

        /* Subscripcion de eventos */
        private void OnEnable()
        {
            GUILevelSection.OnLevelGameEnd += EndLevelGame;
            GUIResultadosSection.OnEndResultados += EndResultados;
            GUISeccionTitle.OnTapClickOnScreen += GoToPantallaInstrucciones;
            GUIInstruccionesSection.OnTapClickOnGameButton += GoToCuentaAtrasGo;
            GUIBuscarPatron.GoToGame += ActivateGameLogic;
        }

        /* Desuscripcion de eventos */
        private void OnDisable()
        {
            GUILevelSection.OnLevelGameEnd -= EndLevelGame;
            GUIResultadosSection.OnEndResultados -= EndResultados;
            GUISeccionTitle.OnTapClickOnScreen -= GoToPantallaInstrucciones;
            GUIInstruccionesSection.OnTapClickOnGameButton -= GoToCuentaAtrasGo;
            GUIBuscarPatron.GoToGame -= ActivateGameLogic;
        }        
        #endregion

        #region Seccion Busqueda patron
        /* Pantalla que busca el image target */
        public void ActivarPantallaPatron(bool goToinstructions) 
        {            
            ActivatePantalla(pantallaBuscarTarget);
            GoToInstructions = goToinstructions;
        }
        public void DesactivarPantallaPatron() { DesactivatePantalla(pantallaBuscarTarget); }

        /* Evento llamado despues de encontrar el image target
            - Segun la variable GoToCuentaAtras se bifurca a la pantalla de cuenta atras o la de title*/
        private void ActivateGameLogic()
        {
            if (GoToInstructions) { GoToInstructionsFromBusquedaPatron(); }
            else { GoToPantallaTitle(); }
        }

        /* Evento llamado desde la pantalla de busqueda de patron, para ir directamente a las instrucciones*/
        public void GoToInstructionsFromBusquedaPatron()
        {
            DesactivarPantallaPatron();
            ActivatePantallaInstructions();
        }

        /*  Activacion de la pantalla de titulo desde la pantalla de busqueda de patron */
        public void GoToPantallaTitle()
        {
            DesactivarPantallaPatron();
            ActivatePantallaTitle();
        }

        #endregion

        #region Pantalla de Titulo        
        public void ActivatePantallaTitle() { ActivatePantalla(pantallaTitle); }

        /*  Desactivacion de la pantalla de titulo desde el GuiController */
        public void DesactivatePantallaTitle() { DesactivatePantalla(pantallaTitle); }

        /* 
         * Evento de llamado desde la pantalla de titulo
         *  - Desactiva la pantalla de titulo
         *  - Activa la pantalla de instrucciones     
         */
        public void GoToPantallaInstrucciones()
        {
            DesactivatePantallaTitle();
            ActivatePantallaInstructions();
        }
        #endregion
        
        #region Pantalla de Instrucciones
        /*  Activacion de la pantalla de instrucciones desde el GUIController*/
        public void ActivatePantallaInstructions() { ActivatePantalla(pantallaInstrucciones); }

        /*  Desactivacion de la pantalla de instrucciones desde el GuiController */
        public void DesactivatePantallaInstructions() { DesactivatePantalla(pantallaInstrucciones); }

        /* 
         * Evento de boton llamado desde la pantalla de las instrucciones
         *  - Desactiva la pagina de instrucciones
         *  - Arranca la corrutina para la cuentra atras y GO
         */
        public void GoToCuentaAtrasGo()
        {
            DesactivatePantallaInstructions();
            ActivarCuentaAtrasGame();            
        }
        #endregion        

        #region Cuenta atras GO
        /* Pantalla que se activa si se ha encontrado el image target, se ejecuta con evento*/
        public void ActivarCuentaAtrasGame()
        {            
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
            pantallaActual = cuentaAtrasGOText.gameObject;

            yield return new WaitForSeconds(1);

            while (cuentaAtrasGo > 0)
            {
                audioController.PlayOneShotSound(countDownSound);
                cuentaAtrasGOText.text = cuentaAtrasGo.ToString();
                cuentaAtrasGo--;
                yield return new WaitForSeconds(1);
            }

            cuentaAtrasGOText.text = "GO";
            yield return new WaitForSeconds(1);

            cuentaAtrasGOText.gameObject.SetActive(false);
            pantallaActual = null;

            OnCuentaAtrasTerminada();
        }
        #endregion

        #region Seccion Juego
        /* 
         *  Metodo que se llama cuando comienza el juego
         *  - Inicia la corrutina para mostrar los parametros 
         *      de aciertos y fallos y para mostrar la barra de tiempo de juego
         */
        public void StartLevelGame() 
        { 
            guiLevelGame.StartLevelGame();
            pantallaActual = pantallaGame;
        }

        public void ActualizarAciertosLevel(int aciertos) { guiLevelGame.ActualizarAciertosLevel(aciertos); }

        public void ActualizarFallosLevel(int fallos) { guiLevelGame.ActualizarFallosLevel(fallos); }

        public void EndLevelGame() 
        {
            pantallaActual = null;
            OnBarraCuentaAtrasTerminada();        
        }
        #endregion

        #region Seccion Resultados
        /* Metodos de activacion y desactivacion de la seccion resultados */
        public void ActivateResultados() 
        { 
            guiResultados.ActivateResultados();
            pantallaActual = pantallaResultados;
        }
        public void DesactivateResultados() 
        { 
            guiResultados.DesactivateResultados();
            pantallaActual = null;
        }

        public void SetResultParameters(int aciertosSesion, int fallosSesion, float aciertosPerc, float fallosPerc)
        {
            guiResultados.SetDescription(aciertosSesion, fallosSesion, aciertosPerc, fallosPerc);
            guiResultados.SetAciertosFallos(aciertosSesion, fallosSesion);
        }

        public void EndResultados()
        {
            contadorFinResultados++;
            if (contadorFinResultados == 2) { OnFinResultados(); }
        }

        /* 
        * Evento llamado desde el boton salir
        */
        public void ButtonExitAction() 
        {
            audioController.PlayOneShotSound(normalButtonSound);
            LoadScene(false); 
        }

        /* 
        * Evento llamado desde el boton jugar
        */
        public void ButtonJugarAction() 
        {
            audioController.PlayOneShotSound(newGameSound);
            LoadScene(true); 
        }

        /**
         * Si el parametro flag es true, recarga la escena para volver a jugar
         * Si el parametro flag es false, cierra la aplicacion
         */
        private void LoadScene(bool flag)
        {            
            OnExitApp(flag);
            if (flag) { SceneManager.LoadScene(SceneManager.GetActiveScene().name); }
            else { Application.Quit(); }            
        }
        #endregion

        #region Logica para activar y desactivar pantallas

        private void ActivatePantalla(GameObject pantalla)
        {
            pantalla.SetActive(true);
            pantallaActual = pantalla;
        }

        private void DesactivatePantalla(GameObject pantalla)
        {
            pantalla.SetActive(false);
            pantallaActual = null;
        }

        #endregion
    }
}

