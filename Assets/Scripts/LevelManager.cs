using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelManager : MonoBehaviour
    {
        public static CinemachineConfiner2D currentConfiner;

        private CinemachineBrain brain;
        private CinemachineCamera cam;

        private static BoxCollider2D currentSection;

        void Start()
        {
            brain = CinemachineBrain.GetActiveBrain(0);
            currentConfiner = GameObject.Find("CM").GetComponent<CinemachineConfiner2D>();
        }
        // Método para mudar o confiner da camera
        public static void ChangeSection(string sectionName)
        {

            // Procura pelo objeto que contem o nome (sectionName),
            // E pega o colisor dele para ser o novo confiner2D

            currentSection = GameObject.Find(sectionName).GetComponent<BoxCollider2D>();

            // Se o objeto for encontrado e tiver o colisor
            if (currentSection)
            {
                //Faz com que a camera limpe o cache do confiner anterior, esquece o confiner anterior
                currentConfiner.InvalidateBoundingShapeCache();

                // Define o novo confiner da camera 

                currentConfiner.BoundingShape2D = currentSection;

                // Reposicionar o Right Limiter, alinhado max x do confiner (direita do confiner)

                GameObject rightLimiter = GameObject.Find("Right");

                // Vector 3 (x, y ,z)

                rightLimiter.transform.position = new Vector3(currentConfiner.BoundingShape2D.bounds.max.x, rightLimiter.transform.position.y);
            }
        }

    }
}