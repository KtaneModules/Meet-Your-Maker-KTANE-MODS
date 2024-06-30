using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using UnityEngine.UI;
public class Button
{
    public KMSelectable Selectable { get; private set; }
    public Text Text { get; private set; }

    public Button(KMSelectable selectable, Text text)
    {
        Selectable = selectable;
        Text = text;
    }
}
