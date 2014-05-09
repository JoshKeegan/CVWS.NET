/*
 * Dissertation CV Wordsearch Solver
 * Demo GUI
 * Configuration
 * By Josh Keegan 09/05/2014
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DemoGUI
{
    static class Configuration
    {
        //Constants
        private static string CONFIG_DIR_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ComputerVisionWordsearchSolver");
        private static string CONFIG_FILE_PATH = Path.Combine(CONFIG_DIR_PATH, "demoGUI.cfg.xml");
        private static Encoding FILE_ENCODING = Encoding.UTF8;

        private const string XML_ROOT_EL = "Config";

        //Variables
        private static bool loaded = false;
        private static Dictionary<string, string> config = new Dictionary<string, string>();

        public static void SetConfigurationOption(string option, string value)
        {
            if(config.ContainsKey(option))
            {
                config[option] = value;
            }
            else
            {
                config.Add(option, value);
            }
        }

        public static string GetConfigurationOption(string option)
        {
            try
            {
                return config[option];
            }
            catch //Option not in config
            {
                return null;
            }
        }

        public static bool Load()
        {
            bool success = false;

            //Prevent from loading multiple times
            if(!loaded)
            {
                //Check there is a config file
                if(File.Exists(CONFIG_FILE_PATH))
                {
                    //Reset the current config to the default
                    config = new Dictionary<string, string>();

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(CONFIG_FILE_PATH);

                    XmlNode rootNode = xmlDoc.SelectSingleNode(XML_ROOT_EL);

                    XmlNodeList configOptionsNodes = rootNode.ChildNodes;

                    //Add all of the options to the config dictionary
                    foreach(XmlNode optionNode in configOptionsNodes)
                    {
                        string key = optionNode.Name;
                        string value = optionNode.InnerText;

                        //Treat any values that are the empty string as being null
                        if(value == "")
                        {
                            value = null;
                        }

                        config.Add(key, value);
                    }

                    //Loaded successfully
                    success = true;
                }

                //Set loaded to true so we won't load the config file again
                loaded = true;
            }

            return success;
        }

        public static void Save()
        {
            //Create the directory if it doesn't already exist
            if(!Directory.Exists(CONFIG_DIR_PATH))
            {
                Directory.CreateDirectory(CONFIG_DIR_PATH);
            }

            //Create/Replace the file
            FileStream fileStream = new FileStream(CONFIG_FILE_PATH, FileMode.Create); //Overwrite the new file if it already exists

            //Use XML Writer Object to write out the XML
            XmlTextWriter xmlOut = new XmlTextWriter(fileStream, FILE_ENCODING);

            //Auto-Indent
            xmlOut.Formatting = Formatting.Indented;

            //Write out the elements to have
            xmlOut.WriteStartDocument();

            //Root Node
            xmlOut.WriteStartElement(XML_ROOT_EL);

            //Save each individual option out
            foreach(KeyValuePair<string, string> kvp in config)
            {
                string key = kvp.Key;
                string value = kvp.Value;

                xmlOut.WriteStartElement(key);
                xmlOut.WriteString(value);
                xmlOut.WriteEndElement();
            }

            xmlOut.WriteEndElement(); //Root El

            //Clean up
            xmlOut.Close();
            fileStream.Close();
        }
    }
}
