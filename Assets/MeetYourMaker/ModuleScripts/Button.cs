using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
public class Button
{
    public KMSelectable Selectable { get; private set; }
    private TextMesh textMesh;
    public string Text { get { return textMesh.text;  } }

    public Button(KMSelectable selectable, TextMesh text)
    {
        Selectable = selectable;
        textMesh = text;
    }

    public void ChangeText(string text)
    {
        //there is defintly a better way to do this
        textMesh.text = text;

        if (text.Length < 6)
        {
            textMesh.fontSize = 300;
        }

        else if (text.Length < 14)
        {
            textMesh.fontSize = 242;
        }

        else if (text.Length < 15)
        {
            textMesh.fontSize = 270;
        }

        else if (text.Length < 16)
        {
            textMesh.fontSize = 222;
        }

        else if (text.Length < 18)
        {
            textMesh.fontSize = 179;
        }

        else
        {
            textMesh.fontSize = 144;
        }
        
    }
}
