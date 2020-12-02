using MemoryPrototype.Logs;
using System.Collections;
using UnityEngine;

namespace MemoryPrototype.Character
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private LogController logController;       //Controlador de logs

        private GameObject[] positionsToWalk;                       //Coleccion de posiciones a recorrer
        private Vector3 nextPosition;                               //Siguiente posicion
        private float currentLerpTime;                              //Tiempo actual de interpolacion lineal
        private const float MOVE_LERP_TIME = 3;                     //Tiempo total de la interpolacion lineal
        private const string CLASS_NAME = "CHARACTER CONTROLLER";
        
        public delegate void FinishAction();                        //Delegado para el evento
        public static event FinishAction OnCharacterFinish;         //Evento para avisar que el character ha terminado
        
        #region Prepare for movement
        /*
            Prepara el personaje para su movimiento
            - Asigna las posiciones a recorrer
            - Coloca al personaje en la primera posicion
        */
        public void PrepareForMovement(GameObject[] placas)
        {
            positionsToWalk = placas;
            SetFirstPosition(NewPosition(positionsToWalk[0].transform.position));
            PrintMessage(" PrepareForMovement() - Placas a recorrer =  "+placas.Length);
        }

        /*
            Asigna al personaje la primera posicion de las placas
         */
        private void SetFirstPosition(Vector3 firstPosition)
        {
            transform.localPosition = firstPosition;
            PrintMessage(" setFirstPosition() - Primera posicion " + transform.localPosition);
        }

        /*
           Situa al personaje en una posicion concreta, la posicion tiene que ajustar la variable Y del
           Vector3 a 0.5 para que el GameObject no se vea enterrado en el plano
        */
        private Vector3 NewPosition(Vector3 position)
        {
            return new Vector3(position.x, 0.5f, position.z);
        }
        #endregion

        #region Character movement

        /*
            Inicia la corrutina de movimiento del personaje
         */
        public void MoveCharacter()
        {
            StartCoroutine(InitMovement());
        }
        
        /*
            El personaje recorre todas las posiciones.
            Cuando termina el recorrido dispara el evento OnCharacterFinish
         */
        private IEnumerator InitMovement()
        {
            MarkPlate(positionsToWalk[0]);

            for (int i = 1; i < positionsToWalk.Length; i++)
            {
                nextPosition = NewPosition(positionsToWalk[i].transform.position);
                LookAtNextPosition(nextPosition);

                currentLerpTime = 0;

                while (transform.localPosition.x != nextPosition.x || transform.localPosition.z != nextPosition.z)
                {
                    MoveToNextPosition();
                    yield return null;
                }
                MarkPlate(positionsToWalk[i]);
                PrintMessage(" MoveCharacter() - Chara ha llegado al destino " + nextPosition);
                SetFirstPosition(nextPosition);
            }
            PrintMessage(" MoveCharacter() - Chara ha terminado el recorrido ");

            yield return new WaitForSeconds(0.5f);
            OnCharacterFinish();
        }

        /*
            Calculo de la interpolacion lineal entre la posicion actual del personaje y la posicion destino.
            - El tiempo actual de recorrido se compara con el tiempo total, 
                si este llega a sobrepasarlo, se iguala al total
            - El tiempo de interpolacion es la operacion entre (currentLerpTime / MOVE_LERP_TIME), 
                cuando el resultado sea 1 habra terminado el recorrido
        */
        private void MoveToNextPosition()
        {
            currentLerpTime += Time.deltaTime;
            if (currentLerpTime >= MOVE_LERP_TIME) { currentLerpTime = MOVE_LERP_TIME; }
            PrintMessage(" MoveToNextPosition() - Moviendo chara a " + nextPosition);
            transform.localPosition = Vector3.Lerp(transform.position, nextPosition, (currentLerpTime / MOVE_LERP_TIME));
        }

        /*
            Hace que el personaje mire hacia una posicicion concreta
         */
        private void LookAtNextPosition(Vector3 position)
        {
            Vector3 newFinalPosition = new Vector3(position.x, transform.position.y, position.z);
            transform.LookAt(newFinalPosition);
            PrintMessage(" LookAtNextPosition() - Mirando a la posicion = " + newFinalPosition);
        }

        /*
            Cambia el color del material el cual el personaje ha pisado
         */
        private void MarkPlate(GameObject placa)
        {
            PrintMessage(" MarkPlate()- Marcando placa " + placa.transform.position);
            placa.GetComponent<PlacaControl>().ChangeMaterialColor();
        }

        #endregion

        #region Estado del personaje
        /*
            Activa o desactiva al personaje
         */
        public void SetActiveCharacter(bool option) {
            gameObject.SetActive(option);
            PrintMessage(" SetActiveCharacter() - Personaje activado = "+GetIsActive());
        }

        /*
            Devuelve si el personaje esta activo o no
         */
        public bool GetIsActive()
        {            
            return gameObject.activeSelf;
        }
        #endregion

        #region Gestion de Logs
        /*
            Usa el controlador de logs para imprimir un mensaje en consola
         */
        private void PrintMessage(string message)
        {
            logController.PrintInConsole(CLASS_NAME + message);
        }
        #endregion

    }//End Class
}

