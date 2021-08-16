/*
 * SCOPE
 * Manages the progress and random encounters as player travels from one point to another
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JourneyManager : MonoBehaviour
{
    [Tooltip("List of all outcomes that can be drawn from for random encounters")]
    [SerializeField]
    private List<OutcomeTypeAbstractScriptable> outcomesToDrawFrom = new List<OutcomeTypeAbstractScriptable>();

    [Tooltip("On average, how many random encounters should occur every minute")]
    [SerializeField]
    [Range(0.0f, 60.0f)]
    private float averageRandomEventsPerMinute;

    private float timeTraveled = 0.0f;

    private MapPointScriptable currentTarget;

    private bool traveling = false;

    private MapProgress mapProgressReference;

    // Added to every event after each round of drawing
    private float probabilityStepValue = 1.0f;

    private Dictionary<OutcomeTypeAbstractScriptable, float> outcomeAndProbability = 
        new Dictionary<OutcomeTypeAbstractScriptable, float>();


    private void Awake()
    {
        // Get reference to MapProgress
        mapProgressReference = FindObjectOfType<MapProgress>();

        // Add each outcome to the dictionary
        foreach(OutcomeTypeAbstractScriptable entry in outcomesToDrawFrom)
        {
            outcomeAndProbability.Add(entry, entry.startingDrawChance);
        }
    }


    private void Update()
    {
        if (traveling)
        {
            timeTraveled += Time.deltaTime;

            // Check if the destination has been reached according to total travel time
            if (timeTraveled >= currentTarget.travelTime)
            {
                DestinationReached();
            }
            else
            {
                // Calculate chance of random encounter for this frame
                float totalProability = averageRandomEventsPerMinute / 60 * Time.deltaTime;

                // Determine if random draw is within left tail
                if(totalProability >= Random.Range(0.0f, 1.0f) )
                {
                    DrawRandomOutcome();
                }
            }
        }
    }

    // TODO: Do we need method to add/remove outcomes at runtime?


    private void DestinationReached()
    {
        traveling = false;

        // TODO: Load location image to UI?

        // Tell MapProgress player has reached target
        mapProgressReference.AddVisitedMapPoint(currentTarget);
    }


    // Called by outside script to initiate travel
    public void ProceedToNextMapPoint(MapPointScriptable nextMapPoint)
    {
        timeTraveled = 0.0f;
        currentTarget = nextMapPoint;
        traveling = true;
    }


    // Called if a random encounter is drawn
    private void DrawRandomOutcome()
    {
        // Select a winner using box probability model
        OutcomeTypeAbstractScriptable drawnItem = BoxProbability.DrawItem(outcomeAndProbability);

        ExecuteRandomOutcome(drawnItem);

        Dictionary<OutcomeTypeAbstractScriptable, float> tempDictionary = 
            new Dictionary<OutcomeTypeAbstractScriptable, float>();

        foreach (KeyValuePair<OutcomeTypeAbstractScriptable, float> entry in outcomeAndProbability)
        {
            // check if the current entry is the drawn item
            if(entry.Key == drawnItem)
            {
                tempDictionary.Add(entry.Key, 0.0f);
            }
            else
            {
                tempDictionary.Add(entry.Key, entry.Value + probabilityStepValue);
            }

            // Add modified items back to dictionary
            outcomeAndProbability = tempDictionary;
        }
    }


    //TODO: Pause traveling when launching minigames
    private void ExecuteRandomOutcome(OutcomeTypeAbstractScriptable outcome)
    {
        switch(outcome.outcomeType)
        {
            case OutcomeTypeEnum.Combat:
                // Load combat scene
                // CombatScriptable combatOutcome = outcome as CombatScriptable;
                // SceneManager.LoadScene(combatOutcome.sceneIndex);

                Debug.Log("Combat Event Identified");
                break;

            case OutcomeTypeEnum.Breakdown:
                // Load breakdown scene
                // BreakdownScriptable breakdownOutcome = outcome as BreakdownScriptable;
                // SceneManager.LoadScene(breakdownOutcome.sceneIndex);

                Debug.Log("Breakdown Event Identified");
                break;

            case OutcomeTypeEnum.Dialogue:
                // execute some code
                Debug.Log("Dialogue Event Identified");
                break;

            case OutcomeTypeEnum.MapPoint:
                // Get taken immediately to target destination
                // DestinationReached();

                Debug.Log("MapPoint Event Identified");
                break;

            case OutcomeTypeEnum.LootGain:
                // execute some code
                Debug.Log("LootGain Event Identified");
                break;

            case OutcomeTypeEnum.LootLoss:
                // execute some code
                Debug.Log("LootLoss Event Identified");
                break;

            case OutcomeTypeEnum.PointOfInterest:
                // execute some code
                Debug.Log("PointOfInterest Event Identified");
                break;
        }
    }


    //TODO: Game Pause Method

    // Pauses travel counter for pause menu, UI display, encounter or other interruption
    private void PauseTravel()
    {
        traveling = false;
    }


    // Resumes travel... obviously
    private void ResumeTravel()
    {
        traveling = true;
    }

    public struct OutcomeAndProbability
    {
        AbstractRandomEventScriptable outcomeReference;
        int probabilityOfOutcome;
    }
}



