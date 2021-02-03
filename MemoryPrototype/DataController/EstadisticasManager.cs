using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Data
{    public class EstadisticasManager : MonoBehaviour
    {        
        private static EstadisticasManager instance;
        public int AciertosTotales { get; set; }
        public int FallosTotales { get; set; }
        
        private void Start()
        {            
            DontDestroyOnLoad(gameObject);

            if(instance == null) { instance = this; }
            else if(instance != this){ Destroy(gameObject); }
        }
    }
}

