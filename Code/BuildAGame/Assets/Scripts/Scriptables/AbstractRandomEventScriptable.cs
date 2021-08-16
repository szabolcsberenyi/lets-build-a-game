/*
 * SCOPE
 * Defines common characteristics of all random events
 */

using UnityEngine;

public abstract class AbstractRandomEventScriptable : OutcomeTypeAbstractScriptable
{
    [Tooltip("What is this event called?")]
    public string eventName;

    [Tooltip("Description of the event")]
    public string eventDescription;
}
