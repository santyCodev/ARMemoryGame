using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MemoryPrototype.Data
{    public class EstadisticasManager : MonoBehaviour
    {
        public static EstadisticasManager instance;
        [SerializeField] private int aciertosTotales;
        [SerializeField] private int fallosTotales;
        [SerializeField] private int aciertosSesion;
        [SerializeField] private int fallosSesion;
        [SerializeField] private int maxAciertos;
        private float mediaReaction;
        private float mediaAccuracy;
        private float percentReaction;
        private float percentAccuracy;
        private List<float> tiemposReaction;
        private List<float> tiemposAccurancy;

        public int AciertosTotales { get { return aciertosTotales; } set { aciertosTotales = value; } }
        public int FallosTotales { get { return fallosTotales; } set { fallosTotales = value; } }
        public int AciertosSesion { get { return aciertosSesion; } set { aciertosSesion = value; } }
        public int FallosSesion { get { return fallosSesion; } set { fallosSesion = value; } }


        private void Awake()
        {
            if (instance == null) { instance = this; }
            else if (instance != this) { Destroy(gameObject); }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            instance.AciertosSesion = 0;
            instance.FallosSesion = 0;
            instance.tiemposReaction = new List<float>();
            instance.tiemposAccurancy = new List<float>();
        }
        public void setReactionTime(double time) { instance.tiemposReaction.Add((float)time); }
        public void setAccuracyTime(double time) { instance.tiemposAccurancy.Add((float)time); }

        public void EndSession()
        {
            if (instance.maxAciertos < instance.AciertosTotales) { instance.maxAciertos = instance.AciertosTotales; }
            instance.mediaReaction = MediaCalc(instance.tiemposReaction);
            instance.mediaAccuracy = MediaCalc(instance.tiemposAccurancy);
            instance.percentReaction = PercentValue(instance.tiemposReaction,instance.mediaReaction);
            instance.percentAccuracy = PercentValue(instance.tiemposAccurancy, instance.mediaAccuracy);
            Debug.Log("Media de reaccion = "+instance.mediaReaction + " seconds");
            Debug.Log("Media de precision = " + instance.percentAccuracy + "%");
            instance.tiemposAccurancy = null;
            instance.percentAccuracy = 0;
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
            float maxTime = instance.MaxValueList(list);
            return (media * 100) / maxTime;
        }

    }
}

