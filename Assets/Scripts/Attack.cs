using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;

    // SFX do player

    private AudioSource audioSource;

    public AudioClip hitSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Inicia o componente AudioSource do

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ao colidir, salva na variavel enemy, o inimigo que foi colidido
        EnemyMeleeController enemy = collision.GetComponent<EnemyMeleeController>();

        // ao colidir, salva na variavel player, o player que foi atingido
        PlayerController player = collision.GetComponent<PlayerController>();

        EnemyRanged enemyRanged = collision.GetComponent<EnemyRanged>();

        // Se a colisão foi com um inimigo
        if (enemyRanged != null)
        {
            // Inimigo recebe dano
            enemy.TakeDamage(damage);

            audioSource.clip = hitSound;
            audioSource.Play();
        }

        // Se a colisão foi com um player
        if (enemy != null)
        {
            // Player recebe dano
            player.TakeDamage(damage);

            audioSource.clip = hitSound;
            audioSource.Play();
        }
    }

}
