/*
 * SCOPE
 * Used to create a new map point and store map progress information
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewMapPoint", menuName = "Map Point", order = 1)]
public class MapPointScriptable : OutcomeTypeAbstractScriptable
{
    [Tooltip("The name of this location")]
    public string mapPointName;

    [Tooltip ("A description of this location")]
    public string mapPointDescription;

    [Tooltip("Time in seconds it takes to get to this location from the last location")]
    [Range(0.0f, 600.0f)]
    public float travelTime;

    [Tooltip("An artistic representation of the location")]
    public Sprite mapPointImage;
    
    [Tooltip ("An image to display once the player has reached or passed this point on the map. Please size to full map size.")]
    public Sprite mapProgressImage;

    [Tooltip ("Any map points that come directly after this map point")]
    public List<MapPointScriptable> followingMapPoint = new List<MapPointScriptable>();

    [Tooltip("The position where this point should be initialized.")]
    public Vector2 mapPointPosition;


    private void OnEnable()
    {
        SetOutcomeType(OutcomeTypeEnum.MapPoint);
    }


    // Set the outcome type
    public override void SetOutcomeType(OutcomeTypeEnum type)
    {
        outcomeType = type;
    }
}
