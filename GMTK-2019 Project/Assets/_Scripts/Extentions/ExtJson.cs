using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ExtJson
{
    /// <summary>
    /// convert a Rect to a Json Data
    /// </summary>
    /// <param name="rect"></param>
    /// <returns></returns>
    public static string RectToJson(Rect rect)
    {
        //{ "x":"50", "y":"30", "width":"200", "height":"421" }
        string ap = "\"";
        string json = "{ " + ap + "x" + ap + ":" + ap + rect.x + ap + ", "; //{ "x":"50", 
        json += ap + "y" + ap + ":" + ap + rect.y + ap + ", ";  //"y":"30", 
        json += ap + "width" + ap + ":" + ap + rect.width + ap + ", ";  //"width":"200", 
        json += ap + "height" + ap + ":" + ap + rect.height + ap + " }";  //"width":"421" }
        return (json);
    }

    public static Rect JsonToRect(string jsonContent)
    {
        string pattern = @"(?<=\"")([^\s,].*?)(?=\"")|null";
        MatchCollection matches = Regex.Matches(jsonContent, pattern);
        List<string> allMatch = matches.Cast<Match>().Select(m => m.Value).ToList();
        for (int i = allMatch.Count - 1; i >= 0; i--)
        {
            if (!ExtString.IsInt(allMatch[i]))
            {
                allMatch.RemoveAt(i);
            }
        }
        return (new Rect(allMatch[0].ToInt(), allMatch[1].ToInt(), allMatch[2].ToInt(), allMatch[3].ToInt()));
    }
}