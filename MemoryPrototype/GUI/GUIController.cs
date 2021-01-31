using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    [SerializeField] private GameObject pageTitle;
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

    // Start is called before the first frame update
    void Start()
    {
        fallosText = datosLevelList[0];
        aciertosText = datosLevelList[1];
        rondasText = datosLevelList[2];
        nivelText = datosLevelList[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /* Activa o desactiva un bloque de texto o texto */
    public void SetActiveText(GameObject texto, bool option) { texto.SetActive(option); }

    /* Devuelve si el personaje esta activo o no */
    public bool GetIsActive(GameObject texto) { return texto.activeSelf; }
}
