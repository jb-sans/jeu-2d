﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;

    private bool isJumping;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask collisionLayer;

    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;

    private void Update()
    {
        //Calcul de la vitesse de deplacement
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        //Si on appuie sur le bouton de saut et que le personnage est au sol
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //Le personnage peut sauter
            isJumping = true;
        }

        //On change l'orientation (gauche ou droite) du personnage
        Flip(rb.velocity.x);

        //Pour apliquer la vitesse à l'animation, on récupère la valeur absolue de la vitesse du RigedBody (vitesse négative si on va vers la gauche)
        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    //Pas de récupération de Input dans la fonction fixedUpdate, toujours dans Update
    //Ici, on fait que ce qui concerne la physique
    void FixedUpdate()
    {
        //Traçage une ligne entre groundCheckLeft et groundCheckRight pour voir si cette ligne touche le sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayer);
        
        //Déplacement du joueur
        MovePlayer(horizontalMovement);
    }

    void MovePlayer(float _horizontalMovement)
    {
        //Création du vecteur de déplacement avec la force de mouvement et la vitesse verticale reste inchangée (pour les sauts par ex.)
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        //Application de la vitesse de déplacement au personnage
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);

        if (isJumping)
        {
            //Application d'une force verticale sur le joueur pour le saut
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }
    }

    void Flip(float _velocity)
    {
        //Si on va vers la droite
        if (_velocity > 0.1f)
        {
            //Changement de l'orientation vers la droite
            spriteRenderer.flipX = false;
        //Sinon, si on va vers la gauche
        } else if (_velocity < -0.1f)
        {
            //Changement de l'orientation vers la gauche
            spriteRenderer.flipX = true;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}
