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
    private const int maxJsonLoadTime = 10; //the max time allowed to laod the json
    private const int maxTime = 25; //the time allowed to load as many modules as possible
    public static List<MakerModule> LoadedModules;
    private static Stopwatch stopWatch = new Stopwatch();
    private static string[] bannedCreators = new string[]{ "Anonymous", "and many contributors" };


    public static IEnumerator LoadData()
    {
        //Stores the raw text of the grabbed json.
        string raw = "";
        UnityWebRequest request = UnityWebRequest.Get("https://ktane.timwi.de/json/raw");
        stopWatch.Start();
        request.SendWebRequest();
        do
        {
            if (stopWatch.Elapsed.TotalSeconds >= maxJsonLoadTime)
            {
                UnityEngineDebug("JSON took too long to load. Using preloaded modules");
                StopLoading();
                break;
            }
            yield return null;
        }
        while (!request.isDone);
        //If an error occurs, we need to default to the hardcoded file.
        if (request.error != null)
        {
            Success = false;
            UnityEngineDebug("Failed to get data!");
        }
        else
        {
            Success = stopWatch.Elapsed.TotalSeconds < maxJsonLoadTime;

            if (!Success)
            {
                UnityEngineDebug("JSON took too long to load. Using preloaded modules");
                StopLoading();
                yield break;
            }

            UnityEngineDebug("Gotten info!");
            raw = request.downloadHandler.text;

            //Turns the raw JSON into an instance of the container class, which contains a List of Dictionaries.
            List<KtaneModule> modData = RepoJSONParser.ParseRaw(raw);
            modData = RandomizeList(modData);
            LoadedModules = new List<MakerModule>();

            foreach (KtaneModule module in modData)
            {
                if (module.Type != "Regular" || module.Contributors == null)
                {
                    continue;
                }

                List<string> creators = module.Contributors.GetContributors();

                if(creators.Count == 0 || creators.Any(creator => bannedCreators.Contains(creator))) 
                {
                    continue;
                }
                UnityWebRequest www = UnityWebRequestTexture.GetTexture($"https://ktane.timwi.de/Icons/{module.Name}.png");

                yield return www.SendWebRequest();

                if(!www.isNetworkError && !www.isHttpError)
                {
                    Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
                    Sprite sprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height), new Vector2(0.5f, 0.5f));
                    LoadedModules.Add(new MakerModule(module.Name, creators.ToArray(), sprite));
                }

                if (stopWatch.Elapsed.TotalSeconds >= maxTime)
                {
                    break;
                }
            }

        }

        LoadedModules = LoadedModules.OrderBy(module => module.ModuleName).ToList();
        StopLoading();
    }

    private static void StopLoading()
    {
        Loading = false;
        LoadingDone = false;
        stopWatch.Stop();
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
}
