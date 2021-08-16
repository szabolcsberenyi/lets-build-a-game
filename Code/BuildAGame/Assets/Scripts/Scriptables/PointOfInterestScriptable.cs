/*
 * SCOPE
 * Scriptable object to create new point of interest events which have different outcomes depending on player choice
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPOIEvent", menuName = "Random Event/Point Of Interest", order = 1)]
public class PointOfInterestScriptable : AbstractRandomEventScriptable
{
    [Tooltip("Describe the situation here")]
    public string dialogueText;

    [Tooltip("List of possilbe player responses to choose from. If only one outcome is listed, player has no choice")]
    public List<OutcomeTypeAbstractScriptable> dialogueResponses = new List<OutcomeTypeAbstractScriptable>();


    private void OnEnable()
    {
        SetOutcomeType(OutcomeTypeEnum.PointOfInterest);
    }


    // Set the outcome type
    public override void SetOutcomeType(OutcomeTypeEnum type)
    {
        outcomeType = type;
    }
}
