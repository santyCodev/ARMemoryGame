using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    private const string ACIERTOS = "Aciertos: ";
    private const string FALLOS = "Fallos: ";
    private const string RONDA = "Ronda: ";
    private const string NIVEL = "Nivel: ";

    [SerializeField] private GameObject pageTitle;
    [SerializeField] private GameObject pageInstructions;
    [SerializeField] private GameObject datosLevel;
    [SerializeField] private TextMeshProUGUI[] datosLevelList;

    private TextMeshProUGUI fallosText;
    private TextMeshProUGUI aciertosText;
    private TextMeshProUGUI rondasText;
    private TextMeshProUGUI nivelText;
    
    [SerializeField] private TextMeshProUGUI cuentaAtrasGOText;
    [SerializeField] private TextMeshProUGUI cuentaAtrasBarraText;
    [SerializeField] private TextMeshProUGUI aciertoText;
    [SerializeField] private TextMeshProUGUI falloText;

    private int cuentaAtrasGo;

    public delegate void GoNextState();                             //Delegado para el evento
    public static event GoNextState OnCuentaAtrasTerminada;         //Evento para avisar que la cuenta atras ha terminado

    // Start is called before the first frame update
    void Start()
    {
        fallosText = datosLevelList[0];
        aciertosText = datosLevelList[1];
        rondasText = datosLevelList[2];
        nivelText = datosLevelList[3];

        cuentaAtrasGo = 3;
    }

    #region Activacion y desactivacion de elementos GUI
    public void ActivatePageTitle() { pageTitle.SetActive(true); }
    public void DesactivatePageTitle() { pageTitle.SetActive(false); }
    public void ActivatePageInstructions() { pageInstructions.SetActive(true); }
    public void DesactivatePageInstructions() { pageInstructions.SetActive(false); }
    public void ActivateDatosLevel() { datosLevel.SetActive(true); }
    public void DesactivateDatosLevel() { datosLevel.SetActive(true); }
    #endregion

    #region Actualizar datos de nivel
    public void ActualizarDatosLevel(int numFallos, int numAciertos, int numRondas, int numLevel)
    {        
        nivelText.text = NIVEL + numLevel.ToString();
        rondasText.text = RONDA + numRondas.ToString();
        aciertosText.text = ACIERTOS + numAciertos.ToString();
        fallosText.text = FALLOS + numFallos.ToString();
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
    #endregion

}
