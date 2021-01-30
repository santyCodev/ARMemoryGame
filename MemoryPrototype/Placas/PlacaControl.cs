using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaControl : MonoBehaviour
{
    private const string CLASS_NAME = "PLACA CONTROL";                                        //Constante con el nombre de la clase

    [SerializeField] private LogController logController;                                       //Controlador de logs
    
    private Material material;
    private Color32 actualColor;
    private Color32 markedColor;
    private Color32 failColor;
    private Color32 successColor;
    public delegate void PlacaAnimated();                           //Delegado para el evento
    public static event PlacaAnimated OnPlacaAnimationFail;         //Evento para avisar que la placa ha terminado la animacion de fallo
    public static event PlacaAnimated OnPlacaAnimationSuccess;      //Evento para avisar que la placa ha terminado la animacion de acierto

    private const float MAX_Y = 0.1f;                               //Altura maxima de placa
    private const float MAX_SCALE = 1.3f;                           //Escala maxima de placa
    private const float LERP_SPEED_CONST = 2f;                   //Constante de velocidad de la interpolacion lineal
    private Vector3 targetPosition;
    private Vector3 actualPosition;
    private Vector3 targetScale;
    private Vector3 actualScale;

    /*
        Recibe el material desde el componente Renderer
     */
    void Start()
    {
        material = GetComponent<Renderer>().material;
        actualColor = material.color;
        markedColor = new Color32(0, 104, 111, 255);
        failColor = new Color32(255, 95, 0, 255);
        successColor = new Color32(0, 255, 2, 255);
        targetPosition = new Vector3(transform.localPosition.x, MAX_Y, transform.localPosition.z);
        targetScale = new Vector3(MAX_SCALE, transform.localScale.y, MAX_SCALE);
    }

    #region Cambio de color
    /* Cambia el color original del material */
    public void ChangeMaterialColor() { material.SetColor("_Color", markedColor); }
    public void ChangeMaterialFailColor() { material.SetColor("_Color", failColor); }
    public void ChangeMaterialSuccessColor() { material.SetColor("_Color", successColor); }

    /* Devuelve el color original del material */
    public void SetOriginalMaterialColor() { material.SetColor("_Color", actualColor); }
    #endregion

    #region Animaciones
    /* Cambia al color de acierto, despues anima la placa con una courritina */
    public void SuccessAnimation()
    {
        ChangeMaterialSuccessColor();
        StartCoroutine(PlacaAnimation(true));
    }

    /* Cambia al color de fallo, despues anima la placa con una courritina */
    public void FailAnimation()
    {
        ChangeMaterialFailColor();
        StartCoroutine(PlacaAnimation(false));
    }

    /*
        Corrutina para animacion de placas
        - Guarda la posicion y escala original de la placa
        - Ejecuta la animacion de posicion y escalado
        - Devuelve a la placa la posicion y escala original
        - Si el parametro = true, lanza el evento para gestionar aciertos
        - Si el parametro = false, lanza el evento para gestionar fallos
     */
    IEnumerator PlacaAnimation(bool animType)
    {
        PrintMessage("PlacaAnimation() - INICIO");
        actualPosition = transform.localPosition;
        actualScale = transform.localScale;

        while (Vector3.Distance(targetPosition, transform.position) > Vector3.kEpsilon)
        {
            ChangePositionAndScale();
            yield return null;
        }

        transform.localPosition = actualPosition;
        transform.localScale = actualScale;

        if (animType) { OnPlacaAnimationSuccess(); }
        else { OnPlacaAnimationFail(); }
        PrintMessage("PlacaAnimation() - FIN");
    }

    /*
        Ejecuta mediante interpolacion lineal el cambio entre:
        - La posicion inicial y final de la placa
        - La escala inicial y final de la placa 
        - En funcion de la distancia con la formula (1/(distance+LERP_SPEED_CONST)) la placa se movera/escalara
               al destino con una velocidad constante
        - Cuanto mas pequeña es la constante LERP_SPEED_CONST, mas rapida sera la velocidad de movimiento/escalado
     */
    private void ChangePositionAndScale()
    {        
        PrintMessage(" ChangePositionAndScale() - TargetPosition = "+ targetPosition +", TargetScale = " +targetScale);
        float positionDistance = Vector3.Distance(targetPosition, transform.position);
        float scaleDistance = Vector3.Distance(targetScale, transform.localScale);
        transform.localPosition = Vector3.Lerp(transform.position, targetPosition, (1 / (positionDistance + LERP_SPEED_CONST)));
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, (1 / (scaleDistance + LERP_SPEED_CONST)));
    }

    #endregion

    #region Gestion de Logs        
    /*
        Usa el controlador de logs para imprimir un mensaje en consola
     */
    private void PrintMessage(string message)
    {
        logController.PrintInConsole(CLASS_NAME + message);
    }
    #endregion
}
