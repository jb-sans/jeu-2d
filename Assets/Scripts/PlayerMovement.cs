using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float climbSpeed;
    public float jumpForce;

    private bool isJumping;
    private bool isGrounded;
    [HideInInspector]
    public bool isClimbing;

    public Transform groundCheck;
    public Transform groundCheckTop;
    public float groundCheckRadius;
    public LayerMask collisionLayer;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D playerCollider;

    public Vector3 velocity = Vector3.zero;
    private float horizontalMovement;
    private float verticalMovement;

    public int GrvtCounter;

    public static PlayerMovement instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Plus d'une instance de PlayerMovement dans scène");
            return;
        }
        instance = this;
    }

    private void Update()
    {
        //Calcul de la vitesse de deplacement
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

        //Si le personnage est à terre
        if (isGrounded)
        {
            //Le compteur est mis à 1
            GrvtCounter = 1;
        }

        //Si le personnage saute, on décrémente
        if (Input.GetButtonDown("Jump"))
        {
            GrvtCounter--;
        }

        // Stack pour le changement de gravité \\
        if (Input.GetButtonDown("Jump") && GrvtCounter == 0 && !isGrounded)
        {
            rb.gravityScale *= -1;
            FlipY();
        }

        //Si on appuie sur le bouton de saut et que le personnage est au sol
        if (Input.GetButtonDown("Jump") && isGrounded && !isClimbing)
        {
            //Le personnage peut sauter
            isJumping = true;
        }

        //On change l'orientation (gauche ou droite) du personnage
        FlipX(rb.velocity.x);

        //Pour apliquer la vitesse à l'animation, on récupère la valeur absolue de la vitesse du RigedBody (vitesse négative si on va vers la gauche)
        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("isClimbing", isClimbing);
    }

    //Pas de récupération de Input dans la fonction fixedUpdate, toujours dans Update
    //Ici, on fait que ce qui concerne la physique
    void FixedUpdate()
    {
        //Traçage d'un cercle de centre groundCheck ou groundCheckTop pour voir si le cercle touche le sol 
        bool gCB = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);
        bool gCT = Physics2D.OverlapCircle(groundCheckTop.position, groundCheckRadius, collisionLayer);
        isGrounded = gCB || gCT;

        //Déplacement du joueur
        MovePlayer(horizontalMovement, verticalMovement);
    }

    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (!isClimbing)
        {
            //Création du vecteur de déplacement avec la force de mouvement et la vitesse verticale reste inchangée (pour les sauts par ex.)
            Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
            //Application de la vitesse de déplacement au personnage
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);

            if (isJumping)
            {
                if(rb.gravityScale > 0.1f )
                {
                    //Application d'une force verticale sur le joueur pour le saut
                    rb.AddForce(new Vector2(0f, jumpForce));
                } else if (rb.gravityScale < 0.1f)
                {
                    rb.AddForce(new Vector2(0f, -jumpForce));
                }
                    isJumping = false;
            }
        }
        else
        {
            Vector3 targetVelocity = new Vector2(0f, _verticalMovement);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
        }
    }

    void FlipX(float _velocity)
    {
        //Si on va vers la droite
        if (_velocity > 0.1f)
        {
            //Changement de l'orientation vers la droite
            spriteRenderer.flipX = false;
            //Sinon, si on va vers la gauche
        }
        else if (_velocity < -0.1f)
        {
            //Changement de l'orientation vers la gauche
            spriteRenderer.flipX = true;
        }

    }

    void FlipY()
    {
        if (rb.gravityScale < 0.1f)
        {
            spriteRenderer.flipY = true;
        }
        else if(rb.gravityScale > -0.1f)
        {
            spriteRenderer.flipY = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheckTop.position, groundCheckRadius);
    }

    public void StopPlayer()
    {
        this.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector3.zero;
        playerCollider.enabled = false;

    }

    public void UnstopPlayer()
    {
        this.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        playerCollider.enabled = true;

    }

}
