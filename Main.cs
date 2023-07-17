using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

// To execute C#, please define "static void Main" on a class
// named Solution.
class Solution
{
    public static void Main(string[] args)
    {
        string[] files = { "/home/coderpad/data/example1.txt", "/home/coderpad/data/example2.txt" };

        if (files.Length == 0)
        {
            Console.WriteLine("Please provide the path to the input file as an argument.");
            return;
        }

        foreach (var file in files)
        {
            try
            {
                var anagramGroups = AnagramAlgorithm.GroupAnagrams(file);
                PrintAnagramGroups(anagramGroups, 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }          
        }
    }

    public static void PrintAnagramGroups(Dictionary<string, List<string>> anagramGroups, int maxWordsPerGroup)
    {
        foreach (var group in anagramGroups)
        {
            if (group.Value.Count <= maxWordsPerGroup)
            {
                Console.WriteLine(string.Join(",", group.Value));
            }
            else
            {
                Console.WriteLine(string.Join(",", group.Value.Take(maxWordsPerGroup)) + ", ...");
            }
        }
    }

}

class AnagramAlgorithm {

    public static Dictionary<string, List<string>> GroupAnagrams(string filePath)
    {
        var anagramGroups = new Dictionary<string, List<string>>();
    
        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (var streamReader = new StreamReader(fileStream))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                string word = line.Trim();
                string sortedWord = SortString(word);

                if (anagramGroups.TryGetValue(sortedWord, out var group))
                {
                    group.Add(word);
                }
                else
                {
                    group = new List<string> { word };
                    anagramGroups.Add(sortedWord, group);
                }
            }
         }

          return anagramGroups;
    }

    // The sorting algorithm used Array.Sort, which has a time complexity of O(n log n) in the average case
    public static string SortString(string input)
    {
        char[] characters = input.ToCharArray();
        Array.Sort(characters);
        return new string(characters);
    }
}


[TestFixture]
public class AnagramTests
{
    [Test]
    public void GroupAnagrams_EmptyFile_ReturnsEmptyResult()
    {
        // Arrange
        string filePath = "empty.txt";

        // Act
        var anagramGroups = AnagramAlgorithm.GroupAnagrams(filePath);

        // Assert
        Assert.IsEmpty(anagramGroups);
    }

    [Test]
    public void GroupAnagrams_SingleWordPerLine_ReturnsIndividualGroups()
    {
        // Arrange
        string filePath = "single_word.txt";

        // Act
        var anagramGroups = AnagramAlgorithm.GroupAnagrams(filePath);

        // Assert
        Assert.AreEqual(3, anagramGroups.Count);

        Assert.IsTrue(anagramGroups.ContainsKey("abc"));
        Assert.AreEqual(1, anagramGroups["abc"].Count);
        Assert.Contains("abc", anagramGroups["abc"]);

        Assert.IsTrue(anagramGroups.ContainsKey("def"));
        Assert.AreEqual(1, anagramGroups["def"].Count);
        Assert.Contains("def", anagramGroups["def"]);

        Assert.IsTrue(anagramGroups.ContainsKey("xyz"));
        Assert.AreEqual(1, anagramGroups["xyz"].Count);
        Assert.Contains("xyz", anagramGroups["xyz"]);
    }

    [Test]
    public void GroupAnagrams_MultipleAnagrams_ReturnsGroupedAnagrams()
    {
        // Arrange
        string filePath = "multiple_anagrams.txt";

        // Act
        var anagramGroups = AnagramAlgorithm.GroupAnagrams(filePath);

        // Assert
        Assert.AreEqual(3, anagramGroups.Count);

        Assert.IsTrue(anagramGroups.ContainsKey("abc"));
        Assert.AreEqual(2, anagramGroups["abc"].Count);
        Assert.Contains("abc", anagramGroups["abc"]);
        Assert.Contains("cab", anagramGroups["abc"]);

        Assert.IsTrue(anagramGroups.ContainsKey("def"));
        Assert.AreEqual(2, anagramGroups["def"].Count);
        Assert.Contains("def", anagramGroups["def"]);
        Assert.Contains("fed", anagramGroups["def"]);

        Assert.IsTrue(anagramGroups.ContainsKey("xyz"));
        Assert.AreEqual(3, anagramGroups["xyz"].Count);
        Assert.Contains("xyz", anagramGroups["xyz"]);
        Assert.Contains("yzx", anagramGroups["xyz"]);
        Assert.Contains("zxy", anagramGroups["xyz"]);
    }
}