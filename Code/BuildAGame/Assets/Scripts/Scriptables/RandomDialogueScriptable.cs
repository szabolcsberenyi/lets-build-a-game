/*
 * SCOPE
 * Scriptable object to create new dialogue events 
 */

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueEvent", menuName = "Random Event/Random Dialogue", order = 1)]
public class RandomDialougeScriptable : AbstractRandomEventScriptable
{
    [Tooltip("Provide dialogue text here")]
    public long dialogueText;

    [Tooltip("Sprite for image of the character engaged in dialogue with the player")]
    public Sprite imageOfSpeaker;

    [Tooltip("List of possilbe player responses to choose from")]
    public List<OutcomeTypeAbstractScriptable> dialogueResponses = new List<OutcomeTypeAbstractScriptable>();


    private void OnEnable()
    {
        SetOutcomeType(OutcomeTypeEnum.Dialogue);
    }


    // Set the outcome type
    public override void SetOutcomeType(OutcomeTypeEnum type)
    {
        outcomeType = type;
    }
}

