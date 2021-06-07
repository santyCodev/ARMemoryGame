using Audio;
using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaControl : MonoBehaviour
{
    private const string CLASS_NAME = "PLACA CONTROL";              //Constante con el nombre de la clase
    private const float MAX_Y = 0.5f;                               //Altura maxima de placa
    private const float MAX_SCALE = 1.3f;                           //Escala maxima de placa
    private const float LERP_SPEED_CONST = 2f;                      //Constante de velocidad de la interpolacion lineal

    [SerializeField] private LogController logController;           //Controlador de logs    
    [SerializeField] private Material[] material;                   //Lista de materiales    
    [SerializeField] private AudioClip blockFailSound;
    [SerializeField] private AudioClip blockSucessSound;
    [SerializeField] private AudioClip blockTapSound;

    public delegate void PlacaAnimated();                           //Delegado para el evento
    public static event PlacaAnimated OnPlacaAnimationFail;         //Evento para avisar que la placa ha terminado la animacion de fallo
    public static event PlacaAnimated OnPlacaAnimationSuccess;      //Evento para avisar que la placa ha terminado la animacion de acierto

    private AudioController audioController;
    private Renderer rend;
    private Color32 failColor;
    private Color32 successColor;
    private Color32 originalColor;    
    private Vector3 targetPosition;
    private Vector3 actualPosition;
    private Vector3 targetScale;
    private Vector3 actualScale;

    /*
        Recibe el material desde el componente Renderer
     */
    void Start()
    {
        failColor = new Color32(255, 64, 64, 255);
        originalColor = new Color32(255, 255, 255, 255);
        successColor = new Color32(197, 255, 118, 255);
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        ChangeOriginalMaterial();
        targetPosition = new Vector3(transform.localPosition.x, MAX_Y, transform.localPosition.z);
        targetScale = new Vector3(MAX_SCALE, transform.localScale.y, MAX_SCALE);
        audioController = GetComponent<AudioController>();
    }

    #region Cambio de material
    /* Asigna el material original [0]*/
    public void ChangeOriginalMaterial() { ChangeMaterial(0, originalColor); }

    /* Asigna el material de fallo [1]*/
    public void ChangeFailMaterial() { ChangeMaterial(1, failColor); }

    /* Asigna el material de acierto [2]*/
    public void ChangeSuccessMaterial() { ChangeMaterial(2, successColor); }

    public void ChangeMarkedMaterial()
    {
        ChangeMaterial(2, originalColor);
        PlaySound(blockTapSound);
    }

    private void ChangeMaterial(int materialIndex, Color32 newColor)
    {
        rend.material = material[materialIndex];
        rend.material.SetColor("_Color", newColor);
    }
    #endregion

    #region Animaciones
    /* Cambia al color de acierto, despues anima la placa con una courritina */
    public void SuccessAnimation()
    {
        PrintMessage("SuccessAnimation() - INICIO");
        ChangeSuccessMaterial();
        PlaySound(blockSucessSound);
        StartCoroutine(PlacaAnimation(true));
    }

    /* Cambia al color de fallo, despues anima la placa con una courritina */
    public void FailAnimation()
    {
        PrintMessage("FailAnimation() - INICIO");
        ChangeFailMaterial();
        PlaySound(blockFailSound, 2.0f);
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
    public IEnumerator PlacaAnimation(bool animType)
    {
        PrintMessage("PlacaAnimation() - INICIO");
        actualPosition = transform.localPosition;
        actualScale = transform.localScale;

        while (Vector3.Distance(targetPosition, transform.localPosition) > Vector3.kEpsilon)
        {
            ChangePositionAndScale();
            yield return null;
        }

        transform.localPosition = actualPosition;
        transform.localScale = actualScale;
        PrintMessage(" PlacaAnimation() - Posicion actual = " + transform.localPosition);

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
        float positionDistance = Vector3.Distance(targetPosition, transform.localPosition);
        float scaleDistance = Vector3.Distance(targetScale, transform.localScale);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, (1 / (positionDistance + LERP_SPEED_CONST)));
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, (1 / (scaleDistance + LERP_SPEED_CONST)));
    }

    #endregion

    #region Audio
    private void PlaySound(AudioClip sound)
    {
        audioController.PlayOneShotSound(sound);
    }

    private void PlaySound(AudioClip sound, float vol)
    {
        audioController.PlayOneShotSound(sound, vol);
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
