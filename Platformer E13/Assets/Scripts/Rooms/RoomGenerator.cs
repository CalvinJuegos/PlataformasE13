using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> StartingChunkPrefabs;
    public List<GameObject> ProceduralChunkPrefabs;
    public List<GameObject> EndingChunkPrefabs;
    public int chunkLevel = 3; // Number of procedural chunks before the ending room

    void Start()
    {
        GenerateRoom();
    }

    void GenerateRoom()
    {
        // Implement random number for choosing elements from the list of prefabs
        // Instantiate the StartingChunk with the player at the specified spawn point
        GameObject startingChunk = Instantiate(StartingChunkPrefabs[0], transform.position, Quaternion.identity);

        // Get the player spawn point from the starting chunk
        Transform playerSpawnPoint = startingChunk.transform.Find("PlayerSpawnPoint");
        Instantiate(player, playerSpawnPoint.position, Quaternion.identity);

        // Generate procedural chunks based on the chunkLevel
        GameObject previousChunk = startingChunk;
        // Instantiate the procedural chunks based on the previous chunk
        for (int i = 0; i < chunkLevel; i++)
        {
            foreach (Transform anchor in previousChunk.transform.Find("Anchors"))
            {
                int randomIndex = Random.Range(0, ProceduralChunkPrefabs.Count);
                // FOR PROCEDURAL GENERARTION; A stored variable should be able to tell the next chunk if player is coming from the top or bottom of the previous chunk
                // Instead of list of prefabs it should call a function that returns the prefab based on the previous chunk and the player's position
                GameObject proceduralChunk = Instantiate(ProceduralChunkPrefabs[randomIndex], anchor.position, Quaternion.identity);
                // Use GenerateChunk(anchor) function to generate the chunk
            }
        }

        // Instantiate the EndingChunk based on the last procedural chunk
        Instantiate(EndingChunkPrefabs[0], previousChunk.transform.Find("Anchors").GetChild(0).position, Quaternion.identity);
    }

    void GenerateChunk(Vector3 position)
    {
        // Instantiate the procedural chunks based on the previous chunk

    }
}