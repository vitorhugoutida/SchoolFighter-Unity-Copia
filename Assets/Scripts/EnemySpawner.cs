using Assets.Scripts;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject[] enemyArray;

    public int numberOfEnemies;
    public int currentEnemies;

    public float spawnTime;

    public string nextSection;

    void Update()
    {
        // Caso atinja o numero maximo de inimigos spawanados

        if (currentEnemies >= numberOfEnemies)
        {
            // Contar a quantidade de inimigos ativos na cena
            int enemies = FindObjectsByType<EnemyMeleeController>(FindObjectsSortMode.None).Length;

            if (enemies <= 0)
            {
                // Avança de secão
                LevelManager.ChangeSection(nextSection);

                // Desabilitar o spawner
                this.gameObject.SetActive(false);
            }
        }
    }

    private object FindAnyObjectByType<T>(FindObjectsSortMode none)
    {
        throw new System.NotImplementedException();
    }

    void SpawnEnemy()
    {
        // Posição de Spwan do inimigo

        Vector2 spawnPosition;

        // Limites Y
        // -0.32
        // -0.93

        spawnPosition.y = Random.Range(-0.93f, -0.32f);

        // Posição X máximo (direita) do confiner da camera + 1 de distancia
        // Pegar RightBound (Limite direito) da Section (Confiner) como base

        float rightSectionBound = LevelManager.currentConfiner.BoundingShape2D.bounds.max.x;

        // Define o x de spawnPosition, igual ao ponto da Direita do Confiner

        spawnPosition.x = rightSectionBound;

        // Instancia ("Spawna") os inimigos
        // Pega um inimigo aleatório da lista de inimigos
        // Spawna na posição spawnPosition
        // Quaternion é uma classe utilizada para trabalhar com rotações

        Instantiate(enemyArray[Random.Range(0, enemyArray.Length)], spawnPosition, Quaternion.identity).SetActive(true);

        // Incrementa o contador de inimigos do Spawner

        currentEnemies++;

        // Se o numero de inimigos atualmente na cena for menor que o numero maximo de inimigos,
        // Invoca novamente a função de spawn
        if (currentEnemies < numberOfEnemies)
        {
            Invoke("SpawnEnemy", spawnTime);
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        
        if (player)
        {
            // Desativa o colliior para inciar o Spawing apenas uma vez
            // ATENção: Desabilita o collider, mas o objeto Spawner continua ativo
            this.GetComponent<BoxCollider2D>().enabled = false;

            // Invoca pela primeira vez a função SpawnEnemy
            SpawnEnemy();
        }
    }
}
