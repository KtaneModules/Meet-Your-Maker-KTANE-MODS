using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakerModule {

	public string ModuleName { get; private set; }
    public string[] Creators { get; private set; }
	public Sprite Icon { get; private set; }
    public static Sprite[] PreloadedIcons;
    public static bool UsePreloadedIcons;

    public MakerModule(string name, string[] creators)
    {
        this.ModuleName = name;
        this.Creators = creators;
        if (UsePreloadedIcons)
        {
            Icon = PreloadedIcons.First(sprite => sprite.name == name);
        }
    }

    public MakerModule(string name, string[] creators, Sprite icon) {
        this.ModuleName = name;
        this.Creators = creators;
        Icon = icon;
    }
}
