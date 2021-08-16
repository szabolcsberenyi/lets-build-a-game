/*
************************************************************************************************************************************
* RANDOM NORMAL GENERATOR v. 1.0
*      by GameDevPowerUp: https://www.GameDevPowerUp.com
*      Download/Contribute/Read the Wiki at the Github Repo: 
* 
* SUMMARY
*      Provides normally-distributed random values with automatic error correcting and error messaging.
*      Learn more about normal vs. uniform distribution: https://www.youtube.com/watch?v=svKjZesrRkA
* 
* LICENSE
*      Unlimited use, modifications, and distribution for personal and commercial applications
*      Please leave attribution in the header when sharing
* 
* DEPENDENCIES
*      1).  Unity: https://Unity.com
*      2).  Open-source library Meta.Numerics: http://www.meta-numerics.net/
*           Download, unzip and move the files into the assets folder of your Unity Project
* 
* HOOKS
*      RandomNormal.Generate(average, standardDeviation)
*      RandomNormal.Generate(average, standardDeviation, howMany)
*      RandomNormal.Generate(average, standardDeviation, minimumBound, maximumBound)
*      RandomNormal.Generate(average, standardDeviation, minimumBound, maximumBound, howMany)
* 
* USE CASES
*      Use as a replacement for uniformly-distributed random number generators, when normal behavior is required.
*      This allows for the use of rare and common events, and more closely mimics the behavior of many real-world systems.
*      Use in loot drops, enemy stats, crafting material attributes and more!
************************************************************************************************************************************ 
*/

using UnityEngine;
using Meta.Numerics.Statistics.Distributions;
using System.Collections.Generic;

public static class RandomNormal
{
    //Minimum difference between min and max bounds
    private static double minDifference = 0.0000001; 

    //Maximum number of SDs a bound can be from the mean
    private static double maxSds = 10.0;

    //Minimum random value to prevent returning negative infinity
    private static float minRandom = 0.0000000001f;

    //Maximum random value to prevent returning positive infinity
    private static float maxRandom = 0.9999999999f; 

    private struct boundaries
    {
        public double minimumBound;
        public double maximumBound;   
    }


    //Generates a single normally-distributed random value with a given standard deviation and average
    public static double Generate(double average, double standardDeviation)
    {
        //Ensures SD is greater than 0
        standardDeviation = StandardDeviationCheck(standardDeviation); 

        NormalDistribution distribution = new NormalDistribution(average, standardDeviation);

        return distribution.InverseLeftProbability(Random.Range(minRandom, maxRandom));
    }


    //Generates a list of normally-distributed random values with a given standard deviation and average
    public static List<double> Generate(double average, double standardDeviation, int howMany)
    {
        //Ensures SD is greater than 0
        standardDeviation = StandardDeviationCheck(standardDeviation); 

        NormalDistribution distribution = new NormalDistribution(average, standardDeviation);
        List<double> results = new List<double>();

        //Set howMany to minimum of 1
        if (howMany <= 0)
        {
            howMany = 1;
        }

        for(int i = 0; i < howMany; i++)
        {
            results.Add(distribution.InverseLeftProbability(Random.Range(minRandom, maxRandom)));
        }
        return results;
    }

    //Generates a single normally-distributed random value with a given standard deviation and average, between two given boundaries
    public static double Generate(double average, double standardDeviation, double minimumBound, double maximumBound)
    {
        //Ensures SD is greater than 0
        standardDeviation = StandardDeviationCheck(standardDeviation);

        boundaries workingBoundaries = new boundaries();
        workingBoundaries.minimumBound = minimumBound;
        workingBoundaries.maximumBound = maximumBound;

        //Ensure boundaries are not equal or reversed
        workingBoundaries = BoundaryCheck(workingBoundaries, average, standardDeviation);

        NormalDistribution distribution = new NormalDistribution(average, standardDeviation);

        return distribution.InverseLeftProbability(Random.Range((float)workingBoundaries.minimumBound, (float)workingBoundaries.maximumBound));
    }


    //Generates a list of normally-distributed random values with a given standard deviation and average, between two given boundaries
    public static List<double> Generate(double average, double standardDeviation, double minimumBound, double maximumBound, int howMany)
    {
        //Ensures SD is greater than 0
        standardDeviation = StandardDeviationCheck(standardDeviation);

        boundaries workingBoundaries = new boundaries();
        workingBoundaries.minimumBound = minimumBound;
        workingBoundaries.maximumBound = maximumBound;

        //Ensure boundaries are not equal or reversed
        workingBoundaries = BoundaryCheck(workingBoundaries, average, standardDeviation);

        NormalDistribution distribution = new NormalDistribution(average, standardDeviation);
        List<double> results = new List<double>();

        //Set howMany to minimum of 1
        if (howMany <= 0)
        {
            howMany = 1;
        }

        for (int i = 0; i < howMany; i++)
        {
            results.Add(distribution.InverseLeftProbability(Random.Range((float)workingBoundaries.minimumBound, (float)workingBoundaries.maximumBound)));
        }

        return results;
    }


    //Checks if the provided standard deviation is a valid number. If not, changes it to 1 and generates an error message
    private static double StandardDeviationCheck(double standardDeviation)
    {
        if(standardDeviation <= 0)
        {
            standardDeviation = 1.0f;
            Debug.LogError("Standard Deviation is Less than Equal to Zero. Changed to 1.");
        }
        
        return standardDeviation;
    }


    //Checks if the upper and lower bounds are valid and converts to probabilities
    private static boundaries BoundaryCheck(boundaries workingBoundaries, double average, double standardDeviation)
    {

        //Correct reveresed min and max boundaries
        if (workingBoundaries.minimumBound > workingBoundaries.maximumBound)
        {
            double hold = workingBoundaries.minimumBound;
            workingBoundaries.minimumBound = workingBoundaries.maximumBound;
            workingBoundaries.maximumBound = hold;
            Debug.LogError("Min boundary is greater than max boundary. Reversing boundary order.");
        }

        //Restrict the minimum bound to pre-determined number of SDs from the mean
        if(workingBoundaries.minimumBound < average - standardDeviation * maxSds)
        {
            workingBoundaries.minimumBound = average - standardDeviation * maxSds;
            Debug.LogError("Minimum bound exceeds allowed distance from the mean. Auto-correcting to " + maxSds + " SDs from the mean.");
        }

        //Restrict the maximum bound to pre-determined number of SDs from the mean
        if (workingBoundaries.maximumBound > average + standardDeviation * maxSds)
        {
            workingBoundaries.maximumBound = average + standardDeviation * maxSds;
            Debug.LogError("Maximum bound exceeds allowed distance from the mean. Auto-correcting to " + maxSds + " SDs from the mean.");
        }

        //Convert boundaries into a probability for rng
        NormalDistribution distribution = new NormalDistribution(average, standardDeviation);
        workingBoundaries.minimumBound = distribution.LeftProbability(workingBoundaries.minimumBound);
        workingBoundaries.maximumBound = distribution.LeftProbability(workingBoundaries.maximumBound);

        //Capture and correct 0% probability
        if (workingBoundaries.minimumBound <= 0)
        {
            workingBoundaries.minimumBound = minDifference;
            Debug.LogError("Correcting 0% probability event in minimum bound.");
        }

        ////Capture and correct 0% probability
        if (workingBoundaries.maximumBound <= 0)
        {
            workingBoundaries.maximumBound = minDifference;
            Debug.LogError("Correcting 0% probability event in maximum bound.");
        }

        //Capture and correct 100% probability
        if (workingBoundaries.maximumBound >= 1)
        {
            workingBoundaries.maximumBound = 1 - minDifference;
            Debug.LogError("Correcting 100% probability event in maximum bound.");
        }

        ////Capture and correct 100% probability
        if (workingBoundaries.minimumBound >= 1)
        {
            workingBoundaries.minimumBound = 1 - minDifference;
            Debug.LogError("Correcting 100% probability event in minimum bound.");
        }

        //Boundaries cannot equal each other. Auto-attempt to create nominal distance between min and max bounds.
        if (workingBoundaries.maximumBound - workingBoundaries.minimumBound < minDifference)
        {
            Debug.LogError("Distance between min and max boundaries too small. Attempting to correct.");

            if (workingBoundaries.minimumBound <= minDifference)
            {
                workingBoundaries.maximumBound += minDifference;
            }
            else
            {
                workingBoundaries.minimumBound -= minDifference;
            }
        }

        return workingBoundaries;
    }

}
