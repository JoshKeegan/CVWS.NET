/*
 * Computer Vision Wordsearch Solver
 * Quantitative Evaluation
 * Confusion Matrix - a summary of the performance of a classification algorithm
 * By Josh Keegan 08/03/2014
 * Last Edit 11/06/2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantitativeEvaluation
{
    public class ConfusionMatrix
    {
        //Private variables
        private string[] classLabels;
        private uint[,] matrix; //[Actual, Predicted]

        //Public variables
        public uint TotalClassifications
        {
            get
            {
                uint totalClassifications = 0;
                for(int i = 0; i < matrix.GetLength(0); i++)
                {
                    for(int j = 0; j < matrix.GetLength(1); j++)
                    {
                        totalClassifications += matrix[i, j];
                    }
                }
                return totalClassifications;
            }
        }

        public uint NumMisclassifications
        {
            get
            {
                uint numMisclassifications = 0;
                for(int i = 0; i < matrix.GetLength(0); i++)
                {
                    for(int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if(i != j)
                        {
                            numMisclassifications += matrix[i, j];
                        }
                    }
                }
                return numMisclassifications;
            }
        }

        //Constructor
        public ConfusionMatrix(string[] labels)
        {
            this.classLabels = labels;
            
            //Initialise the internal representation of the confusion matrix
            this.matrix = new uint[labels.Length, labels.Length];

            for(int i = 0; i < this.matrix.GetLength(0); i++)
            {
                for (int j = 0; j < this.matrix.GetLength(1); j++)
                {
                    this.matrix[i, j] = 0;
                }
            }
        }

        //Public methods
        public void Add(string actual, string predicted)
        {
            int actualIdx = Array.FindIndex(classLabels, label => label == actual);
            int predictedIdx = Array.FindIndex(classLabels, label => label == predicted);

            Add(actualIdx, predictedIdx);
        }

        public void Add(int actualLabelIdx, int predictedLabelIdx)
        {
            matrix[actualLabelIdx, predictedLabelIdx]++;
        }

        //Statistical Calculation methods relative to a single class
        
        //TODO: Implement things like true positive, true negative etc... See:
        //http://en.wikipedia.org/wiki/Confusion_matrix
        //http://en.wikipedia.org/wiki/Precision_and_recall#Definition_.28classification_context.29

        public void WriteToCsv(string path)
        {
            StreamWriter writer = new StreamWriter(path);

            //Headers
            writer.Write("label,");
            for(int i = 0; i < classLabels.Length; i++)
            {
                writer.Write(classLabels[i]);
                
                //If this isn't the last thing on the line, write a comma
                if(i != classLabels.Length - 1)
                {
                    writer.Write(",");
                }
            }
            writer.Write("\n");

            //Matrix contents
            for (int i = 0; i < classLabels.Length; i++)
            {
                //Write out the label for this class
                writer.Write(classLabels[i] + ",");

                //Now write out this row from the matrix
                for(int j = 0; j < matrix.GetLength(1); j++)
                {
                    writer.Write(matrix[j, i]); //It is the convention to display CM's with Predicted class as columns and Actual class as rows

                    //If this isn't the last thing on the line, write a comma
                    if (j != matrix.GetLength(1) - 1)
                    {
                        writer.Write(",");
                    }
                }
                writer.Write("\n");
            }

            //Clean up
            writer.Close();
        }
    }
}
