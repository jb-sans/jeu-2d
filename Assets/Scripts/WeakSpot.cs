using UnityEngine;

public class WeakSpot : MonoBehaviour
{
    public GameObject objectToDestoy;

    //Penser à cocher isTrigger et au moins un des 2 objets concernés par la collision doit avoir un RigidBody
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si il y a une collision entre un objet taggé comme "Player" et notre boite de collision
        if (collision.CompareTag("Player"))
        {
            //Destruction de l'objet glissé dans l'interface Unity
            Destroy(objectToDestoy);
        }
    }
}