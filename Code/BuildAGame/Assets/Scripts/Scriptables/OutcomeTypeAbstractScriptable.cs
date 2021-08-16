/*
 * SCOPE
 * An abstract class to be inherited by any scriptable objects that can be used as an outcome type
 */

using UnityEngine;

public abstract class OutcomeTypeAbstractScriptable : ScriptableObject
{
    [Tooltip("The chance for this event to be drawn 0 to 10")]
    [Range(0, 10)]
    public int startingDrawChance = 5;

    public OutcomeTypeEnum outcomeType = OutcomeTypeEnum.Undefined;

    public abstract void SetOutcomeType(OutcomeTypeEnum type);
}
