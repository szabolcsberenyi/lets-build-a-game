using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcomeAbstractTester : MonoBehaviour
{
    public List<OutcomeTypeAbstractScriptable> outcomes = new List<OutcomeTypeAbstractScriptable>();

    private void Start()
    {
        // Test to see if outcometype is successfully set
        foreach (OutcomeTypeAbstractScriptable entry in outcomes)
        {
            Debug.Log(entry.outcomeType);
        }
    }
}
