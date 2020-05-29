﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatMenu : MonoBehaviour
{
    public GameObject loading;
    public static CheatMenu Instance;
    private string[] Levels = new string[] 
    {
        "Level1",
        "Level2",
        "Level3",
        "Level4",
        "Level5",
        "Level6",
        "Level7",
        "Level8",
        "Level9",
        "Level10",
        "Level11",
        "Level12",
        "Level13",
        "Level14",
        "Level15",
        "Level16",
        "Level17",
        "Level18",
        "Level19",
        "Level20",
        "Level21",
        "Level22",
        "Level23",
        "Level24",
        "Level25",
        "Level26",
        "Level27",
        "Level28",
        "Level29",
        "Level30",
        "Level31",
        "Level32",
    };
    private int Cutscene;

    private void Start()
    {
        Instance = this;
    }
    public void ChangeLevel(string level)
    {
        player.Instance.CheatMenu.SetActive(false);
        StartCoroutine(Reload(level));
    }
    private IEnumerator Reload(string level)
    {
        SetupLoading(true);
        SaturationControl.lastIndex = System.Array.IndexOf(Levels, level);
        List<string> listUnload = new List<string>();
        bool haveExterno = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Externo")
            {
                haveExterno = true;
            }
            else
            {
                if (SceneManager.GetSceneAt(i).name != "HUB")
                {
                    listUnload.Add(SceneManager.GetSceneAt(i).name);
                }
            }
        }
        if (haveExterno) yield return SceneManager.UnloadSceneAsync("Externo");
        while (listUnload.Count > 0)
        {
            yield return SceneManager.UnloadSceneAsync(Levels[System.Array.IndexOf(Levels, listUnload[0])]);
            listUnload.RemoveAt(0);
        }
        yield return SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        for (int i = 0; i < HubEvents.Instance.levels[System.Array.IndexOf(Levels, level)].LevelLoad.Length; i++)
        {
            yield return SceneManager.LoadSceneAsync(HubEvents.Instance.levels[System.Array.IndexOf(Levels, level)].LevelLoad[i], LoadSceneMode.Additive);
        }
        yield return new WaitForEndOfFrame();
        //player.Instance.transform.position = new Vector3 (HubEvents.transforms[System.Array.IndexOf(Levels, level)].transform.position.x - 3, HubEvents.transforms[System.Array.IndexOf(Levels, level)].transform.position.y, HubEvents.transforms[System.Array.IndexOf(Levels, level)].transform.position.z) ;
        SetupLoading(false);
    }
    public void ChangeCutscene(int index)
    {
        player.Instance.CheatMenu.SetActive(false);
        StartCoroutine(CutsceneOverride(index));
    }
    private IEnumerator CutsceneOverride(int index)
    {
        SetupLoading(true);
        List<string> listUnload = new List<string>();
        bool haveExterno = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == "Externo")
            {
                haveExterno = true;
            }
            else
            {
                if (SceneManager.GetSceneAt(i).name != "HUB")
                {
                    listUnload.Add(SceneManager.GetSceneAt(i).name);
                }
            }
        }
        if (haveExterno) yield return SceneManager.UnloadSceneAsync("Externo");
        while (listUnload.Count > 0)
        {
            yield return SceneManager.UnloadSceneAsync(Levels[System.Array.IndexOf(Levels, listUnload[0])]);
            listUnload.RemoveAt(0);
        }
        yield return SceneManager.LoadSceneAsync("Level1", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level8", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level9", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level16", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level17", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level24", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level25", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level32", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Externo", LoadSceneMode.Additive);
        if (index == 5 || index == 6) yield return SceneManager.LoadSceneAsync("Creditos", LoadSceneMode.Additive);
        player.Instance.gameObject.transform.position = new Vector3(-10, 1.64f, 0);
        SetupLoading(false);
        if (index != 6) LevelLoader.Instance.Cutscene(index);
        else
        {
            HubEvents.Instance.FadeOut();
            HubEvents.Instance.Creditos();
        }
    }
    private void SetupLoading(bool value)
    {
        player.Instance.CutsceneMode = value;
        loading.SetActive(value);
        if (value)
        {
            for(int i = 0; i < loading.transform.childCount; i++)
            {
                loading.transform.GetChild(i).gameObject.SetActive(i == 1);
            }
        }
    }
}
