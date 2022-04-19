using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVParser
{
    private readonly string[] lineSeparator = { "\r\n" };

    private readonly char surround = '"';

    public void ProcessTextData(string csvData, Action<Dictionary<string, string>> onProcessingComplete)
    {
        var textData = new Dictionary<string, string>();
        var lines = csvData.Split(lineSeparator, StringSplitOptions.None);

        var csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            var fields = csvParser.Split(line);

            for (var f = 0; f < fields.Length; f++)
            {
                fields[f] = fields[f].TrimStart(' ', surround);
                fields[f] = fields[f].TrimEnd(surround);
                fields[f] = fields[f].Replace("\"", "");
            }

            if (string.IsNullOrEmpty(fields[1]) || textData.ContainsKey(fields[1])) continue;
            textData.Add(fields[1], fields[2]);
        }

        onProcessingComplete(textData);
    }

    public void ProcessScenarioData(string csvData, Action<Dictionary<string, ScenarioTextData>> onProcessingComplete)
    {
        var textData = new Dictionary<string, ScenarioTextData>();
        var lines = csvData.Split(lineSeparator, StringSplitOptions.None);

        var csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            var fields = csvParser.Split(line);

            for (var f = 0; f < fields.Length; f++)
            {
                fields[f] = fields[f].TrimStart(' ', surround);
                fields[f] = fields[f].TrimEnd(surround);
                fields[f] = fields[f].Replace("\"", "");
            }

            if (string.IsNullOrEmpty(fields[1]) || textData.ContainsKey(fields[1])) continue;

            var scenarioTextData = new ScenarioTextData()
            {
                id = fields[1],
                beforeText = fields[2],
                optionA = fields[3],
                optionB = fields[4],
                optionC = fields[5]
            };
            textData.Add(fields[1], scenarioTextData);
        }

        onProcessingComplete(textData);
    }
    
    public Dictionary<string, Dictionary<string, string>> GetTextDataLanguageDictionary(string csvData)
    {
        var textDataDictionary = new Dictionary<string, Dictionary<string, string>>()
        {
            { "en-EN", new Dictionary<string, string>() },
            { "nl-NL", new Dictionary<string, string>() }
        };
        
        var lines = csvData.Split(lineSeparator, StringSplitOptions.None);

        var csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];
            
            var fields = csvParser.Split(line);

            for (var f = 0; f < fields.Length; f++)
            {
                fields[f] = fields[f].TrimStart(' ', surround);
                fields[f] = fields[f].TrimEnd(surround);
                fields[f] = fields[f].Replace("\"", "");
            }

            if (string.IsNullOrEmpty(fields[1]) || textDataDictionary["en-EN"].ContainsKey(fields[1])) continue;
            textDataDictionary["en-EN"].Add(fields[1], fields[2]);
            textDataDictionary["nl-NL"].Add(fields[1], fields[3]);
        }
        
        return textDataDictionary;
    }
        
    public Dictionary<string, Dictionary<string, ScenarioTextData>> GetScenarioLanguageDictionary(string csvData)
    {
        var textDataDictionary = new Dictionary<string, Dictionary<string, ScenarioTextData>>()
        {
            { "en-EN", new Dictionary<string, ScenarioTextData>() },
            { "nl-NL", new Dictionary<string, ScenarioTextData>() }
        };
        
        var lines = csvData.Split(lineSeparator, StringSplitOptions.None);

        var csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            var fields = csvParser.Split(line);

            for (var f = 0; f < fields.Length; f++)
            {
                fields[f] = fields[f].TrimStart(' ', surround);
                fields[f] = fields[f].TrimEnd(surround);
                fields[f] = fields[f].Replace("\"", "");
            }

            if (string.IsNullOrEmpty(fields[1]) || textDataDictionary["en-EN"].ContainsKey(fields[1])) continue;

            var englishData = new ScenarioTextData()
            {
                id = fields[1],
                beforeText = fields[2],
                optionA = fields[3],
                optionB = fields[4],
                optionC = fields[5]
            };
            textDataDictionary["en-EN"].Add(fields[1], englishData);
            
            if (textDataDictionary["nl-NL"].ContainsKey(fields[1])) continue;
            
            var dutchData = new ScenarioTextData()
            {
                id = fields[1],
                beforeText = fields[6],
                optionA = fields[7],
                optionB = fields[8],
                optionC = fields[9]
            };
            textDataDictionary["nl-NL"].Add(fields[1], dutchData);
        }

        return textDataDictionary;
    }
}