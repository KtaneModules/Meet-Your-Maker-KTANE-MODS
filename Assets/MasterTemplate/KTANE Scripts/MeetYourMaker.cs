using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;
using UnityEngine.UI;

public class MeetYourMaker : MonoBehaviour {
    static int ModuleIdCounter = 1;
    private KMBombInfo Bomb;
    private KMAudio Audio;
    private KMNeedyModule Needy;
    int ModuleId;
    private bool ModuleSolved;


    #region Buttons/Icons
    [SerializeField]
    private Image[] images;
    private Image blankIcon;
    private Button[] buttons;
    private Sprite icon;
    #endregion
    void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
        ModuleId = ModuleIdCounter++;
        Bomb = GetComponent<KMBombInfo>();
        Audio = GetComponent<KMAudio>();
        Needy = GetComponent<KMNeedyModule>();
        GetComponent<KMBombModule>().OnActivate += Activate;
        Needy.OnNeedyActivation += OnNeedyActivation;
        Needy.OnNeedyDeactivation += OnNeedyDeactivation;
        Needy.OnTimerExpired += OnTimerExpired;

        buttons = new Button[4];
        for (int i = 1; i < 5; i++)
        {
            Debug.Log(i);
            GameObject gameObject = transform.Find($"Button{i}").gameObject;
            KMSelectable selectable = gameObject.GetComponent<KMSelectable>();
            selectable.OnInteract += delegate () { ButtonPress(selectable); return false; };
            buttons[i] = new Button(selectable, gameObject.GetComponent<TextMesh>());
        }
    }

    void OnDestroy () { //Shit you need to do when the bomb ends
      
   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

   protected void OnNeedyActivation () { //Shit that happens when a needy turns on.

   }

   protected void OnNeedyDeactivation () { //Shit that happens when a needy turns off.
      Needy.OnPass();
   }

   protected void OnTimerExpired () { //Shit that happens when a needy turns off due to running out of time.

   }

   void Start () { //Shit that you calculate, usually a majority if not all of the module
      Needy.SetResetDelayTime(30f, 50f);
   }

   void Update () { //Shit that happens at any point after initialization

   }

    private void ButtonPress(KMSelectable button)
    {
        Debug.Log($"{button} was pressed");
    }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
   }

   void TwitchHandleForcedSolve () { //Void so that autosolvers go to it first instead of potentially striking due to running out of time.
      StartCoroutine(HandleAutosolver());
   }

   IEnumerator HandleAutosolver () {
      yield return null;
   }
}
