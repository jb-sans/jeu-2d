using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public Grid grid;

    private Transform playerTransform;
    private float spawnX = 0.0f;
    private float tileLength = 22.0f;
    private int tilesOnScreen = 2;

    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = 0; i < tilesOnScreen; i++)
        {
            SpawnTile();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(playerTransform.position.x > (spawnX - tilesOnScreen * tileLength))
        {
            SpawnTile();
        }
    }

    private void SpawnTile(int prefabIndex = -1)
    {
        GameObject go = Instantiate(tilePrefabs[0]) as GameObject;
        go.transform.SetParent(grid.transform);
        go.transform.position = Vector2.right * spawnX;
        spawnX += tileLength;
    }

}
