using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaControl : MonoBehaviour
{
    public Color ActualColor { get; set; }
    private Material material;
    private Color32 newColor;

    /*
        Recibe el material desde el componente Renderer
     */
    void Start()
    {
        material = GetComponent<Renderer>().material;
        newColor = new Color32(0, 104, 111, 255);
    }

    /*
        Cambia el color original del material por el color "newColor"
     */
    public void ChangeMaterialColor() {
        ActualColor = material.color;
        material.SetColor("_Color", newColor);
    }

    /*
        Devuelve el color de material original
     */
    public void SetOriginalMaterialColor() {
        material.SetColor("_Color", ActualColor);
    }
}
