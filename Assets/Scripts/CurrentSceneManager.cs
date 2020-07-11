using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
    public bool isPlayerPresenceByDefault = false;
    public int coinsPickedUpInThisSceneCount;

    public static CurrentSceneManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Plus d'une instance de CurrentSceneManager dans scène");
            return;
        }
        instance = this;
    }
}
