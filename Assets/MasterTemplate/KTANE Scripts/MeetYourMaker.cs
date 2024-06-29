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
    private static JsonReader jsonData;

    private List<MakerModule> modules;

    #region Buttons/Icons
    [SerializeField]
    private Sprite[] preloadedIcons;
    [SerializeField]
    private Sprite blankIcon;
    [SerializeField]
    private Sprite icon;
    private Button[] buttons;
    #endregion
    void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
        ModuleId = ModuleIdCounter++;
        Bomb = GetComponent<KMBombInfo>();
        Audio = GetComponent<KMAudio>();
        Needy = GetComponent<KMNeedyModule>();
        Needy.OnNeedyActivation += OnNeedyActivation;
        Needy.OnNeedyDeactivation += OnNeedyDeactivation;
        Needy.OnTimerExpired += OnTimerExpired;

        buttons = new Button[4];
        for (int i = 1; i < 5; i++)
        {
            int dummy = i;
            Debug.Log(dummy);
            Transform transform = this.transform.Find($"Button {dummy}");
            Debug.Log(transform);
            KMSelectable selectable = transform.GetComponent<KMSelectable>();
            Debug.Log(selectable);
            buttons[dummy - 1] = new Button(selectable, transform.Find("Text").GetComponent<TextMesh>());
            selectable.OnInteract += delegate () { ButtonPress(buttons[dummy - 1]); return false; };
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
        modules = new List<MakerModule>();
        //if the json failed to load, use the data from the appendix
        if (!JsonReader.Success)
        {
            Debug.Log("Unable to connect to the repo, using preloaded modules");
            LoadPreloadedModules();

        }
      Needy.SetResetDelayTime(30f, 50f);
   }

   void Update () { //Shit that happens at any point after initialization

   }

    private void ButtonPress(Button button)
    {
        Debug.Log($"{button.Selectable.name} was pressed");
    }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

    private void LoadPreloadedModules()
    {
        MakerModule.PreloadedIcons = preloadedIcons;
        MakerModule.UsePreloadedIcons = true;
        modules.Add(new MakerModule("3D Maze", new string[] { }));
        modules.Add(new MakerModule("3D Tunnels", new string[] { }));
        modules.Add(new MakerModule("101 Dalmatians", new string[] { }));
        modules.Add(new MakerModule("Accumulation", new string[] { }));
        modules.Add(new MakerModule("Adjacent Letters", new string[] { }));
        modules.Add(new MakerModule("Adventure Game", new string[] { }));
        modules.Add(new MakerModule("Algebra", new string[] { }));
        modules.Add(new MakerModule("Alphabet", new string[] { }));
        modules.Add(new MakerModule("Alphabet Numbers", new string[] { }));
        modules.Add(new MakerModule("Anagrams", new string[] { }));
        modules.Add(new MakerModule("Arithmelogic", new string[] { }));
        modules.Add(new MakerModule("Astrology", new string[] { }));
        modules.Add(new MakerModule("Backgrounds", new string[] { }));
        modules.Add(new MakerModule("Bartending", new string[] { }));
        modules.Add(new MakerModule("Bases", new string[] { }));
        modules.Add(new MakerModule("Battleship", new string[] { }));
        modules.Add(new MakerModule("Benedict Cumberbatch", new string[] { }));
        modules.Add(new MakerModule("Big Circle", new string[] { }));
        modules.Add(new MakerModule("Binary LEDs", new string[] { }));
        modules.Add(new MakerModule("Binary Puzzle", new string[] { }));
        modules.Add(new MakerModule("Binary Tree", new string[] { }));
        modules.Add(new MakerModule("Bitmaps", new string[] { }));
        modules.Add(new MakerModule("Bitwise Operations", new string[] { }));
        modules.Add(new MakerModule("Black Hole", new string[] { }));
        modules.Add(new MakerModule("Blackjack", new string[] { }));
        modules.Add(new MakerModule("Blind Alley", new string[] { }));
        modules.Add(new MakerModule("Blind Maze", new string[] { }));
        modules.Add(new MakerModule("Blockbusters", new string[] { }));
        modules.Add(new MakerModule("Boggle", new string[] { }));
        modules.Add(new MakerModule("Boolean Maze", new string[] { }));
        modules.Add(new MakerModule("Boolean Venn Diagram", new string[] { }));
        modules.Add(new MakerModule("Braille", new string[] { }));
        modules.Add(new MakerModule("British Slang", new string[] { }));
        modules.Add(new MakerModule("Broken Buttons", new string[] { }));
        modules.Add(new MakerModule("Broken Guitar Chords", new string[] { }));
        modules.Add(new MakerModule("Brush Strokes", new string[] { }));
        modules.Add(new MakerModule("Burger Alarm", new string[] { }));
        modules.Add(new MakerModule("Burglar Alarm", new string[] { }));
        modules.Add(new MakerModule("Button Grid", new string[] { }));
        modules.Add(new MakerModule("Button Sequence", new string[] { }));
        modules.Add(new MakerModule("Caesar Cipher", new string[] { }));
        modules.Add(new MakerModule("Calendar", new string[] { }));
        modules.Add(new MakerModule("Catchphrase", new string[] { }));
        modules.Add(new MakerModule("Challenge & Contact", new string[] { }));
        modules.Add(new MakerModule("Character Shift", new string[] { }));
        modules.Add(new MakerModule("Cheap Checkout", new string[] { }));
        modules.Add(new MakerModule("Christmas Presents", new string[] { }));
        modules.Add(new MakerModule("Coffeebucks", new string[] { }));
        modules.Add(new MakerModule("Color Decoding", new string[] { }));
        modules.Add(new MakerModule("Color Generator", new string[] { }));
        modules.Add(new MakerModule("Color Math", new string[] { }));
        modules.Add(new MakerModule("Color Morse", new string[] { }));
        modules.Add(new MakerModule("Colored Keys", new string[] { }));
        modules.Add(new MakerModule("Colored Squares", new string[] { }));
        modules.Add(new MakerModule("Colored Switches", new string[] { }));
        modules.Add(new MakerModule("Colorful Insanity", new string[] { }));
        modules.Add(new MakerModule("Colorful Madness", new string[] { }));
        modules.Add(new MakerModule("Colour Code", new string[] { }));
        modules.Add(new MakerModule("Colour Flash", new string[] { }));
        modules.Add(new MakerModule("Complex Keypad", new string[] { }));
        modules.Add(new MakerModule("Complicated Buttons", new string[] { }));
        modules.Add(new MakerModule("Connection Check", new string[] { }));
        modules.Add(new MakerModule("Connection Device", new string[] { }));
        modules.Add(new MakerModule("Cookie Jars", new string[] { }));
        modules.Add(new MakerModule("Cooking", new string[] { }));
        modules.Add(new MakerModule("Coordinates", new string[] { }));
        modules.Add(new MakerModule("Countdown", new string[] { }));
        modules.Add(new MakerModule("Crackbox", new string[] { }));
        modules.Add(new MakerModule("Crazy Talk", new string[] { }));
        modules.Add(new MakerModule("Creation", new string[] { }));
        modules.Add(new MakerModule("Cryptography", new string[] { }));
        modules.Add(new MakerModule("Curriculum", new string[] { }));
        modules.Add(new MakerModule("Decolored Squares", new string[] { }));
        modules.Add(new MakerModule("DetoNATO", new string[] { }));
        modules.Add(new MakerModule("Digit String", new string[] { }));
        modules.Add(new MakerModule("Digital Cipher", new string[] { }));
        modules.Add(new MakerModule("Digital Root", new string[] { }));
        modules.Add(new MakerModule("Discolored Squares", new string[] { }));
        modules.Add(new MakerModule("Divided Squares", new string[] { }));
        modules.Add(new MakerModule("Dominoes", new string[] { }));
        modules.Add(new MakerModule("Double Color", new string[] { }));
        modules.Add(new MakerModule("Double-Oh", new string[] { }));
        modules.Add(new MakerModule("Dr. Doctor", new string[] { }));
        modules.Add(new MakerModule("Dragon Energy", new string[] { }));
        modules.Add(new MakerModule("Elder Futhark", new string[] { }));
        modules.Add(new MakerModule("Emoji Math", new string[] { }));
        modules.Add(new MakerModule("Encrypted Morse", new string[] { }));
        modules.Add(new MakerModule("English Test", new string[] { }));
        modules.Add(new MakerModule("Equations", new string[] { }));
        modules.Add(new MakerModule("Equations X", new string[] { }));
        modules.Add(new MakerModule("Error Codes", new string[] { }));
        modules.Add(new MakerModule("European Travel", new string[] { }));
        modules.Add(new MakerModule("Extended Password", new string[] { }));
        modules.Add(new MakerModule("Factory Maze", new string[] { }));
        modules.Add(new MakerModule("Fast Math", new string[] { }));
        modules.Add(new MakerModule("Faulty Sink", new string[] { }));
        modules.Add(new MakerModule("FizzBuzz", new string[] { }));
        modules.Add(new MakerModule("Flags", new string[] { }));
        modules.Add(new MakerModule("Flashing Lights", new string[] { }));
        modules.Add(new MakerModule("Flavor Text", new string[] { }));
        modules.Add(new MakerModule("Follow the Leader", new string[] { }));
        modules.Add(new MakerModule("Font Select", new string[] { }));
        modules.Add(new MakerModule("Foreign Exchange Rates", new string[] { }));
        modules.Add(new MakerModule("Forget Everything", new string[] { }));
        modules.Add(new MakerModule("Forget Me Not", new string[] { }));
        modules.Add(new MakerModule("Forget This", new string[] { }));
        modules.Add(new MakerModule("Four-Card Monte", new string[] { }));
        modules.Add(new MakerModule("Free Parking", new string[] { }));
        modules.Add(new MakerModule("Friendship", new string[] { }));
        modules.Add(new MakerModule("Functions", new string[] { }));
        modules.Add(new MakerModule("Gadgetron Vendor", new string[] { }));
        modules.Add(new MakerModule("Game of Life Simple", new string[] { }));
        modules.Add(new MakerModule("Genetic Sequence", new string[] { }));
        modules.Add(new MakerModule("Graffiti Numbers", new string[] { }));
        modules.Add(new MakerModule("Graphic Memory", new string[] { }));
        modules.Add(new MakerModule("Greek Calculus", new string[] { }));
        modules.Add(new MakerModule("Grid Matching", new string[] { }));
        modules.Add(new MakerModule("Gridlock", new string[] { }));
        modules.Add(new MakerModule("Grocery Store", new string[] { }));
        modules.Add(new MakerModule("Gryphons", new string[] { }));
        modules.Add(new MakerModule("Guitar Chords", new string[] { }));
        modules.Add(new MakerModule("Harmony Sequence", new string[] { }));
        modules.Add(new MakerModule("Hexamaze", new string[] { }));
        modules.Add(new MakerModule("Hidden Colors", new string[] { }));
        modules.Add(new MakerModule("Hieroglyphics", new string[] { }));
        modules.Add(new MakerModule("Hogwarts", new string[] { }));
        modules.Add(new MakerModule("Homophones", new string[] { }));
        modules.Add(new MakerModule("Horrible Memory", new string[] { }));
        modules.Add(new MakerModule("Human Resources", new string[] { }));
        modules.Add(new MakerModule("Hunting", new string[] { }));
        modules.Add(new MakerModule("Ice Cream", new string[] { }));
        modules.Add(new MakerModule("Identity Parade", new string[] { }));
        modules.Add(new MakerModule("IKEA", new string[] { }));
        modules.Add(new MakerModule("Instructions", new string[] { }));
        modules.Add(new MakerModule("Know Your Way", new string[] { }));
        modules.Add(new MakerModule("Krazy Talk", new string[] { }));
        modules.Add(new MakerModule("Kudosudoku", new string[] { }));
        modules.Add(new MakerModule("Lasers", new string[] { }));
        modules.Add(new MakerModule("Laundry", new string[] { }));
        modules.Add(new MakerModule("LED Encryption", new string[] { }));
        modules.Add(new MakerModule("LED Grid", new string[] { }));
        modules.Add(new MakerModule("LED Math", new string[] { }));
        modules.Add(new MakerModule("Left and Right", new string[] { }));
        modules.Add(new MakerModule("LEGOs", new string[] { }));
        modules.Add(new MakerModule("Letter Keys", new string[] { }));
        modules.Add(new MakerModule("Light Cycle", new string[] { }));
        modules.Add(new MakerModule("Lightspeed", new string[] { }));
        modules.Add(new MakerModule("Lion’s Share", new string[] { }));
        modules.Add(new MakerModule("Listening", new string[] { }));
        modules.Add(new MakerModule("Logic", new string[] { }));
        modules.Add(new MakerModule("Logic Gates", new string[] { }));
        modules.Add(new MakerModule("Logical Buttons", new string[] { }));
        modules.Add(new MakerModule("Lombax Cubes", new string[] { }));
        modules.Add(new MakerModule("Mad Memory", new string[] { }));
        modules.Add(new MakerModule("Mafia", new string[] { }));
        modules.Add(new MakerModule("Mahjong", new string[] { }));
        modules.Add(new MakerModule("Maintenance", new string[] { }));
        modules.Add(new MakerModule("Manometers", new string[] { }));
        modules.Add(new MakerModule("Marble Tumble", new string[] { }));
        modules.Add(new MakerModule("Maritime Flags", new string[] { }));
        modules.Add(new MakerModule("Mashematics", new string[] { }));
        modules.Add(new MakerModule("Mastermind Simple", new string[] { }));
        modules.Add(new MakerModule("Maze Scrambler", new string[] { }));
        modules.Add(new MakerModule("Mazematics", new string[] { }));
        modules.Add(new MakerModule("Maze³", new string[] { }));
        modules.Add(new MakerModule("Mega Man 2", new string[] { }));
        modules.Add(new MakerModule("Melody Sequencer", new string[] { }));
        modules.Add(new MakerModule("Micro-Modules", new string[] { }));
        modules.Add(new MakerModule("Microcontroller", new string[] { }));
        modules.Add(new MakerModule("Mineseeker", new string[] { }));
        modules.Add(new MakerModule("Minesweeper", new string[] { }));
        modules.Add(new MakerModule("Modern Cipher", new string[] { }));
        modules.Add(new MakerModule("Module Homework", new string[] { }));
        modules.Add(new MakerModule("Module Maze", new string[] { }));
        modules.Add(new MakerModule("Modules Against Humanity", new string[] { }));
        modules.Add(new MakerModule("Modulo", new string[] { }));
        modules.Add(new MakerModule("Monsplode Trading Cards", new string[] { }));
        modules.Add(new MakerModule("Monsplode, Fight!", new string[] { }));
        modules.Add(new MakerModule("Morse Buttons", new string[] { }));
        modules.Add(new MakerModule("Morse War", new string[] { }));
        modules.Add(new MakerModule("Morse-A-Maze", new string[] { }));
        modules.Add(new MakerModule("Morsematics", new string[] { }));
        modules.Add(new MakerModule("Mortal Kombat", new string[] { }));
        modules.Add(new MakerModule("Mouse in the Maze", new string[] { }));
        modules.Add(new MakerModule("Murder", new string[] { }));
        modules.Add(new MakerModule("Mystic Square", new string[] { }));
        modules.Add(new MakerModule("Neutralization", new string[] { }));
        modules.Add(new MakerModule("Nonogram", new string[] { }));
        modules.Add(new MakerModule("Number Nimbleness", new string[] { }));
        modules.Add(new MakerModule("Number Pad", new string[] { }));
        modules.Add(new MakerModule("Numbers", new string[] { }));
        modules.Add(new MakerModule("Odd One Out", new string[] { }));
        modules.Add(new MakerModule("Only Connect", new string[] { }));
        modules.Add(new MakerModule("Orientation Cube", new string[] { }));
        modules.Add(new MakerModule("Painting", new string[] { }));
        modules.Add(new MakerModule("Party Time", new string[] { }));
        modules.Add(new MakerModule("Passport Control", new string[] { }));
        modules.Add(new MakerModule("Pattern Cube", new string[] { }));
        modules.Add(new MakerModule("Periodic Table", new string[] { }));
        modules.Add(new MakerModule("Perplexing Wires", new string[] { }));
        modules.Add(new MakerModule("Perspective Pegs", new string[] { }));
        modules.Add(new MakerModule("Piano Keys", new string[] { }));
        modules.Add(new MakerModule("Pie", new string[] { }));
        modules.Add(new MakerModule("Pigpen Rotations", new string[] { }));
        modules.Add(new MakerModule("Planets", new string[] { }));
        modules.Add(new MakerModule("Playfair Cipher", new string[] { }));
        modules.Add(new MakerModule("Plumbing", new string[] { }));
        modules.Add(new MakerModule("Poetry", new string[] { }));
        modules.Add(new MakerModule("Point of Order", new string[] { }));
        modules.Add(new MakerModule("Poker", new string[] { }));
        modules.Add(new MakerModule("Polyhedral Maze", new string[] { }));
        modules.Add(new MakerModule("Press X", new string[] { }));
        modules.Add(new MakerModule("Probing", new string[] { }));
        modules.Add(new MakerModule("Purgatory", new string[] { }));
        modules.Add(new MakerModule("Question Mark", new string[] { }));
        modules.Add(new MakerModule("Quintuples", new string[] { }));
        modules.Add(new MakerModule("Quiz Buzz", new string[] { }));
        modules.Add(new MakerModule("Radiator", new string[] { }));
        modules.Add(new MakerModule("Regular Crazy Talk", new string[] { }));
        modules.Add(new MakerModule("Resistors", new string[] { }));
        modules.Add(new MakerModule("Retirement", new string[] { }));
        modules.Add(new MakerModule("Reverse Morse", new string[] { }));
        modules.Add(new MakerModule("Rhythms", new string[] { }));
        modules.Add(new MakerModule("Rock-Paper-Scissors-Lizard-Spock", new string[] { }));
        modules.Add(new MakerModule("Roman Art", new string[] { }));
        modules.Add(new MakerModule("Round Keypad", new string[] { }));
        modules.Add(new MakerModule("Rubik’s Clock", new string[] { }));
        modules.Add(new MakerModule("Rubik’s Cube", new string[] { }));
        modules.Add(new MakerModule("S.E.T.", new string[] { }));
        modules.Add(new MakerModule("Safety Safe", new string[] { }));
        modules.Add(new MakerModule("Schlag den Bomb", new string[] { }));
        modules.Add(new MakerModule("Scripting", new string[] { }));
        modules.Add(new MakerModule("Sea Shells", new string[] { }));
        modules.Add(new MakerModule("Semaphore", new string[] { }));
        modules.Add(new MakerModule("Seven Wires", new string[] { }));
        modules.Add(new MakerModule("Shape Shift", new string[] { }));
        modules.Add(new MakerModule("Shapes and Bombs", new string[] { }));
        modules.Add(new MakerModule("Shikaku", new string[] { }));
        modules.Add(new MakerModule("Signals", new string[] { }));
        modules.Add(new MakerModule("Silly Slots", new string[] { }));
        modules.Add(new MakerModule("Simon Samples", new string[] { }));
        modules.Add(new MakerModule("Simon Scrambles", new string[] { }));
        modules.Add(new MakerModule("Simon Screams", new string[] { }));
        modules.Add(new MakerModule("Simon Sends", new string[] { }));
        modules.Add(new MakerModule("Simon Shrieks", new string[] { }));
        modules.Add(new MakerModule("Simon Sings", new string[] { }));
        modules.Add(new MakerModule("Simon Sounds", new string[] { }));
        modules.Add(new MakerModule("Simon Speaks", new string[] { }));
        modules.Add(new MakerModule("Simon Spins", new string[] { }));
        modules.Add(new MakerModule("Simon States", new string[] { }));
        modules.Add(new MakerModule("Simon Stops", new string[] { }));
        modules.Add(new MakerModule("Simon’s Stages", new string[] { }));
        modules.Add(new MakerModule("Simon’s Star", new string[] { }));
        modules.Add(new MakerModule("Sink", new string[] { }));
        modules.Add(new MakerModule("Skewed Slots", new string[] { }));
        modules.Add(new MakerModule("Skinny Wires", new string[] { }));
        modules.Add(new MakerModule("Skyrim", new string[] { }));
        modules.Add(new MakerModule("Snooker", new string[] { }));
        modules.Add(new MakerModule("Sonic & Knuckles", new string[] { }));
        modules.Add(new MakerModule("Sonic the Hedgehog", new string[] { }));
        modules.Add(new MakerModule("Souvenir", new string[] { }));
        modules.Add(new MakerModule("Spinning Buttons", new string[] { }));
        modules.Add(new MakerModule("Splitting the Loot", new string[] { }));
        modules.Add(new MakerModule("Square Button", new string[] { }));
        modules.Add(new MakerModule("Stack'em", new string[] { }));
        modules.Add(new MakerModule("Street Fighter", new string[] { }));
        modules.Add(new MakerModule("Subscribe to Pewdiepie", new string[] { }));
        modules.Add(new MakerModule("Subways", new string[] { }));
        modules.Add(new MakerModule("Sueet Wall", new string[] { }));
        modules.Add(new MakerModule("Superlogic", new string[] { }));
        modules.Add(new MakerModule("Switches", new string[] { }));
        modules.Add(new MakerModule("Symbol Cycle", new string[] { }));
        modules.Add(new MakerModule("Symbolic Coordinates", new string[] { }));
        modules.Add(new MakerModule("Symbolic Password", new string[] { }));
        modules.Add(new MakerModule("SYNC-125 [3]", new string[] { }));
        modules.Add(new MakerModule("Synchronization", new string[] { }));
        modules.Add(new MakerModule("Synonyms", new string[] { }));
        modules.Add(new MakerModule("T-Words", new string[] { }));
        modules.Add(new MakerModule("Tangrams", new string[] { }));
        modules.Add(new MakerModule("Tap Code", new string[] { }));
        modules.Add(new MakerModule("Tax Returns", new string[] { }));
        modules.Add(new MakerModule("Ten-Button Color Code", new string[] { }));
        modules.Add(new MakerModule("Tennis", new string[] { }));
        modules.Add(new MakerModule("Terraria Quiz", new string[] { }));
        modules.Add(new MakerModule("Text Field", new string[] { }));
        modules.Add(new MakerModule("The Bulb", new string[] { }));
        modules.Add(new MakerModule("The Clock", new string[] { }));
        modules.Add(new MakerModule("The Code", new string[] { }));
        modules.Add(new MakerModule("The Crystal Maze", new string[] { }));
        modules.Add(new MakerModule("The Cube", new string[] { }));
        modules.Add(new MakerModule("The Digit", new string[] { }));
        modules.Add(new MakerModule("The Festive Jukebox", new string[] { }));
        modules.Add(new MakerModule("The Gamepad", new string[] { }));
        modules.Add(new MakerModule("The Giant’s Drink", new string[] { }));
        modules.Add(new MakerModule("The Hangover", new string[] { }));
        modules.Add(new MakerModule("The Hexabutton", new string[] { }));
        modules.Add(new MakerModule("The Hypercube", new string[] { }));
        modules.Add(new MakerModule("The iPhone", new string[] { }));
        modules.Add(new MakerModule("The Jack-O'-Lantern", new string[] { }));
        modules.Add(new MakerModule("The Jewel Vault", new string[] { }));
        modules.Add(new MakerModule("The Jukebox", new string[] { }));
        modules.Add(new MakerModule("The Labyrinth", new string[] { }));
        modules.Add(new MakerModule("The London Underground", new string[] { }));
        modules.Add(new MakerModule("The Moon", new string[] { }));
        modules.Add(new MakerModule("The Necronomicon", new string[] { }));
        modules.Add(new MakerModule("The Number", new string[] { }));
        modules.Add(new MakerModule("The Number Cipher", new string[] { }));
        modules.Add(new MakerModule("The Plunger Button", new string[] { }));
        modules.Add(new MakerModule("The Radio", new string[] { }));
        modules.Add(new MakerModule("The Screw", new string[] { }));
        modules.Add(new MakerModule("The Sphere", new string[] { }));
        modules.Add(new MakerModule("The Stare", new string[] { }));
        modules.Add(new MakerModule("The Stock Market", new string[] { }));
        modules.Add(new MakerModule("The Stopwatch", new string[] { }));
        modules.Add(new MakerModule("The Sun", new string[] { }));
        modules.Add(new MakerModule("The Swan", new string[] { }));
        modules.Add(new MakerModule("The Switch", new string[] { }));
        modules.Add(new MakerModule("The Time Keeper", new string[] { }));
        modules.Add(new MakerModule("The Triangle", new string[] { }));
        modules.Add(new MakerModule("The Troll", new string[] { }));
        modules.Add(new MakerModule("The Wire", new string[] { }));
        modules.Add(new MakerModule("The Witness", new string[] { }));
        modules.Add(new MakerModule("Third Base", new string[] { }));
        modules.Add(new MakerModule("Tic Tac Toe", new string[] { }));
        modules.Add(new MakerModule("Timezone", new string[] { }));
        modules.Add(new MakerModule("Timing is Everything", new string[] { }));
        modules.Add(new MakerModule("Triangle Buttons", new string[] { }));
        modules.Add(new MakerModule("Turn the Key", new string[] { }));
        modules.Add(new MakerModule("Turn the Keys", new string[] { }));
        modules.Add(new MakerModule("Turtle Robot", new string[] { }));
        modules.Add(new MakerModule("Two Bits", new string[] { }));
        modules.Add(new MakerModule("Unfair Cipher", new string[] { }));
        modules.Add(new MakerModule("Unrelated Anagrams", new string[] { }));
        modules.Add(new MakerModule("USA Maze", new string[] { }));
        modules.Add(new MakerModule("Valves", new string[] { }));
        modules.Add(new MakerModule("Varicolored Squares", new string[] { }));
        modules.Add(new MakerModule("Vexillology", new string[] { }));
        modules.Add(new MakerModule("Visual Impairment", new string[] { }));
        modules.Add(new MakerModule("Waste Management", new string[] { }));
        modules.Add(new MakerModule("Wavetapping", new string[] { }));
        modules.Add(new MakerModule("Web Design", new string[] { }));
        modules.Add(new MakerModule("Westeros", new string[] { }));
        modules.Add(new MakerModule("Wire Placement", new string[] { }));
        modules.Add(new MakerModule("Wire Spaghetti", new string[] { }));
        modules.Add(new MakerModule("Word Search", new string[] { }));
        modules.Add(new MakerModule("X-Ray", new string[] { }));
        modules.Add(new MakerModule("X01", new string[] { }));
        modules.Add(new MakerModule("Yahtzee", new string[] { }));
        modules.Add(new MakerModule("Zoni", new string[] { }));
        modules.Add(new MakerModule("Zoo", new string[] { }));
    }

    private Sprite FindIcon(string name)
    {
        return preloadedIcons.First(icon => icon.name == name);
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
