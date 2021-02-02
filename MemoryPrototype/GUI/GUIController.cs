using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryPrototype.Gui
{
    public class GUIController : MonoBehaviour
    {
        private const string ACIERTOS = "Aciertos: ";
        private const string FALLOS = "Fallos: ";
        private const int CUENTA = 500;

        [SerializeField] private GameObject seccionTitle;
        [SerializeField] private GameObject seccionInstrucciones;
        [SerializeField] private GameObject seccionAciertosFallos;
        [SerializeField] private GameObject seccionBarraCuentaAtras;

        [SerializeField] private TextMeshProUGUI cuentaAtrasGOText;
        [SerializeField] private TextMeshProUGUI aciertoText;
        [SerializeField] private TextMeshProUGUI falloText;
        
        private TimeBarControl timeBarControl;
        private int cuentaAtrasGo;
        private int cuentaAtras;

        public delegate void GoNextState();                             //Delegado para el evento
        public static event GoNextState OnCuentaAtrasTerminada;         //Evento para avisar que la cuenta atras ha terminado

        // Start is called before the first frame update
        void Start()
        {
            aciertoText.text = ACIERTOS;
            falloText.text = FALLOS;
            cuentaAtrasGo = 3;
            cuentaAtras = CUENTA;
            timeBarControl = seccionBarraCuentaAtras.GetComponent<TimeBarControl>();
            timeBarControl.SetMaxTime(CUENTA);
        }

        #region Activacion y desactivacion de elementos GUI
        public void ActivatePageTitle() { seccionTitle.SetActive(true); }
        public void DesactivatePageTitle() { seccionTitle.SetActive(false); }
        public void ActivatePageInstructions() { seccionInstrucciones.SetActive(true); }
        public void DesactivatePageInstructions() { seccionInstrucciones.SetActive(false); }
        public void ActivateDatosLevel() { seccionAciertosFallos.SetActive(true); }
        public void DesactivateDatosLevel() { seccionAciertosFallos.SetActive(false); }
        public void ActivateBarraCuentaAtras() { seccionBarraCuentaAtras.SetActive(true); }
        public void DesactivateBarraCuentaAtras() { seccionBarraCuentaAtras.SetActive(false); }
        #endregion

        #region Actualizar datos de nivel
        public void ActualizarDatosLevel(int numFallos, int numAciertos)
        {
            aciertoText.text = ACIERTOS + numAciertos.ToString();
            falloText.text = FALLOS + numFallos.ToString();
        }
        #endregion

        #region Acciones de boton
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
        #endregion

        #region Corrutinas
        /* 
         * Corrutina para mostrar en Gui la cuenta atras
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

        public void StartCuentaAtrasBarra()
        {
            StartCoroutine(CuentaAtrasBarra());
        }
        IEnumerator CuentaAtrasBarra()
        {
            ActivateBarraCuentaAtras();

            while (cuentaAtras > 0)
            {
                cuentaAtras--;
                timeBarControl.SetTime(cuentaAtras);
                yield return null;
            }
            
            yield return new WaitForSeconds(1);
            DesactivateBarraCuentaAtras();
        }
        #endregion

    }
}

