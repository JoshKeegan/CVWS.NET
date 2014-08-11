/*
 * CVWS.NET: Computer Vision Wordsearch Solver .NET
 * libCVWS
 * Trainable Feature Extraction Algorithm - abstract class
 * By Josh Keegan 11/03/2014
 * Last Edit 17/05/2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

using libCVWS.Exceptions;


namespace libCVWS.ClassifierInterfacing.FeatureExtraction
{
    [Serializable]
    public abstract class TrainableFeatureExtractionAlgorithm : FeatureExtractionAlgorithm, ISerializable
    {
        private bool trained = false;

        //Constructor (used during deserialization)
        protected TrainableFeatureExtractionAlgorithm(SerializationInfo info, StreamingContext context)
        {
            trained = (bool)info.GetValue("trained", typeof(bool));
        }

        protected TrainableFeatureExtractionAlgorithm() { }

        //Use the public methods here to perform checks about the training before then passing the call on to the child class in the protected DoBlah methods
        public void Train(Bitmap[] charImgs)
        {
            if(trained)
            {
                throw new TrainableFeatureExtractionAlgorithmException("Trainable feature extraction algorithm has already been trained");
            }
            trained = true;

            DoTrain(charImgs);
        }

        protected abstract void DoTrain(Bitmap[] charImgs);

        public static TrainableFeatureExtractionAlgorithm Load(string filePath)
        {
            TrainableFeatureExtractionAlgorithm trained = null;

            Stream stream = File.Open(filePath, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();

            trained = (TrainableFeatureExtractionAlgorithm)formatter.Deserialize(stream);
            
            //Clean up
            stream.Close();

            return trained;
        }

        public void Save(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Create);

            this.Save(stream);

            //Clean up
            stream.Close();
        }

        public void Save(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, this);
        }

        public override double[][] Extract(Bitmap[] charImgs)
        {
            if(!trained)
            {
                throw new TrainableFeatureExtractionAlgorithmException("Trainable feature extraction algorithms must be trained before extracting data");
            }

            return DoExtract(charImgs);
        }

        protected abstract double[][] DoExtract(Bitmap[] charImgs);

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("trained", trained);
        }
    }
}
