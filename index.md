---
layout: default
title: CVWS.NET
---

**CVWS.NET** is an open source Computer Vision system capable of finding and solving word searches in images from smartphones. 

The overall project is comprised of:

 * **libCVWS** - the library providing the core functionality and is intended for use by developers of other applications
 * **DemoGUI** - a desktop application designed to showcase the project. Allows you to select the processing method for each stage from detecing the word search right through to solving it
 * **QuantitativeEvaluation** - crunches the numbers to determine just how good the overall system is, as well as comparing the performance of different implementations of each stage
 * **ImageMarkup** - database-less storage of data about images containing word searches. Used for training, cross-validation and evaluation of the feature extractors and classifiers that lie at the heart of this project
 * **DataEntryGUI** - a desktop application for entering data about images containing word searches
 * **UnitTests** - self-explanatory. Ensures that the more complicated low-level methods of the project (that could produce deep bugs) are doing what they should be. Tests are re-run before builds to keep things that way!

## libCVWS ##
At the core of the project is **libCVWS**, a library containing all of the methods necessary to go from an image containing a wordsearch along with a list of word, to a solution. This means that everything needed to build your own word search solving application can be found in this library.
**libCVWS** exposes as much of what's going on "under the hood" as possible to give developers choice about how each processing stage is handled (e.g. would you rather specify a location for a word search rather than finding it, which method should be used to segment a word search image up into its rows & columns). By exposing this functionality, **libCVWS** is also of use in other applications that have nothing to do with word searches (e.g. use libCVWS.Imaging to efficiently combine images into a single larger one, or easily set up a Neural Network with common feature extraction techniques by using libCVWS.ClassifierInterfacing).