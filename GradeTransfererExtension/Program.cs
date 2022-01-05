using System;
using System.IO;
using System.Collections.Generic;
using ExtensionsMethods;
using System.Diagnostics;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
public class Program : MonoBehaviour
{
    public InputField desmosPointsInput;
    
    public void SelectProgram(int programCodeResult)
    {
        if (programCodeResult == 1)
        {
            //desmos
            int points;
            if (int.TryParse(desmosPointsInput.text, out points))
            {
                Debug.Log("Sort and format complete");
                string clip = GUIUtility.systemCopyBuffer;
                string[] clips = clip.Split("\n\r".ToCharArray());
                List<string> myList = new List<string>();
                for (int i = 0; i < clips.Length; i++)
                {
                    string[] clipses = clips[i].Split(' ');

                    if (clipses.Length > 1)
                    {
                        if (clipses[1].Contains(","))
                            clipses[1] = new string((char[]) clipses[1].ToCharArray().RemoveElement(','));
                        if (clipses[0].Contains(","))
                            clipses[0] = new string((char[]) clipses[0].ToCharArray().RemoveElement(','));

                        string output = clipses[1] + ", " + clipses[0] + "\t" + points;
                        myList.Add(output);
                    }
                    else if (clips[i].ToCharArray().Length > 1)
                    {
                        myList.Add("_" + clips[i]);
                        Debug.Log(clips[i] + " did not input a full name. Manually enter grade.");
                    }
                }
                
                myList.Sort();
                foreach (string s in myList)
                {
                    GUIUtility.systemCopyBuffer +="\r\n" + s;
                }
                
                // rick rollem
                //  Clipboard.SetText("https://www.youtube.com/watch?v=oHg5SJYRHA0");
                Debug.Log("Copy a new list to sort again.");
            }
            else Debug.Log("Invalid entry. Please input a number, such as 10");
        }
        else if (programCodeResult == 2)
        {
            //big ideas
            //process the clipboard
            Debug.Log("Sort and format complete");
            string clip = GUIUtility.systemCopyBuffer;
            string[] clips = clip.Split("\n\r".ToCharArray());
            List<string> myList = new List<string>();
            string[][] sortedLists = new string[0][];


            int k = 1;

            for (int i = 0; i < clips.Length; i++)
            {
                string[] clipses = clips[i].Split(',');
                for (int j = 0; j < clipses.Length; ++j)
                {
                    if (clipses[j].Contains("\""))
                        clipses[j] = new string((char[]) clipses[j].ToCharArray().RemoveElement('"'));

                }

                if (clipses.Length == 3)
                {
                    string output = clipses[0] + ", " + clipses[1] + "\t" + clipses[2];
                    myList.Add(output);
                }
                else if (clipses.Length == 2)
                {
                    Debug.Log("Not enough data copied");
                }
                else if (clipses.Length >= 4)
                {
                    string[] allScores = new string[0];
                    k = clipses.Length - 3;
                    for (int j = 0; j < clipses.Length - 3; ++j)
                    {
                        string output = clipses[0] + ", " + clipses[1] + "\t" + clipses[j + 3];
                        allScores = (string[]) allScores.AddToArray(output);
                    }
                    sortedLists = (string[][]) sortedLists.AddToArray(allScores);
                    myList.Add(allScores[0]);
                }


            }

            Dictionary<string, int> indices = new Dictionary<string, int>();
            for (int i = 0; i < myList.Count; ++i)
            {
                indices.Add(myList[i], i);
            }
            Debug.Log("There are " + indices.Count + " students");
            myList.Sort();
            List<string[]> finalSortedLists = new List<string[]>();


            for (int i = 0; i < myList.Count; ++i)
            {
                //failing
                finalSortedLists.Add(sortedLists[indices[myList[i]]]);
            }

            myList.Clear();
            
            for (int i = 0; i < k; ++i)
            {
                //current location is here
                for (int j = 0; j < finalSortedLists.Count; ++j)
                {
                    myList.Add(finalSortedLists[j][i]);
                    Debug.Log(finalSortedLists[j][i]);
                    if (i == 0)
                    {
                        GUIUtility.systemCopyBuffer += finalSortedLists[j][i] + "\r\n";
                    }

                }

                myList.Add("\r\n");
            }

            //add to file here
            string[] print = new string[0];
            foreach (string s in myList)
            {
                print = (string[]) print.AddToArray(s);
            }

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/BigIdeasScores.txt";
            File.WriteAllLines(path, print);
            Process.Start("notepad", path);
        }
        else if (programCodeResult == 3)
        {
            //albert.io
            int points;
            if (int.TryParse(desmosPointsInput.text, out points))
            {
                Debug.Log("Sort and format complete");
                string clip = GUIUtility.systemCopyBuffer;
                string[] clips = clip.Split("\r\n".ToCharArray());
                List<string> myList = new List<string>();
                for (int i = 0; i < clips.Length; i += 10)
                {
                    if (ReformatStudentNames.studentNameReformatter.TryGetValue(clips[i], out string newName))
                    {
                        clips[i] = newName;
                    }
                    
                        
                    if (clips[i + 2]==("Submitted")||clips[i+2]=="Submitted Late")
                    {
                        myList.Add(clips[i] + "\t" + desmosPointsInput.text + "/" + desmosPointsInput.text);
                    }
                    else myList.Add(clips[i] + "\t" +"0/" + desmosPointsInput.text);

                }
                myList.Sort();
                foreach (string s in myList)
                {
                    GUIUtility.systemCopyBuffer +="\r\n" + s;
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/AlbertScores.txt";
                File.WriteAllLines(path, myList);
                Process.Start("notepad", path);
                
                //rrick rollem
                //  Clipboard.SetText("https://www.youtube.com/watch?v=oHg5SJYRHA0");
                Debug.Log("Copy a new list to sort again.");
            }
            else Debug.Log("Invalid entry. Please input a number, such as 10");
        }
        else if (programCodeResult == 4)
        {
            //AP Classroom Videos
            int points;
            if (int.TryParse(desmosPointsInput.text, out points))
            {
                Debug.Log("Sort and format complete");
                string clip = GUIUtility.systemCopyBuffer;
                string[] clips = clip.Split("\r\n".ToCharArray());
                List<string> myList = new List<string>();
                for (int i = 0; i < clips.Length; i++)
                {
                    
                    string[] clipses = clips[i].Split(' ');
                    clips[i] = "";
                    for (int j = 1; j <clipses.Length ; ++j)
                    {
                        clips[i] += clipses[j];
                        if (j <clipses.Length-1)
                            clips[i] += " ";
                    }
                    clips[i] += ", " + clipses[0];
                    
                    if (ReformatStudentNames.studentNameReformatter.TryGetValue(clips[i], out string newName))
                    {
                        clips[i] = newName;
                    }

                     
                        myList.Add(clips[i] + "\t" + desmosPointsInput.text + "/" + desmosPointsInput.text);
                }
                myList.Sort();
                GUIUtility.systemCopyBuffer = "";
                foreach (string s in myList)
                {
                    GUIUtility.systemCopyBuffer +="\r\n" + s;
                }
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/APVideoScores.txt";
                File.WriteAllLines(path, myList);
                Process.Start("notepad", path);
                
                //rrick rollem
                //  Clipboard.SetText("https://www.youtube.com/watch?v=oHg5SJYRHA0");
                Debug.Log("Copy a new list to sort again.");
            }
            else Debug.Log("Invalid entry. Please input a number, such as 10");
        }
        else
        {
            Debug.Log("Invalid Program Code.");
        }
    }
}

