using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float timeOffset;
    public Vector3 posOffset;

    public bool isFollowingPlayer = true;

    private Vector3 velocity;

    public static CameraFollow instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Plus d'une instance de CameraFollow dans scène");
            return;
        }
        instance = this;
    }

    void Update()
    {
        if (isFollowingPlayer)
        {
            //Pour que la caméra bouge avec le joueur
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position + posOffset, ref velocity, timeOffset);
        } else
        {
            transform.position = transform.position;
        }
       
    }
}
