using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Transform playerSpawn;
    private Animator fadeSystem;

    private void Awake()
    {
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        fadeSystem = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(ReplacePlayer(collision));
        }
    }

    private IEnumerator ReplacePlayer(Collider2D collision)
    {
        CameraFollow.instance.isFollowingPlayer = false;
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        CameraFollow.instance.isFollowingPlayer = true;
        collision.transform.position = playerSpawn.position;
    }

}
