using UnityEngine;
using UnityEngine.Rendering;

public class EnemyRanged : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    private bool facingRight;
    private bool previousDirectionRight;

    private bool isDead;

    private Transform target;

    private float enemySpeed = 0.3f;
    private float currentSpeed;

    private float verticalForce, horizontalForce;

    private bool isWalking = false;

    private float walkTimer;

    public int maxHealth;

    public int currentHealth;

    private float staggerTime = 0.5f;
    private bool isTakingDamage = false;
    private float damageTimer;

    private float attackRate = 1f;
    private float nextAttack;

    public Sprite enemyImage;

    // Variavel para armazenar o projetil
    public GameObject projectile;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Buscar o Player e armazenar sua posi��o
        target = FindAnyObjectByType<PlayerController>().transform;

        // Inicializar a velocidade do inimigo
        currentSpeed = enemySpeed;

        // Inicializar a vida do inimigo
        currentHealth = maxHealth;
    }


    void Update()
    {
        // Verificar se o Player est� para a Direita ou para a Esquerda
        // E com isso determinar o lado que o Inimigo ficar� virado
        if (target.position.x < this.transform.position.x)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }

        // Se facingRight for TRUE, vamos virar o inimigo em 180� no eixo Y,
        // Sen�o vamos virar o inimigo para a esquerda

        // Se o Player � direita e a dire��o anterior N�O era direita (inimigo olhando para esquerda)
        if (facingRight && !previousDirectionRight)
        {
            this.transform.Rotate(0, 180, 0);
            previousDirectionRight = true;
        }

        // Se o Player N�O est� � direita e a dire��o anterior ERA direita (inimigo olhando para direita)
        if (!facingRight && previousDirectionRight)
        {
            this.transform.Rotate(0, -180, 0);
            previousDirectionRight = false;
        }

        // Iniciar o timer do caminhar do inimigo
        walkTimer += Time.deltaTime;

        // Gerenciar a anima��o do inimigo
        if (horizontalForce == 0 && verticalForce == 0)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        // Gereciar o tempo de stagger
        if (isTakingDamage && !isDead)
        {
            damageTimer += Time.deltaTime;

            ZeroSpeed();

            if (damageTimer >= staggerTime)
            {
                isTakingDamage = false;
                damageTimer = 0;

                ResetSpeed();
            }
        }

        // Atualiza o animator
        UpdateAnimator();
    }

    void UpdateAnimator()
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            isTakingDamage = true;

            currentHealth -= damage;

            animator.SetTrigger("Hurt");

            // Atualiza a UI do inimigo
            FindFirstObjectByType<UIManager>().UpdateEnemyUI(maxHealth, currentHealth, enemyImage);

            if (currentHealth <= 0)
            {
                isDead = true;

                // Corrige o bug do inimigo deslizar ap�s morto
                rb.linearVelocity = Vector2.zero;

                animator.SetTrigger("Dead");
            }
        }
    }

    void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    void ResetSpeed()
    {
        currentSpeed = enemySpeed;
    }

    public void DisableEnemy()
    {
        // Desabilita este inimigo
        this.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            // MOVIMENTA��O

            // Variavel para armazenar a distancia entre o Inimigo e o Player
            Vector3 targetDistance = target.position - this.transform.position;

            // Determina se a for�a horizontal deve ser negativa ou positiva
            // 5 / 5     =   1
            // -5 / 5    =   -1
            //horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

            // Entre 1 e 2 segundos, ser� feita uma defini��o de dire��o vertical
            if (walkTimer >= Random.Range(2.5f, 3.5f))
            {
                verticalForce = targetDistance.y / Mathf.Abs(targetDistance.y);
                horizontalForce = targetDistance.x / Mathf.Abs(targetDistance.x);

                // Zera o timer de movimenta��o para andar verticalmente novamente daqui a +- 1 seg
                walkTimer = 0;
            }

            // Caso esteja perto do Player, parar a movimenta��o
            if (Mathf.Abs(targetDistance.x) < 1f)
            {
                horizontalForce = 0;
            }
            if (Mathf.Abs(targetDistance.y) < 0.5f)
            {
                verticalForce = 0;
            }

            if (!isTakingDamage)
            {
                // Aplica velocidade no inimigo fazendo o movimentar
                rb.linearVelocity = new Vector2(horizontalForce * currentSpeed, verticalForce * currentSpeed);
            }



            // ATAQUE
            // Se estiver perto do Player e o timer do jogo for maior que o valor de nextAttack
            if (Mathf.Abs(targetDistance.x) < 1.3f && Mathf.Abs(targetDistance.y) < 0.05f && Time.time > nextAttack)
            {
                // Executa anima��o de ataque
                animator.SetTrigger("Attack");

                ZeroSpeed();

                // Pega o tempo atual e soma o attackRate, para definir a partir de quando o inimigo poder� atacar novamente
                nextAttack = Time.time + attackRate;
            }
        }
    }

    public void Shoot()
    {
        // Define a posi��o do spawn do projetil

        Vector2 spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y + 0.2f);

        // Spawnar o projetil na posi��o definida
        GameObject shotObject = Instantiate(projectile, spawnPosition, Quaternion.identity);

        // Ativar o projetil
        shotObject.SetActive(true);

        var shotPhysics = shotObject.GetComponent<Rigidbody2D>();

        if (facingRight)
        {
            // Aplica for�a no projetil para ele se deslocar para a direita
            shotPhysics.AddForceX(80f);

        }
        else
        {
            // Aplica for�a no projetil para ele se deslocar para a esquerda
            shotPhysics.AddForceX(-80f);
        }
    }
}
