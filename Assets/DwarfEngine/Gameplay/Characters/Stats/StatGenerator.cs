using System;
using System.Collections.Generic;
using UnityEngine;

namespace DwarfEngine
{
    public class StatGenerator : MonoBehaviour
    {
        private const string statName = "stat_name";
        private const string statType = "stat_type";
        private const string statMinMax = "stat_minmax";

        private const char minMaxSeperator = '-';
        private const char listSeperator = ';';

        //public string collectionName;

        public TextAsset collectionsFile;
        public TextAsset groupsFile;
        public TextAsset statsFile;

        public Dictionary<string, string[]> collectionDictionary;
        public Dictionary<string, string[]> groupDictionary;

        public List<StatTemplate> statTemplates;
        public List<DynamicStat> randomStats;

        private CSVParser csvParser;

        private void Start()
        {
            ParseStats();
            randomStats = GetRandomStats("coll_character");
        }

        public void ParseStats()
        {
            csvParser = new CSVParser();
            statTemplates = new List<StatTemplate>();

            #region Parse Collections
            string[] collectionLines = csvParser.ParseLines(collectionsFile.text);
            //string[] collectionColumns = csvParser.ParseFields(collectionLines[0]);

            collectionDictionary = new Dictionary<string, string[]>();
            for (int i = 1; i < collectionLines.Length; i++)
            {
                string[] fields = csvParser.ParseFields(collectionLines[i]);
                string[] groups = csvParser.ParseCustom(fields[1], listSeperator);
                collectionDictionary.Add(fields[0], groups);
            }
            #endregion

            #region Parse Groups
            string[] groupLines = csvParser.ParseLines(groupsFile.text);
            //string[] groupColumns = csvParser.ParseFields(groupLines[0]);

            groupDictionary = new Dictionary<string, string[]>();
            for (int i = 1; i < groupLines.Length; i++)
            {
                string[] fields = csvParser.ParseFields(groupLines[i]);
                string[] groups = csvParser.ParseCustom(fields[1], listSeperator);
                groupDictionary.Add(fields[0], groups);
            }
            #endregion

            #region Parse Stats
            string[] statLines = csvParser.ParseLines(statsFile.text);
            string[] statColumns = csvParser.ParseFields(statLines[0]);

            for (int i = 1; i < statLines.Length; i++) // Start with the second line, first line defines the column names
            {
                string[] fields = csvParser.ParseFields(statLines[i]); // Fields of the line

                StatTemplate statModel = new StatTemplate();

                for (int j = 0; j < fields.Length; j++)
                {
                    if (statColumns[j] == statName)
                    {
                        statModel.name = fields[j];
                    }
                    else if (statColumns[j] == statType)
                    {
                        statModel.type = fields[j] == "int" ? typeof(int) : typeof(float);
                    }
                    else if (statColumns[j] == statMinMax)
                    {
                        string[] minMax = fields[j].Split(minMaxSeperator);

                        statModel.minValue = float.Parse(minMax[0]);
                        statModel.maxValue = float.Parse(minMax[1]);
                    }
                }

                statTemplates.Add(statModel);
            } 
            #endregion
        }

        public List<DynamicStat> GetRandomStats(string collectionName)
        {
            if (string.IsNullOrEmpty(collectionName))
            {
                return null;
            }

            List<DynamicStat> generatedStats = new List<DynamicStat>();

            foreach (string groupName in collectionDictionary[collectionName]) // For each group in the collection
            {
                int randomStat = UnityEngine.Random.Range(0, groupDictionary[groupName].Length); // Get a random index
                string randomStatName = groupDictionary[groupName][randomStat]; // Select the name of the random stat from the group
                StatTemplate statTemp = statTemplates.Find(s => s.name == randomStatName); // Get the randomly selected stat from its name

                DynamicStat stat = new DynamicStat();

                stat.name = statTemp.name; // Set the stat name
                float randomValue = UnityEngine.Random.Range(statTemp.minValue, statTemp.maxValue); // Get its final value
                stat.value = statTemp.type == typeof(int) ? Mathf.FloorToInt(randomValue) : (float)(Math.Truncate((double)randomValue * 10.0) / 10.0); // Round to int or float
                generatedStats.Add(stat); // Finally, add the generated stat to the list
            }

            return generatedStats;
        }

        // TODO : Save the stats to a new CSV file
    }
}
