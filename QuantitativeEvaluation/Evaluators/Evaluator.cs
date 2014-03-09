/*
 * Dissertation CV Wordsearch Solver
 * Quantitative Evaluation
 * Evaluator - abstract class
 * By Josh Keegan 08/03/2014
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuantitativeEvaluation.Evaluators
{
    public abstract class Evaluator<E, D, L> //E => System to Evaluate, D => Data to evaluate, L => Label for data
    {
        //Private Variables
        protected E toEvaluate;
        //TODO: Add variables here for storing data that can be used to compute things such as precision, recall, F-measure etc . . .

        public Evaluator(E toEvaluate)
        {
            this.toEvaluate = toEvaluate;
        }

        public abstract void Evaluate(D[] evaluationData, L[] evaluationLabels);
    }
}
