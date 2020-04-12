using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DwarfEngine
{
    public class CSVParser
    {
        //private const char lineSeperator = '\n';
        private const char fieldSeperator = ',';

        public string[] ParseLines(string file)
        {
            //return file.Split(lineSeperator);
            return file.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public string[] ParseFields(string line)
        {
            //line = Regex.Replace(line, lineFix, string.Empty);
            return line.Split(fieldSeperator);
        }

        public string[] ParseCustom(string field, char seperator)
        {
            return field.Split(seperator);
        }
    }
}
