using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIBuscarPatron : MonoBehaviour
{
    private const string LINK_URL = "https://santicodev.wixsite.com/memorypathar";

    [SerializeField] private GameObject placas;
    [SerializeField] private GameObject character;

    public delegate void GameEvent();                           //Delegado para el evento
    public static event GameEvent GoToGame;
    
    public void PatronEncontrado() 
    {
        placas.SetActive(true);
        character.SetActive(true);
        GoToGame();             
    }

    public void GoToLink()
    {
        Application.OpenURL(LINK_URL);
    }
}
