using MemoryPrototype.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MemoryPrototype.Gui
{
    public class GUIInstruccionesSection : MonoBehaviour
    {
        private const string INSTRUCTION_MESSAGE_1 = "Memorice el camino y toque en orden inverso las posiciones (de la ultima a la primera).";
        private const string INSTRUCTION_MESSAGE_2 = "Memorice el camino y toque en orden inverso las posiciones (de la ultima a la primera).";
        private const string INSTRUCTION_MESSAGE_3 = "Cada posicion correcta sera un acierto. Al completar el camino, la ultima posicion se tornará verde.";
        private const string INSTRUCTION_MESSAGE_4 = "Una posicion incorrecta sera un fallo, se indicara en rojo el bloque que ha tenido que tocar y tendra que volver a comenzar un nuevo camino.";
        private const string INSTRUCTION_MESSAGE_5 = "Si completa un camino 3 veces, se añadira al camino un bloque mas para memorizar.";
        private const string INSTRUCTION_MESSAGE_6 = "¿Cuantos bloques del camino es capaz de memorizar en un tiempo determinado?";

        private const float MAX_SCALE = 1.2f;
        private const float LERP_SPEED_CONST = 2f;                      //Constante de velocidad de la interpolacion lineal

        private List<string> instructionMessages;
        [SerializeField] private Sprite[] instructionImages;
        [SerializeField] private Image sourceImage;
        [SerializeField] private TextMeshProUGUI instruccionesText;
        [SerializeField] private Transform sigTextTransform;
        [SerializeField] private Transform antTextTransform;
        [SerializeField] private Transform jugarTextTransform;
        [SerializeField] private CorroutinesUtils corroutinesUtils;
        [SerializeField] private InputControlUtils inputControlUtils;
  
        public delegate void GoToGame();                             //Delegado para el evento
        public static event GoToGame OnTapClickOnGameButton;         //Evento para avisar que la cuenta de GO atras ha terminado

        private int indexImages;        

        private void Start()
        {
            Debug.Log("Instrucciones start");
            InitializeMessages();
            indexImages = 0;
            ActivateScreen();
            StartCorroutineText(sigTextTransform);
            StartCorroutineText(antTextTransform);
            StartCorroutineText(jugarTextTransform);
        }
        
        private void InitializeMessages()
        {
            instructionMessages = new List<string>
            {
                INSTRUCTION_MESSAGE_1,
                INSTRUCTION_MESSAGE_2,
                INSTRUCTION_MESSAGE_3,
                INSTRUCTION_MESSAGE_4,
                INSTRUCTION_MESSAGE_5,
                INSTRUCTION_MESSAGE_6
            };
        }

        private void StartCorroutineText(Transform textTransform)
        {
            Vector3 initialScale = textTransform.localScale;
            Vector3 targetScale = new Vector3(MAX_SCALE, MAX_SCALE, textTransform.localScale.z);
            StartCoroutine(corroutinesUtils.TextScaleAnimation(initialScale, targetScale, LERP_SPEED_CONST, textTransform));
        }

        public void OnTapJugarClick() 
        {
            //Se va al juego, lanza el evento
            OnTapClickOnGameButton();
        }
        public void OnTapSigClick() 
        {
            //Se va a la pantalla de instruccion siguiente
            indexImages++;
            ActivateScreen();
        }

        public void OnTapAntClick() 
        {
            //Se va a la pantalla de instruccion anterior
            indexImages--;
            ActivateScreen();
        }       

        private void ActivateScreen()
        {
            Debug.Log("Instrucciones activate screen");
            if (instructionImages.Length > 1)
            {
                if      (indexImages == 0) { SetScreenParameters(true, false, instructionMessages[0]); }
                else if (indexImages == instructionImages.Length - 1) { SetScreenParameters(false, true, instructionMessages[instructionImages.Length - 1]); }
                else    { SetScreenParameters(true, true, instructionMessages[indexImages]); }
            } 
        }

        private void SetScreenParameters(bool valueSig, bool valueAnt, string message)
        {
            Debug.Log("Instrucciones activate button message");
            sigTextTransform.gameObject.SetActive(valueSig);
            antTextTransform.gameObject.SetActive(valueAnt);
            sourceImage.sprite = instructionImages[indexImages];
            instruccionesText.text = message;
        }
    }
}

