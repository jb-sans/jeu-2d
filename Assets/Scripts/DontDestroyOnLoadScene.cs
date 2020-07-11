using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadScene : MonoBehaviour
{
    public GameObject[] objects;

    public static DontDestroyOnLoadScene instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Plus d'une instance de DontDestroyOnLoadScene dans scène");
            return;
        }
        instance = this;

        foreach (var element in objects)
        {
            DontDestroyOnLoad(element);
        }
    }

    //Déplacement des objets dans le tab dans la scène actuelle
    //pour éviter la duplication de ces objets lors du respawn
    //du joueur
    public void RemoveFromDontDestroyOnLoad()
    {
        foreach (var element in objects)
        {
            SceneManager.MoveGameObjectToScene(element, SceneManager.GetActiveScene());
        }
    }
}
