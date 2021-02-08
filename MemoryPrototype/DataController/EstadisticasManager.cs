using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Data
{    public class EstadisticasManager : MonoBehaviour
    {        
        public static EstadisticasManager instance;
        [SerializeField] private int aciertosTotales;
        [SerializeField] private int fallosTotales;
        
        public int AciertosTotales { get { return aciertosTotales; } set { aciertosTotales = value; } }
        public int FallosTotales { get { return fallosTotales; } set { fallosTotales = value; } }
        private void Awake()
        {
            if (instance == null) { instance = this; }
            else if (instance != this) { Destroy(gameObject); }
            DontDestroyOnLoad(gameObject);
        }
    }
}

