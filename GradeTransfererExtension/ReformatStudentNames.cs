using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ReformatStudentNames : MonoBehaviour
{
    private string namePath;
    private string formattedPath;

    private void Awake()
    {
        namePath = Application.persistentDataPath + "/names.txt";
        formattedPath = Application.persistentDataPath + "/reformattedNames.txt";
        studentNameReformatter = new Dictionary<string, string>();
        if (File.Exists(namePath))
        {
            string[] names = File.ReadAllLines(namePath);
            string[] reformattedNames = File.ReadAllLines(formattedPath);

            for (int i = 0; i < names.Length; ++i)
            {
                studentNameReformatter.Add(names[i], reformattedNames[i]);
            }
        }
    }

    public static Dictionary<string, string> studentNameReformatter;

    private void AddStudentNames(string name, string reformatted)
    {
        if (!studentNameReformatter.ContainsKey(name))
            studentNameReformatter.Add(name, reformatted);
        else
        {
            studentNameReformatter.Remove(name);
            studentNameReformatter.Add(name,reformatted);
        }
    }

    public TMP_InputField originalNameField;
    public TMP_InputField reformattedNameField;
    public void HelperAddNames()
    {
        AddStudentNames(originalNameField.text,reformattedNameField.text);
        SaveNames();
        originalNameField.text = "";
        reformattedNameField.text = "";
    }

    private void SaveNames()
    {
        File.WriteAllLines(namePath,studentNameReformatter.Keys);
        File.WriteAllLines(formattedPath,studentNameReformatter.Values);

    }
    
}
