using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{
    private bool isInRange;
    private PlayerMovement playerMovement;
    public BoxCollider2D platformCollider;
    public Text interactUI;

    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //La première condition est là pour gérer le cas où on a plusierus échelles
        if (isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.isClimbing = false;
            platformCollider.isTrigger = false;
            //Update est appelé à chaque frame donc toute la fonction est exécutée
            //tout le temps, ainsi le if suivant sera exécuté aussi, on appuie sur
            //la touche E et on est dans la zone, on met alors un return pour empecher
            //d'exécuter la fonction update à la frame où on fait ce morceau de code
            return;
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.isClimbing = true;
            platformCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUI.enabled = true;
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            playerMovement.isClimbing = false;
            platformCollider.isTrigger = false;
            interactUI.enabled = false;
        }
    }
}
