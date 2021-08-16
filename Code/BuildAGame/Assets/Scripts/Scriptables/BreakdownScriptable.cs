/*
 * SCOPE
 * Scriptable object to create new breakdown events which link to a different scene
 */

using UnityEngine;
[CreateAssetMenu(fileName = "NewBreakdownEvent", menuName = "Random Event/Breakdown Event", order = 1)]
public class BreakdownScriptable : AbstractRandomEventScriptable
{
    [Tooltip("The index of the breakdown/repair scene to be called")]
    public int sceneIndex;

    private void OnEnable()
    {
        SetOutcomeType(OutcomeTypeEnum.Breakdown);
    }


    // Set the outcome type
    public override void SetOutcomeType(OutcomeTypeEnum type)
    {
        outcomeType = type;
    }
}