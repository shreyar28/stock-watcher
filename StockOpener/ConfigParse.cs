using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace StockOpener
{
    /// <summary>
    /// This class handles the config file and places it into a public static dictionary for use throughout the program.
    /// </summary>
    class ConfigParse
    {


            /// <summary>
            /// A dictionary containing the config keys and values. External programs can get data, but can't modify/add data.
            /// </summary>
            public static Dictionary<string, string> ConfigKeys
            {
                get;
                private set;
            } = new Dictionary<string, string>();

            /// <summary>
            /// This will look into the config file and update the public dictionary with new values.
            /// </summary>
            public static void ParseConfig(string configName)
            {
                ConfigKeys.Clear();
                ReadXmlConfig(configName);
                CheckForMissingInput();
            }

            /// <summary>
            /// This will read the XML file and place it into a dictionary.
            /// </summary>
            private static void ReadXmlConfig(string configName)
            {
                try
                {
                    XmlDocument doc = new XmlDocument();
                    doc.XmlResolver = null;
                    doc.Load(Path.Combine(".", configName));

                    XmlNode root = doc.DocumentElement;
                    XmlNodeList nodeList = root.SelectNodes("/configuration/appSettings/add");

                    foreach (XmlNode node in nodeList)
                    {
                        ConfigKeys.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }


                }
                catch (Exception e)
                {
                Console.WriteLine("Exception occured. Weird.");
                Console.WriteLine("Exception: " + e);
                Console.WriteLine("Please report to your local friendly software developer.");
                Console.ReadKey();
                Environment.Exit(1);
            }
            }

            /// <summary>
            /// This will check the dictionary for missing content.
            /// </summary>
            private static void CheckForMissingInput()
            {
                try
                {
                    foreach (var option in ConfigKeys)
                    {
                        //If config entry is blank OR starts with '!#' AND ends with '#!'
                        if (option.Value == "")
                        {
                            throw new ArgumentException("There is some missing data (" + option + ") that needs to be entered into the config file. Exiting application.");
                        }
                        else if (option.Value.Length >= 4 && option.Value.Substring(0, 2) == "!#" && option.Value.Substring(option.Value.Length - 2, 2) == "#!")
                        {
                            throw new ArgumentException("There is some missing data (" + option + ") that needs to be entered into the config file. Exiting application.");
                        }
                    }
                }
                catch (Exception e)
                {
                Console.WriteLine("Exception occured. Weird.");
                Console.WriteLine("Exception: " + e);
                Console.WriteLine("Please report to your local friendly software developer.");
                Console.ReadKey();
                Environment.Exit(1);
                }
            }
    }
}

