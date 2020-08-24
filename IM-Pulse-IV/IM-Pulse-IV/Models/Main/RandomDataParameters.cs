using System;
using System.Collections.Generic;
using System.Text;

namespace IM_Pulse_IV.Models.Main
{
    public class RandomDataParameters
    {
        public const int MinSegmentLength = 100;
        public const int MaxSegmentLength = 5000;
        public const int MinNumberOfSegments = 1;
        public const int MaxNumberOfSegments = 100;
        public const int MinCommandsPerSegment = 5;
        public const int MaxCommandsPerSegment = 50;
        public const int MinTextGenerationFrequency = 1000;
        public const int MaxTextGenerationFrequency = 10000;

        public RandomDataParameters() { }

        public RandomDataParameters(int segmentLength, int numberOfSegments, string delimiter, 
            int minCommandsPerSegment, int maxCommandsPerSegment, bool isPulse, 
            int randomTextGenerationFrequency)
        {
            SegmentLength = segmentLength;
            NumberOfSegments = numberOfSegments;
            DeLimiter = delimiter;
            MinNumberOfCommandsPerSegment = minCommandsPerSegment;
            MaxNumberOfCommandsPerSegment = maxCommandsPerSegment;
            IsPulseVsContinuous = isPulse;
            RandomTextGenerationFrequency = randomTextGenerationFrequency;
        }

        public int SegmentLength { get; set; }
        public int NumberOfSegments { get; set; }
        public string DeLimiter { get; set; }
        public int MinNumberOfCommandsPerSegment { get; set; }
        public int MaxNumberOfCommandsPerSegment { get; set; }
        public bool IsPulseVsContinuous { get; set; }
        public int RandomTextGenerationFrequency { get; set; }
        public string ValidationMessage { get; set; }

        public bool EntriesAreValid()
        {
            string standardMinMaxMessage = " value must be between Min & Max";
            
            if(SegmentLength < MinSegmentLength 
                || SegmentLength > MaxSegmentLength) 
            {
                ValidationMessage = ("Segment Length" + standardMinMaxMessage);    
                return false; 
            }

            else if(NumberOfSegments < MinNumberOfSegments 
                || NumberOfSegments > MaxNumberOfSegments) 
            {
                ValidationMessage = ("Number of segments" + standardMinMaxMessage);
                return false; 
            }

            else if(DeLimiter.Length > 1 
                || DeLimiter.Length == 0) 
            {
                ValidationMessage = ("Delimiter must be a single string character");
                return false; 
            }

            else if(MinNumberOfCommandsPerSegment < MinCommandsPerSegment 
                || MinNumberOfCommandsPerSegment > MaxCommandsPerSegment 
                || MinNumberOfCommandsPerSegment > MaxNumberOfCommandsPerSegment) 
            {
                ValidationMessage = ("Command Segments" + standardMinMaxMessage);
                return false; 
            }

            else if(MaxNumberOfCommandsPerSegment < MinCommandsPerSegment 
                || MaxNumberOfCommandsPerSegment > MaxCommandsPerSegment)
            {
                ValidationMessage = ("Number of Commands" + standardMinMaxMessage);
                return false; 
            }

            else if(RandomTextGenerationFrequency < MinTextGenerationFrequency 
                || RandomTextGenerationFrequency > MaxTextGenerationFrequency)
            {
                ValidationMessage = ("Random Text Generation Frequency" + standardMinMaxMessage);
                return false; 
            }

            else 
            {
                ValidationMessage = ("All Entries are Valid!");
                return true; 
            }
        }
    }
}
