using MemoryPrototype.Logs;
using System.Collections;
using UnityEngine;

namespace MemoryPrototype.Character
{
    public class CharacterController : MonoBehaviour
    {
        public LogController logController;                 //Controlador de logs

        private GameObject[] stepPositions;                 //Coleccion de posiciones a recorrer
        private Vector3 nextPosition;                       //Siguiente posicion
        private float moveLerpTime = 1f;                    //Tiempo total de la interpolacion lineal
        private float currentLerpTime;                      //Tiempo actual de interpolacion lineal
        public bool CharacterStarted { get; set; }
        public bool CharacterEnded { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            this.CharacterEnded = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void PrepareForMovement(GameObject[] placas)
        {
            stepPositions = placas;
            StartCoroutine(MoveCharacter());
            this.CharacterStarted = true;
        }

        /*
            Corrutina donde el personaje recorre todas las posiciones de la coleccion.
         */
        private IEnumerator MoveCharacter()
        {

            SetFirstPosition(NewCharacterPosition(stepPositions[0].transform));
            MarkPlate(stepPositions[0]);

            for (int i = 1; i < stepPositions.Length; i++)
            {

                SetNextPosition(stepPositions[i].transform.position);
                LookAtNextPosition(nextPosition);

                currentLerpTime = 0;

                while (transform.localPosition.x != nextPosition.x || transform.localPosition.z != nextPosition.z)
                {
                    MoveToNextPosition();
                    yield return null;
                }
                MarkPlate(stepPositions[i]);
                logController.PrintInConsole("MoveCharacter() - Chara ha llegado al destino " + nextPosition);
                SetFirstPosition(nextPosition);
            }

            logController.PrintInConsole("MoveCharacter() - Chara ha terminado el recorrido ");
            this.CharacterStarted = false;

            yield return WaitForEnd();

            this.CharacterEnded = true;
        }

        private IEnumerator WaitForEnd()
        {
            float counter = 0;
            float waitTime = 0.5f;
            while (counter < waitTime)
            {
                counter += Time.deltaTime;
                yield return null;
            }
        }

        /*
            Cambia el color del material el cual el personaje ha pisado
         */
        private void MarkPlate(GameObject placa)
        {
            placa.GetComponent<PlacaControl>().ChangeMaterialColor();
        }

        /*
            Se llama desde la corrutina donde realiza el calculo de la interpolacion lineal entre la posicion
            actual del personaj y la posicion destino.
            - El tiempo actual de recorrido se compara con el tiempo total, si este llega a sobrepasarlo, se iguala al total
            - El tiempo de interpolacion es la operacion entre (currentLerpTime / moveLerpTime), cuando el resultado sea 1
                habra terminado el recorrido y el personaje estara en el 
        */
        private void MoveToNextPosition()
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= moveLerpTime) { currentLerpTime = moveLerpTime; }

            logController.PrintInConsole("MoveCharacter()- Moviendo chara a " + nextPosition);

            transform.localPosition = Vector3.Lerp(transform.position, nextPosition, (currentLerpTime / moveLerpTime));
        }

        /*
            Asigna la posicion parametro a la posicion del personaje, esta posicion tiene que ser la primera de todas las
            posiciones, o la siguiente posicion si el personaje ha llegado a ella
         */
        private void SetFirstPosition(Vector3 firstPosition)
        {
            transform.localPosition = firstPosition;
            logController.PrintInConsole("setFirstPosition()- Primera posicion " + transform.localPosition);
        }

        /*
            Asigna la siguiente posicion para que el personaje sepa a donde ir.
         */
        private void SetNextPosition(Vector3 position)
        {
            nextPosition = new Vector3(position.x, 0.5f, position.z);
            logController.PrintInConsole("SetNextPosition()- Se va a mover chara a " + nextPosition);
        }

        /*
            Situa al personaje en una posicion concreta, la posicion tiene que ajustar la variable Y del
            Vector3 a 0.5 para que el GameObject no se vea enterrado en el plano
         */
        private Vector3 NewCharacterPosition(Transform newPosition)
        {
            float positionY = newPosition.position.y + transform.position.y;
            Vector3 actualPosition = new Vector3(newPosition.position.x, positionY, newPosition.position.z);
            return actualPosition;
        }

        /*
            Hace que el personaje mire hacia una posicicion concreta
         */
        private void LookAtNextPosition(Vector3 position)
        {
            Vector3 newFinalPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(newFinalPosition);
            logController.PrintInConsole("LookAtPosition - Mirando a la posicion = " + newFinalPosition);
        }
    }
}

