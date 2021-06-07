using System.Collections.Generic;
using UnityEngine;
namespace MemoryPrototype.Data
{
    public class EstadisticasManager : MonoBehaviour
    {
        public static EstadisticasManager instance;
        
        private List<float> tiemposReaction;
        private List<float> tiemposAccurancy;
        private List<int> listAciertos;
        private List<int> listFallos;
        public int AciertosSesion { get; set; }
        public int FallosSesion { get; set; }
        public int RecordAciertos { get; set; }
        public int RecordFallos { get; set; }
        public float MediaAciertos { get; set; }
        public float MediaFallos { get; set; }
        public float AciertosPerc { get; set; }
        public float FallosPerc { get; set; }
        public bool SetIsExitInfo { get; set; }

        private void Awake()
        {
            if (instance == null) { instance = this; }
            else if (instance != this) { Destroy(gameObject); }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            tiemposReaction = new List<float>();
            tiemposAccurancy = new List<float>();
            listAciertos = new List<int>();
            listFallos = new List<int>();
            SetInitialParameters();
        }
        public void SetReactionTime(double time) 
        { 
            tiemposReaction.Add((float)time);
        }
        public void SetAccuracyTime(double time) 
        { 
            tiemposAccurancy.Add((float)time);
        }      

        public void EndSession()
        {
            if (RecordAciertos < AciertosSesion) { RecordAciertos = AciertosSesion; }
            if (RecordFallos < FallosSesion) { RecordFallos = FallosSesion; }

            listAciertos.Add(AciertosSesion);
            listFallos.Add(FallosSesion);

            MediaAciertos = MediaCalcInt(listAciertos);
            MediaFallos = MediaCalcInt(listFallos);

            CalculatePercentSession();
        }

        private void CalculatePercentSession()
        {
            int sumaAciertosFallos = (AciertosSesion + FallosSesion);
            AciertosPerc = (AciertosSesion * 100) / sumaAciertosFallos;
            FallosPerc = (FallosSesion * 100) / sumaAciertosFallos;
        }

        public void SetInitialParameters()
        {
            AciertosSesion = 0;
            FallosSesion = 0;
            AciertosPerc = 0;
            FallosPerc = 0;
            RecordAciertos = 0;
            RecordFallos = 0;
            MediaAciertos = 0;
            MediaFallos = 0;
        }

        
        private float MediaCalcInt(List<int> list)
        {
            float nums = 0.0f;
            foreach (var item in list) { nums = nums + item; }
            return nums / (list.Count);

        }
    }
}

