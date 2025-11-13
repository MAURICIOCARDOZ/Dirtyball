using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class IsometricFollow : MonoBehaviour
    {
        // El objetivo que estamos siguiendo
        [SerializeField]
        private Transform target;

        // La distancia que queremos que la cámara mantenga
        [SerializeField]
        private float distance = 10.0f;

        // La altura que queremos que la cámara tenga sobre el objetivo
        [SerializeField]
        private float height = 5.0f;

        // El ángulo fijo de la cámara en el eje X (inclinación)
        [SerializeField]
        private float angleX = 45.0f; 

        // El ángulo fijo de la cámara en el eje Y (rotación horizontal)
        [SerializeField]
        private float angleY = 45.0f;

        // La suavidad de la transición de la cámara
        [SerializeField]
        private float damping = 5.0f;

        private Vector3 offset;

        void Start()
        {
            // Calcula el desplazamiento inicial de la cámara
            // La rotación se aplica una vez para obtener la dirección fija
            Quaternion rotation = Quaternion.Euler(angleX, angleY, 0);
            offset = rotation * new Vector3(0, 0, -distance);
        }

        void LateUpdate()
        {
            // Salida temprana si no hay un objetivo
            if (!target)
                return;

            // La posición deseada es la del objetivo más el offset fijo
            Vector3 wantedPosition = target.position + offset;

            // La altura se ajusta para que la cámara no baje del suelo, si es necesario
            // pero el código original no lo hace, solo ajusta la altura sobre el target
            wantedPosition.y = target.position.y + height;

            // Interpola la posición actual de la cámara hacia la posición deseada
            transform.position = Vector3.Lerp(transform.position, wantedPosition, damping * Time.deltaTime);

            // Mantiene la rotación de la cámara fija.
            // Esto asegura que siempre mire desde el mismo ángulo.
            transform.rotation = Quaternion.Euler(angleX, angleY, 0);

            // También puedes usar LookAt si el ángulo es fijo y solo necesitas que mire al centro
            // transform.LookAt(target);
            // Sin embargo, para una vista isométrica completa, la línea de arriba es más precisa.
        }
    }
}