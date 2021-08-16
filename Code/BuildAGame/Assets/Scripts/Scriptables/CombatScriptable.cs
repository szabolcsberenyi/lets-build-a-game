/*
 * SCOPE
 * Scriptable object to create new combat events which link to a different scene
 */

using UnityEngine;


[CreateAssetMenu(fileName = "NewCombatEvent", menuName = "Random Event/Combat Event", order = 1)]
public class CombatScriptable : AbstractRandomEventScriptable
{
    [Tooltip("The index of the combat scene to be called")]
    public int sceneIndex;


    private void OnEnable()
    {
        SetOutcomeType(OutcomeTypeEnum.Combat);
    }


    // Set the outcome type
    public override void SetOutcomeType(OutcomeTypeEnum type)
    {
        outcomeType = type;
    }
}
