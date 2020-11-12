using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaControl : MonoBehaviour
{
    public Color ActualColor { get; set; }
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public void ChangeMaterialColor() {
        ActualColor = material.color;
        material.SetColor("_Color", new Color32(0,104,111,255));
    }

    public void SetOriginalMaterialColor() {
        material.SetColor("_Color", ActualColor);
    }
}
