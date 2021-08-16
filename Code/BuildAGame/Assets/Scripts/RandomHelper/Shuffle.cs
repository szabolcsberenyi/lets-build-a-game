/*
************************************************************************************************************************************
* SHUFFLE v. 1.0
*      by GameDevPowerUp: https://www.GameDevPowerUp.com
*      Download/Contribute/Read the Wiki at the Github Repo: 
* 
* SUMMARY
*      Randomizes the order of a list of variables.
* 
* LICENSE
*      Unlimited use, modifications, and distribution for personal and commercial applications
*      Please leave attribution in the header when sharing
* 
* DEPENDENCIES
*      1).  Unity: https://Unity.com
* 
* HOOKS
*      Shuffle.List(listOfItems)
* 
* USE CASES
*      Randomize a list of objects. Great way to add random noise if you select items in order from a list. 
************************************************************************************************************************************ 
*/

using System.Collections.Generic;
using System;

public static class Shuffle
{
    public static List<T> List<T>(List<T> listToRandomize)
    {
        Random random = new Random();

        //Add an empty line to the list to use for shuffling
        listToRandomize.Add(listToRandomize[0]);

        for (int i = listToRandomize.Count - 1; i > 1; i--)
        {
            //Get the nmext random value
            int rnd = random.Next(i + 1);

            //Set the last list slot equal to a list entry at random position
            listToRandomize[listToRandomize.Count - 1] = listToRandomize[rnd];

            //Set the entry at random list position equal to the list entry at the current count position
            listToRandomize[rnd] = listToRandomize[i];

            //Set the list entry at current count position equal to the entry in the last list slot
            listToRandomize[i] = listToRandomize[listToRandomize.Count - 1];
        }

        //Remove last entry
        listToRandomize.RemoveAt(listToRandomize.Count - 1);

        return listToRandomize;
    }
}
