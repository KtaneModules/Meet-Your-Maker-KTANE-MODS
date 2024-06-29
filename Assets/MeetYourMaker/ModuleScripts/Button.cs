using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
public class Button
{
    public KMSelectable Selectable { get; private set; }
    public TextMesh TextMesh { get; private set; }

    public Button(KMSelectable selectable, TextMesh text)
    {
        Selectable = selectable;
        TextMesh = text;
    }
}
