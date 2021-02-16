using System.Collections.Generic;
using UnityEngine;
namespace MemoryPrototype.Data
{
    public class EstadisticasManager : MonoBehaviour
    {
        public static EstadisticasManager instance;
        private float mediaReaction;
        private float mediaPrecision;
        private float percentPrecision;
        private float recordReaction;
        private float recordPrecision;
        private List<float> tiemposReaction;
        private List<float> tiemposAccurancy;

        public int AciertosSesion { get; set; }
        public int FallosSesion { get; set; }
        public int RecordAciertos { get; set; }

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
            
            mediaReaction = MediaCalc(tiemposReaction);
            mediaPrecision = MediaCalc(tiemposAccurancy);
            percentPrecision = PercentValue(tiemposAccurancy, mediaPrecision);            

            if (recordReaction == 0 || recordReaction > mediaReaction) { recordReaction = mediaReaction; }
            if (recordPrecision < percentPrecision) { recordPrecision = percentPrecision; }

            Debug.Log("Record de aciertos = " + RecordAciertos);
            Debug.Log("Aciertos en esta sesion = " + AciertosSesion);
            Debug.Log("Fallos en esta sesion = " + FallosSesion);
            Debug.Log("Media de reaccion en esta sesion = " + mediaReaction.ToString("0.00") + " seconds");
            Debug.Log("Media de precision en esta sesion = " + percentPrecision + "%");
            Debug.Log("Record de reaccion = " + recordReaction.ToString("0.00") + " seconds");
            Debug.Log("Record de precision = " + recordPrecision + "%");

            AciertosSesion = 0;
            FallosSesion = 0;
            mediaReaction = 0;
            mediaPrecision = 0;
            percentPrecision = 0;
        }

        private void ShowTimes(List<float> list, string timeType)
        {
            string times = "";
            foreach (var time in list)
            {
                times += time.ToString() + "," ;
            }
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

