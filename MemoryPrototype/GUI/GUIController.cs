using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    private const string ACIERTOS = "Aciertos: ";
    private const string FALLOS = "Fallos: ";
    private const int CUENTA = 10;

    [SerializeField] private GameObject pageTitle;
    [SerializeField] private GameObject pageInstructions;
    [SerializeField] private GameObject datosLevel;
       
    [SerializeField] private TextMeshProUGUI cuentaAtrasGOText;
    [SerializeField] private TextMeshProUGUI cuentaAtrasBarraText;
    [SerializeField] private TextMeshProUGUI aciertoText;
    [SerializeField] private TextMeshProUGUI falloText;

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
    }

    #region Activacion y desactivacion de elementos GUI
    public void ActivatePageTitle() { pageTitle.SetActive(true); }
    public void DesactivatePageTitle() { pageTitle.SetActive(false); }
    public void ActivatePageInstructions() { pageInstructions.SetActive(true); }
    public void DesactivatePageInstructions() { pageInstructions.SetActive(false); }
    public void ActivateDatosLevel() { datosLevel.SetActive(true); }
    public void DesactivateDatosLevel() { datosLevel.SetActive(false); }
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

        while(cuentaAtrasGo > 0)
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
        cuentaAtrasBarraText.gameObject.SetActive(true);

        while (cuentaAtras > 0)
        {
            cuentaAtrasBarraText.text = cuentaAtras.ToString();
            cuentaAtras--;
            yield return new WaitForSeconds(1);
        }

        cuentaAtrasBarraText.text = "SE ACABO";
        yield return new WaitForSeconds(1);
        cuentaAtrasBarraText.gameObject.SetActive(false);
    }
    #endregion

}
