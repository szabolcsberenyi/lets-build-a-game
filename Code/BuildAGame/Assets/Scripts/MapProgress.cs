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

    [Tooltip("The object where the map points should be added to.")]
    [SerializeField] private Transform mapPointsRoot;

    [Tooltip("A prefab for map points")]
    [SerializeField] private GameObject mapPointImagePrefab;

    private void Awake()
    {
        // Uncomment out below to automatically show map progress when the OverworldMap canvas is enabled
        ShowMapProgress();
    }

    // TODO: This needs to get called by the UI somewhere? Or we can have it done automatically when map canvas is enabled
    /// <summary>
    /// ShowMapProgress is reponsible for interating every map point reached by the player and placing them in the screen
    /// </summary>
    public void ShowMapProgress()
    {
        Debug.Log("ShowMapProgress called");
        
        // Instantiate each progress sprite as UI image
        foreach (MapPointScriptable entry in mapPointsReached)
        {
            GameObject newObject = Instantiate(mapPointImagePrefab, Vector3.zero, Quaternion.identity, mapPointsRoot);
            newObject.transform.localPosition = entry.mapPointPosition;
            newObject.GetComponent<Image>().sprite = entry.mapProgressImage;

            foreach (MapPointScriptable nextPoint in entry.followingMapPoint) // For each next point in the points reached
            {
                if (!mapPointsReached.Contains(nextPoint)) // Check if it was not reached before
                {
                    GameObject newNextPoint = Instantiate(mapPointImagePrefab, Vector3.zero, Quaternion.identity, mapPointsRoot); // display in the not-reached version
                    newNextPoint.transform.localPosition = nextPoint.mapPointPosition;
                    //newObject.GetComponent<Image>().sprite = nextPoint.mapProgressImage; // once we get all the images to work with, we can start replacing the sprites
                }
            }
        }
    }

    /// <summary>
    /// Method to add visited points
    /// </summary>
    /// <param name="mapPointToAdd">Map Point to be added</param>
    public void AddVisitedMapPoint(MapPointScriptable mapPointToAdd)
    {
        mapPointsReached.Add(mapPointToAdd);
    }
}
