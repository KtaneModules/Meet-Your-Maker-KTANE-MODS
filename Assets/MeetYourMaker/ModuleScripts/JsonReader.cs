using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;


public class JsonReader {

	public static bool Success;
    public static bool LoadingDone;
    public static bool Loading;
    private const int maxTime = 25; //the time allowed to load as many modules as possible

    public static IEnumerator LoadData(SpriteRenderer icon)
    {
        //Stores the raw text of the grabbed json.
        string raw = "";
        UnityWebRequest request = UnityWebRequest.Get("https://ktane.timwi.de/json/raw");
        //Waits until the web request returns the JSON file.
        yield return request.SendWebRequest();
        //If an error occurs, we need to default to the hardcoded file.
        if (request.error != null)
        {
            Success = false;
            UnityEngineDebug("Failed to get data!");
        }
        else
        {
            UnityEngineDebug("Gotten info!");
            raw = request.downloadHandler.text;
            Success = true;
        }
        //Turns the raw JSON into an instance of the container class, which contains a List of Dictionaries.
        List<KtaneModule> modData = RepoJSONParser.ParseRaw(raw);
        modData = RandomizeList(modData).ToList();
        List<MakerModule> usableModules = new List<MakerModule>();

        int count = 0;
        Stopwatch stopWatch = new Stopwatch();

        foreach (KtaneModule module in modData)
        {
            stopWatch.Start();
            if (module.Type != "Regular" && module.Type != "Needy" || module.Contributors == null)
            {
                continue;
            }
            UnityWebRequest www = UnityWebRequestTexture.GetTexture($"https://ktane.timwi.de/Icons/{module.Name}.png");

            yield return www.SendWebRequest();
            if(!www.isNetworkError && !www.isHttpError)
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
                Sprite sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));
                List<string> creators = new List<string>();
                creators.AddRange(module.Contributors.Developer);
                if (module.Contributors.Manual != null)
                { 
                    creators.AddRange(module.Contributors.Manual);
                }
                usableModules.Add(new MakerModule(module.Name, creators.ToArray(), sprite));
                UnityEngineDebug("Loaded " + module.Name);
                count++;
            }

            if (stopWatch.Elapsed.TotalSeconds >= maxTime)
            {
                UnityEngineDebug($"Loaded {count} modules");
                break;
            }
        }




        Loading = false;
        LoadingDone = true;
    }
    private static List<KtaneModule> RandomizeList(List<KtaneModule> orgininal)
    {
        List<KtaneModule> newList = new List<KtaneModule>();
        List<KtaneModule> oldList = new List<KtaneModule>(orgininal);
        while (oldList.Count > 0)
        {
            KtaneModule result = oldList.PickRandom();
            oldList.Remove(result);
            newList.Add(result);
        }
        return newList;
    }

    private static void UnityEngineDebug(string message)
    {
        UnityEngine.Debug.Log(message);
    }
    private static void UnityEngineDebug(TimeSpan message)
    {
        UnityEngine.Debug.Log(message);
    }
}
