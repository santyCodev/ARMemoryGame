using System.Collections.Generic;
using UnityEngine;
namespace MemoryPrototype.Data
{
    public class EstadisticasManager : MonoBehaviour
    {
        public static EstadisticasManager instance;
        
        private List<float> tiemposReaction;
        private List<float> tiemposAccurancy;

        public int AciertosSesion { get; set; }
        public int FallosSesion { get; set; }
        public int RecordAciertos { get; set; }
        public float MediaReaction { get; set; }
        public float MediaPrecision { get; set; }
        public float PercentPrecision { get; set; }
        public float RecordReaction { get; set; }
        public float RecordPrecision { get; set; }


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
        }
        public void SetReactionTime(double time) 
        { 
            tiemposReaction.Add((float)time);
            ShowTimes(tiemposReaction, "tiemposReaction");
        }
        public void SetAccuracyTime(double time) 
        { 
            tiemposAccurancy.Add((float)time);
            ShowTimes(tiemposAccurancy, "tiemposAccurancy");
        }

        public void EndSession()
        {
            if (RecordAciertos < AciertosSesion) { RecordAciertos = AciertosSesion; }
            
            MediaReaction = MediaCalc(tiemposReaction);
            MediaPrecision = MediaCalc(tiemposAccurancy);
            PercentPrecision = PercentValue(tiemposAccurancy, MediaPrecision);            

            if (RecordReaction == 0 || RecordReaction > MediaReaction) { RecordReaction = MediaReaction; }
            if (RecordPrecision < PercentPrecision) { RecordPrecision = PercentPrecision; }
            
        }

        public void SetInitialParameters()
        {
            AciertosSesion = 0;
            FallosSesion = 0;
            MediaReaction = 0;
            MediaPrecision = 0;
            PercentPrecision = 0;
        }

        private void ShowTimes(List<float> list, string timeType)
        {
            string times = "";
            foreach (var time in list) { times += time.ToString() + "," ; }
            Debug.Log(timeType + "= [" + times + "]");
        }

        private float MediaCalc(List<float> list)
        {
            float times = 0.0f;
            foreach (var time in list) { times = times + time; }
            return times / (list.Count);
        }

        private float MaxValueList(List<float> list)
        {
            float maxTime = 0.0f;
            foreach (var time in list) { if (maxTime < time) { maxTime = time; } }
            return maxTime;
        }

        private float PercentValue(List<float> list, float media)
        {
            float maxTime = MaxValueList(list);
            return (media * 100) / maxTime;
        }
    }
}

