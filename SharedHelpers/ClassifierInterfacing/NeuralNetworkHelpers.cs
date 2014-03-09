/*
 * Dissertation CV Wordsearch Solver
 * Shared helpers
 * Neural Network Helpers - functions to help interface with the AForge.NET Neural Networks
 * By Josh Keegan 08/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedHelpers.Exceptions;

namespace SharedHelpers.ClassifierInterfacing
{
    public static class NeuralNetworkHelpers
    {
        public static double[][] GetDesiredOutputForChars(char[] chars)
        {
            double[][] toRet = new double[chars.Length][];

            for (int i = 0; i < chars.Length; i++)
            {
                toRet[i] = GetDesiredOutputForChar(chars[i]);
            }

            return toRet;
        }

        public static double[] GetDesiredOutputForChar(char c)
        {
            //Check that the char is within the expected bounds
            if(c < 'A' || c > 'Z')
            {
                throw new UnexpectedClassifierOutputException("Classifier output character must be within range A-Z. Received " + c);
            }

            int cIdx = c - 'A';
            double[] toRet = new double[ClassifierHelpers.NUM_CHAR_CLASSES];

            for (int i = 0; i < toRet.Length; i++)
            {
                toRet[i] = i == cIdx ? 0.5f : -0.5f;
            }
            return toRet;
        }

        public static char GetMostLikelyChar(double[] networkOutput)
        {
            //Check the number of classes in the network output received is valid
            if(networkOutput.Length != ClassifierHelpers.NUM_CHAR_CLASSES)
            {
                throw new UnexpectedClassifierOutputException("Received a classifier output with " + 
                    networkOutput.Length + " classes. Expected " + ClassifierHelpers.NUM_CHAR_CLASSES + " classes");
            }

            int maxIdx = 0;
            for(int i = 1; i < networkOutput.Length; i++)
            {
                if(networkOutput[i] > networkOutput[maxIdx])
                {
                    maxIdx = i;
                }
            }

            return (char)('A' + maxIdx);
        }
    }
}
