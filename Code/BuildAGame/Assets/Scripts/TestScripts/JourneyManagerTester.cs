using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyManagerTester : MonoBehaviour
{
    public MapPointScriptable mapPointRef;
    public JourneyManager journeyManagerRef;

    private void Start()
    {
        journeyManagerRef.ProceedToNextMapPoint(mapPointRef);
    }
}
