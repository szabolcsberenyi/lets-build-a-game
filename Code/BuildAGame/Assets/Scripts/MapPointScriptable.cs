/*
 * SCOPE
 * Used to create a new map point and store map progress information
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "NewMapPoint", menuName = "Map Point", order = 1)]
public class MapPointScriptable : ScriptableObject
{
    [Tooltip("The name of this location")]
    public string mapPointName;

    [Tooltip ("A description of this location")]
    public string mapPointDescription;

    [Tooltip("An image of the actual map point. Please size it to the map size and use a format that supports transparency.")]
    public Sprite mapPointImage;
    
    [Tooltip ("An image to display once the player has reached or passed this point on the map. Please size to full map size.")]
    public Sprite mapProgressImage;

    [Tooltip ("Any map points that come directly after this map point")]
    public List<MapPointScriptable> followingMapPoint = new List<MapPointScriptable>();
}
