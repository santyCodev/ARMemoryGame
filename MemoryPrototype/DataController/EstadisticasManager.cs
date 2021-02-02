using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Data
{    public class EstadisticasManager : MonoBehaviour
    {
        private const string CLASS_NAME = "ESTADISTICAS MANAGER";
        public int AciertosTotales { get; set; }
        public int FallosTotales { get; set; }

        private void Awake()
        {
            AciertosTotales = 0;
            FallosTotales = 0;
        }
    }
}

