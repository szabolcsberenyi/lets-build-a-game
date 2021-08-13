/*
 * SCOPE
 * Maintains a list of map points that have been reached and displays them
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapProgress : MonoBehaviour
{
    [Tooltip("All map points the player has reached")]
    [SerializeField] private List<MapPointScriptable> mapPointsReached = new List<MapPointScriptable>();

    [Tooltip("A prefab for progress images")]
    [SerializeField] private GameObject imagePrefab;


    private void Awake()
    {
        // Uncomment out below to automatically show map progress when the OverworldMap canvas is enabled
        ShowMapProgress();
    }

    // TODO: This needs to get called by the UI somewhere? Or we can have it done automatically when map canvas is enabled
    public void ShowMapProgress()
    {
        Debug.Log("ShowMapProgress called");
        
        // Instantiate each progress sprite as UI image
        foreach (MapPointScriptable entry in mapPointsReached)
        {
            GameObject newObject = Instantiate(imagePrefab, transform);
            newObject.GetComponent<Image>().sprite = entry.mapProgressImage; 
        }
    }
}
