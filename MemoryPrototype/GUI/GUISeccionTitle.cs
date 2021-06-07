using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using MemoryPrototype.Logs;
using MemoryPrototype.Utils;
using Audio;

namespace MemoryPrototype.Gui
{
    public class GUISeccionTitle : MonoBehaviour
    {
        private const float MAX_SCALE = 1.2f;
        private const float LERP_SPEED_CONST = 2f;                      //Constante de velocidad de la interpolacion lineal

        [SerializeField] private Transform tapText;
        [SerializeField] private CorroutinesUtils corroutinesUtils;
        [SerializeField] private InputControlUtils inputControlUtils;
        [SerializeField] private AudioClip goToPlaySound;

        private AudioController audioController;
        private Vector3 initialScale;
        private Vector3 targetScale;

        public delegate void GoToInstrucciones();                             //Delegado para el evento
        public static event GoToInstrucciones OnTapClickOnScreen;         //Evento para avisar que la cuenta de GO atras ha terminado

        private void Start()
        {
            initialScale = tapText.localScale;
            targetScale = new Vector3(MAX_SCALE, MAX_SCALE, tapText.localScale.z);
            audioController = GetComponent<AudioController>();
            StartCoroutine(corroutinesUtils.TextScaleAnimation(initialScale, targetScale, LERP_SPEED_CONST, tapText));            
        }

        // Update is called once per frame
        void Update() 
        { 
            if(inputControlUtils.InputControl()) 
            {
                audioController.PlayOneShotSound(goToPlaySound);
                OnTapClickOnScreen();
            }
        }
    }
}

