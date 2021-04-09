using MemoryPrototype.Logs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MemoryPrototype.Utils
{    public class CorroutinesUtils : MonoBehaviour
    {
        private const string CLASS_NAME = "SECTION TITLE";              //Constante con el nombre de la clase
        [SerializeField] private LogController logController;           //Controlador de logs    

        public IEnumerator TextScaleAnimation(Vector3 initialScale, Vector3 targetScale, float lerpSpeedConst, Transform tapText)
        {
            PrintMessage(" TextAnimation() - START");
            Vector3 startScale = initialScale;
            Vector3 endScale = targetScale;
            float time = 0;
            bool isCreciente = true;

            while (DistanceIsOk(endScale, tapText.localScale))
            {
                PrintMessage(" TextAnimation() - Distance is OK =  true");
                float scaleDistance = Vector3.Distance(endScale, tapText.localScale);
                tapText.localScale = Vector3.Lerp(tapText.localScale, endScale, (time / (scaleDistance + lerpSpeedConst)));
                time += Time.deltaTime;

                if (!DistanceIsOk(endScale, tapText.localScale))
                {
                    PrintMessage(" TextAnimation() - Distance is OK =  false");
                    tapText.localScale = endScale;
                    if (isCreciente) { SetScaleParameters(ref startScale, ref endScale, ref time, ref isCreciente, targetScale, initialScale); }
                    else { SetScaleParameters(ref startScale, ref endScale, ref time, ref isCreciente, initialScale, targetScale); }
                }

                yield return null;
            }
        }

        private void SetScaleParameters(ref Vector3 startScale, ref Vector3 endScale, ref float time, ref bool isCreciente, Vector3 initialScale, Vector3 targetScale)
        {
            startScale = initialScale;
            endScale = targetScale;
            time = 0;
            isCreciente = !isCreciente;
        }

        private bool DistanceIsOk(Vector3 targetScale, Vector3 actualScale)
        {
            return (Vector3.Distance(targetScale, actualScale) > Vector3.kEpsilon);
        }

        #region Gestion de Logs        
        /*
            Usa el controlador de logs para imprimir un mensaje en consola
         */
        private void PrintMessage(string message)
        {
            logController.PrintInConsole(CLASS_NAME + message);
        }
        #endregion
    }
}

