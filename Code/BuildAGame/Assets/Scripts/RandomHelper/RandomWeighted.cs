/*
************************************************************************************************************************************
* BOX PROBABILITY v. 1.0
*      by GameDevPowerUp: https://www.GameDevPowerUp.com
*      Download/Contribute/Read the Wiki at the Github Repo: 
* 
* SUMMARY
*      Selects an item from a provided list based on weighted probability of selection. 
*      Does not require probabilities to equal 100%. 
* 
* LICENSE
*      Unlimited use, modifications, and distribution for personal and commercial applications
*      Please leave attribution in the header when sharing
* 
* DEPENDENCIES
*      1).  Unity: https://Unity.com
* 
* HOOKS
*      RandomWeighted.DrawItem(listOfItems)
*      RandomWeighted.ItemToDraw 
* 
* USE CASES
*      Use to randomly select an item from a list of available items, using a custom probability distribution.
*      Perfect for loot drops. 
************************************************************************************************************************************ 
*/

using System.Collections.Generic;

public static class BoxProbability
{
    //Select an item from a list of items and return the selected item
    public static T DrawItem<T>(Dictionary<T, float> itemAndProbability)
    {
        float totalValue = 0.0f;
        bool positiveCheck = false;

        foreach (KeyValuePair<T, float> entry in itemAndProbability)
        {
            totalValue += entry.Value;
            
            // Check for at least one positive value
            if (entry.Value >= 0)
            {
                positiveCheck = true;
            }
        }
        
        if(!positiveCheck)
        {
            return default(T);
        }

        float cumulativeProbability = 0.0f;
        float rng = UnityEngine.Random.value;

        // Iterate through the dictionary and determine which entry is selected
        foreach (KeyValuePair<T, float> entry in itemAndProbability)
        {
            cumulativeProbability += entry.Value / totalValue;

            if (cumulativeProbability >= rng)
            {
                return entry.Key;
            }
        }

        return default(T);
    }
}