using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MeetYourMaker : MonoBehaviour {
   static int ModuleIdCounter = 1;
   private KMNeedyModule Needy;
   int ModuleId;
   private bool needyActive;
   private static JsonReader jsonData;
   private List<string> allCreators;
   private List<MakerModule> modules;
   private string correctAnswer;
   private GameObject loadingGameObject;
    private GameObject warningGameObject;
   #region Buttons/Icons
   [SerializeField]
   private Sprite[] preloadedIcons;
   [SerializeField]
   private Sprite blankIcon;
   private SpriteRenderer icon;
   private Button[] buttons;
    private JsonReader jsonReader;
   #endregion
   void Awake () { //Avoid doing calculations in here regarding edgework. Just use this for setting up buttons for simplicity.
       ModuleId = ModuleIdCounter++;
       icon = transform.Find("Icon").GetComponent<SpriteRenderer>();
       Needy = GetComponent<KMNeedyModule>();
       warningGameObject = transform.Find("Warning").gameObject;
       warningGameObject.SetActive(false);
       loadingGameObject = transform.Find("Loading Text").gameObject;
       Needy.OnNeedyActivation += OnNeedyActivation;
       Needy.OnNeedyDeactivation += OnNeedyDeactivation;
       Needy.OnTimerExpired += OnTimerExpired;
        jsonReader = GetComponent<JsonReader>();
        buttons = new Button[4];
       for (int i = 1; i < 5; i++)
       {
           int dummy = i;
           KMSelectable selectable = transform.Find($"Button {dummy}").GetComponent<KMSelectable>();
            Text text = transform.Find($"Button {dummy}/Canvas/Text").GetComponent<Text>();
           buttons[dummy - 1] = new Button(selectable, text);
           selectable.OnInteract += delegate () { ButtonPress(buttons[dummy - 1]); return false; };
       }
   }
   protected void OnNeedyActivation () { //Shit that happens when a needy turns on.
        GenrateQuestion();
        needyActive = true;
   }
   protected void OnNeedyDeactivation() { //Shit that happens when a needy turns off.
        needyActive = false;
        ResetModule();
        Needy.OnPass();
   }
   private void ResetModule()
   {
       icon.sprite = blankIcon;
       foreach (Button button in buttons)
       {
            button.Text.text = "";
       }
   }
   private void Logging(string message)
   {
       Debug.Log($"[Meet Your Maker #{ModuleId}] {message}");
   }
   protected void OnTimerExpired () { //Shit that happens when a needy turns off due to running out of time.
        Strike("Timer ran out");
        ResetModule();
   }

   IEnumerator Start () { //Shit that you calculate, usually a majority if not all of the module
        ResetModule();
        modules = new List<MakerModule>();
        //if data is not done loaded
        if (!JsonReader.LoadingDone)
        {
            //if not already loading, load
            if (!JsonReader.Loading)
            {
                yield return jsonReader.LoadData(ModuleId);
            }

            //if aleady loading, wait until loading is done
            else
            {
                do
                {
                    yield return new WaitForSeconds(0.1f);

                } while (!JsonReader.LoadingDone);
            }
        }
        //if the json failed to load, use the data from the appendix
        if (!JsonReader.Success)
        {
            Logging("Unable to connect to the repo, using preloaded modules");
            LoadPreloadedModules();
            warningGameObject.SetActive(true);
        }
        else
        {
            modules = JsonReader.LoadedModules;
            Logging($"Loaded {modules.Count} modules");
            Logging(string.Join(", ", modules.Select(m => m.ModuleName).ToArray()));
        }

        loadingGameObject.gameObject.SetActive(false);
        allCreators = new List<string>();
        foreach (MakerModule module in modules)
        {
            allCreators.AddRange(module.Creators);
            allCreators = allCreators.Distinct().ToList();
        }

        //longest name: thestorebrandslimshady
    }
    private void ButtonPress(Button button)
    {
        button.Selectable.AddInteractionPunch(1f);
        if (!needyActive) return;
        if (button.Text.text != correctAnswer)
        {
            Strike($"You pressed {button.Text.text}.");
        }
        OnNeedyDeactivation();
    }
   void Strike (string message) 
   {
      Logging($"Strike! {message}");
      GetComponent<KMNeedyModule>().HandleStrike();
   }
    private void GenrateQuestion()
    {
        allCreators = RandomizeList(allCreators);
        MakerModule selectedModule = modules.PickRandom();
        icon.sprite = selectedModule.Icon;
        correctAnswer = selectedModule.Creators.PickRandom();
        List<string> answers = new List<string>();
        answers.Add(correctAnswer);
        answers.AddRange(allCreators.Where(creator => !selectedModule.Creators.Contains(creator)).Take(3));
        answers = RandomizeList(answers);

        for(int i = 0; i < 4; i++)
        {
            buttons[i].Text.text = answers[i];
        }

        Logging($"Module is {selectedModule.ModuleName}. Correct answer is {correctAnswer}");
    }
    private List<string> RandomizeList(List<string> orgininal)
    {
        List<string> newList = new List<string>();
        List<string> oldList = new List<string>(orgininal);
        while (oldList.Count > 0) 
        {
            string result = oldList.PickRandom();
            oldList.Remove(result);
            newList.Add(result);
        }
        return newList;
    }
    private void LoadPreloadedModules()
    {
        MakerModule.PreloadedIcons = preloadedIcons;
        modules.Add(new MakerModule("3D Maze", new string[] { "Spare Wizard" }));
        modules.Add(new MakerModule("3D Tunnels", new string[] { "Groover", "Timwi" }));
        modules.Add(new MakerModule("101 Dalmatians", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Accumulation", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Adjacent Letters", new string[] { "Timwi", "Lumbud84" }));
        modules.Add(new MakerModule("Adventure Game", new string[] { "Spare Wizard" }));
        modules.Add(new MakerModule("Algebra", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Alphabet", new string[] { "mitterdoo" }));
        modules.Add(new MakerModule("Alphabet Numbers", new string[] { "Royal_Flu$h", "WhiteShadowZz" }));
        modules.Add(new MakerModule("Anagrams", new string[] { "Mock Army" }));
        modules.Add(new MakerModule("Arithmelogic", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Astrology", new string[] { "Spare Wizard" }));
        modules.Add(new MakerModule("Backgrounds", new string[] { "McNiko67" }));
        modules.Add(new MakerModule("Bartending", new string[] { "RockDood" }));
        modules.Add(new MakerModule("Bases", new string[] { "AAces" }));
        modules.Add(new MakerModule("Battleship", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Benedict Cumberbatch", new string[] { "Royal_Flu$h","Arlas" }));
        modules.Add(new MakerModule("Big Circle", new string[] { "CaitSith2", "Quinn Wuest", "Hendruid" }));
        modules.Add(new MakerModule("Binary LEDs", new string[] { "Willowyn" }));
        modules.Add(new MakerModule("Binary Puzzle", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Binary Tree", new string[] { "billy_bao" }));
        modules.Add(new MakerModule("Bitmaps", new string[] { "Timwi", "Lumbud84" }));
        modules.Add(new MakerModule("Bitwise Operations", new string[] { "Virepri" }));
        modules.Add(new MakerModule("Black Hole", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Blackjack", new string[] { "Kritzy" }));
        modules.Add(new MakerModule("Blind Alley", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Blind Maze", new string[] { "McNiko67", "Riverbui" }));
        modules.Add(new MakerModule("Blockbusters", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Boggle", new string[] { "AAces" }));
        modules.Add(new MakerModule("Boolean Maze", new string[] { "Theta" }));
        modules.Add(new MakerModule("Boolean Venn Diagram", new string[] { "ZekNikZ" }));
        modules.Add(new MakerModule("Braille", new string[] { "Timwi" }));
        modules.Add(new MakerModule("British Slang", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Broken Buttons", new string[] { "samfundev" }));
        modules.Add(new MakerModule("Broken Guitar Chords", new string[] { "Timwi", "Royal_Flu$h" }));
        modules.Add(new MakerModule("Brush Strokes", new string[] { "EpicToast", "TasThiluna", "Blananas2" }));
        modules.Add(new MakerModule("Burger Alarm", new string[] { "EpicToast" }));
        modules.Add(new MakerModule("Burglar Alarm", new string[] { "Marksam" }));
        modules.Add(new MakerModule("Button Grid", new string[] { "EpicToast", "Quinn Wuest", "Asew54321" }));
        modules.Add(new MakerModule("Button Sequence", new string[] { "ZekNikZ" }));
        modules.Add(new MakerModule("Caesar Cipher", new string[] { "Eluminate" }));
        modules.Add(new MakerModule("Calendar", new string[] { "AAces", "MarioXTurn" }));
        modules.Add(new MakerModule("Catchphrase", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Challenge & Contact", new string[] { "EpicToast" }));
        modules.Add(new MakerModule("Character Shift", new string[] { "AAces", "Vantastic" }));
        modules.Add(new MakerModule("Cheap Checkout", new string[] { "samfundev" }));
        modules.Add(new MakerModule("Christmas Presents", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Coffeebucks", new string[] { "Royal_Flu$h", "Elias" }));
        modules.Add(new MakerModule("Color Decoding", new string[] { "Windesign" }));
        modules.Add(new MakerModule("Color Generator", new string[] { "EternityShack" }));
        modules.Add(new MakerModule("Color Math", new string[] { "SL7205" }));
        modules.Add(new MakerModule("Color Morse", new string[] { "ZekNikZ", "Quinn Wuest", "Timwi", "SKiPP" }));
        modules.Add(new MakerModule("Colored Keys", new string[] { "LeGeND" }));
        modules.Add(new MakerModule("Colored Squares", new string[] { "Timwi", "TheAuthorOfOZ" }));
        modules.Add(new MakerModule("Colored Switches", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Colorful Insanity", new string[] { "KingSlendy" }));
        modules.Add(new MakerModule("Colorful Madness", new string[] { "KingSlendy" }));
        modules.Add(new MakerModule("Colour Code", new string[] { "MrMelon" }));
        modules.Add(new MakerModule("Colour Flash", new string[] { "Bashly" }));
        modules.Add(new MakerModule("Complex Keypad", new string[] { "AAces" }));
        modules.Add(new MakerModule("Complicated Buttons", new string[] { "ZekNikZ" }));
        modules.Add(new MakerModule("Connection Check", new string[] { "clutterArranger" }));
        modules.Add(new MakerModule("Connection Device", new string[] { "Kritzy" }));
        modules.Add(new MakerModule("Cookie Jars", new string[] { "EpicToast" }));
        modules.Add(new MakerModule("Cooking", new string[] { "Marksam", "Brawlbox" }));
        modules.Add(new MakerModule("Coordinates", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Countdown", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Crackbox", new string[] { "Marksam" }));
        modules.Add(new MakerModule("Crazy Talk", new string[] { "Perky" }));
        modules.Add(new MakerModule("Creation", new string[] { "samfundev", "Grybo" }));
        modules.Add(new MakerModule("Cryptography", new string[] { "Perky" }));
        modules.Add(new MakerModule("Curriculum", new string[] { "Fixdoll" }));
        modules.Add(new MakerModule("Decolored Squares", new string[] { "Timwi" }));
        modules.Add(new MakerModule("DetoNATO", new string[] { "DVD", "Kusane Hexaku" }));
        modules.Add(new MakerModule("Digit String", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Digital Cipher", new string[] { "Goofy" }));
        modules.Add(new MakerModule("Digital Root", new string[] { "red031000", "CyanLights" }));
        modules.Add(new MakerModule("Discolored Squares", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Divided Squares", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Dominoes", new string[] { "Gee", "Blananas2" }));
        modules.Add(new MakerModule("Double Color", new string[] { "Brandon", "AAces" }));
        modules.Add(new MakerModule("Double-Oh", new string[] { "Timwi", "Elias" }));
        modules.Add(new MakerModule("Dr. Doctor", new string[] { "Timwi", "Livio", "DanielL" }));
        modules.Add(new MakerModule("Dragon Energy", new string[] { "Brandon", "AAces", "Timwi" }));
        modules.Add(new MakerModule("Elder Futhark", new string[] { "Goofy" }));
        modules.Add(new MakerModule("Emoji Math", new string[] { "Mock Army" }));
        modules.Add(new MakerModule("Encrypted Morse", new string[] { "BlockWorker" }));
        modules.Add(new MakerModule("English Test", new string[] { "Lupo511", "theFIZZYnator" }));
        modules.Add(new MakerModule("Equations", new string[] { "AAces" }));
        modules.Add(new MakerModule("Equations X", new string[] { "eXish" }));
        modules.Add(new MakerModule("Error Codes", new string[] { "mookieg849" }));
        modules.Add(new MakerModule("European Travel", new string[] { "Royal_Flu$h", "Monopoly" }));
        modules.Add(new MakerModule("Extended Password", new string[] { "taggedjc", "plebeians" }));
        modules.Add(new MakerModule("Factory Maze", new string[] { "EpicToast" }));
        modules.Add(new MakerModule("Fast Math", new string[] { "SL7205", "LeGeND" }));
        modules.Add(new MakerModule("Faulty Sink", new string[] { "Riverbui" }));
        modules.Add(new MakerModule("FizzBuzz", new string[] { "ZekNikZ" }));
        modules.Add(new MakerModule("Flags", new string[] { "Piggered", "Monopoly" }));
        modules.Add(new MakerModule("Flashing Lights", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Flavor Text", new string[] { "DVD" }));
        modules.Add(new MakerModule("Follow the Leader", new string[] { "Timwi", "TheAuthorOfOZ" }));
        modules.Add(new MakerModule("Font Select", new string[] { "McNiko67" }));
        modules.Add(new MakerModule("Foreign Exchange Rates", new string[] { "Perky" }));
        modules.Add(new MakerModule("Forget Everything", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Forget Me Not", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Forget This", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Four-Card Monte", new string[] { "Kritzy" }));
        modules.Add(new MakerModule("Free Parking", new string[] { "Royal_Flu$h", "StKildaFan" }));
        modules.Add(new MakerModule("Friendship", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Functions", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Gadgetron Vendor", new string[] { "LeGeND" }));
        modules.Add(new MakerModule("Game of Life Simple", new string[] { "Eotall", "WarioLGP" }));
        modules.Add(new MakerModule("Genetic Sequence", new string[] { "TheThirdMan" }));
        modules.Add(new MakerModule("Graffiti Numbers", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Graphic Memory", new string[] { "TheThirdMan" }));
        modules.Add(new MakerModule("Greek Calculus", new string[] { "billy_bao" }));
        modules.Add(new MakerModule("Grid Matching", new string[] { "Windesign" }));
        modules.Add(new MakerModule("Gridlock", new string[] { "Timwi", "Elias" }));
        modules.Add(new MakerModule("Grocery Store", new string[] { "TheRealWitch" }));
        modules.Add(new MakerModule("Gryphons", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Guitar Chords", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Harmony Sequence", new string[] { "Goofy" }));
        modules.Add(new MakerModule("Hexamaze", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Hidden Colors", new string[] { "LeGeND" }));
        modules.Add(new MakerModule("Hieroglyphics", new string[] { "Royal_Flu$h", "Elias" }));
        modules.Add(new MakerModule("Hogwarts", new string[] { "Timwi", "Mickeylover", "Brawlbox" }));
        modules.Add(new MakerModule("Homophones", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Horrible Memory", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Human Resources", new string[] { "Timwi", "Elias" }));
        modules.Add(new MakerModule("Hunting", new string[] { "taggedjc" }));
        modules.Add(new MakerModule("Ice Cream", new string[] { "ZekNikZ", "SKiPP", "LeGeND" }));
        modules.Add(new MakerModule("Identity Parade", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("IKEA", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Instructions", new string[] { "EpicToast" }));
        modules.Add(new MakerModule("Know Your Way", new string[] { "Blananas2", "Joostoos" }));
        modules.Add(new MakerModule("Krazy Talk", new string[] { "EpicToast" }));
        modules.Add(new MakerModule("Kudosudoku", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Lasers", new string[] { "Riverbui", "Timwi", "Elias" }));
        modules.Add(new MakerModule("Laundry", new string[] { "Flamanis", "Hendruid", "AcrylicStain" }));
        modules.Add(new MakerModule("LED Encryption", new string[] { "Virepri" }));
        modules.Add(new MakerModule("LED Grid", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("LED Math", new string[] { "LeGeND" }));
        modules.Add(new MakerModule("Left and Right", new string[] { "Goofy" }));
        modules.Add(new MakerModule("LEGOs", new string[] { "ZekNikZ" }));
        modules.Add(new MakerModule("Letter Keys", new string[] { "Mage of R. Jelly" }));
        modules.Add(new MakerModule("Light Cycle", new string[] { "Timwi", "Rexkix" }));
        modules.Add(new MakerModule("Lightspeed", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Lion's Share", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Listening", new string[] { "Perky" }));
        modules.Add(new MakerModule("Logic", new string[] { "SL7205" }));
        modules.Add(new MakerModule("Logic Gates", new string[] { "Groover", "Maca" }));
        modules.Add(new MakerModule("Logical Buttons", new string[] { "Marksam", "SpaceScrew" }));
        modules.Add(new MakerModule("Lombax Cubes", new string[] { "LeGeND" }));
        modules.Add(new MakerModule("Mad Memory", new string[] { "DVD" }));
        modules.Add(new MakerModule("Mafia", new string[] { "Timwi", "MarioXTurn", "Quinn Wuest" }));
        modules.Add(new MakerModule("Mahjong", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Maintenance", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Manometers", new string[] { "ThePhenix33" }));
        modules.Add(new MakerModule("Marble Tumble", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Maritime Flags", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Mashematics", new string[] { "Marksam" }));
        modules.Add(new MakerModule("Mastermind Simple", new string[] { "Eotall" }));
        modules.Add(new MakerModule("Maze Scrambler", new string[] { "McNiko67" }));
        modules.Add(new MakerModule("Mazematics", new string[] { "Skyeward" }));
        modules.Add(new MakerModule("Mega Man 2", new string[] { "Goofy" }));
        modules.Add(new MakerModule("Melody Sequencer", new string[] { "Goofy" }));
        modules.Add(new MakerModule("Micro-Modules", new string[] { "Kritzy" }));
        modules.Add(new MakerModule("Microcontroller", new string[] { "Flush" }));
        modules.Add(new MakerModule("Mineseeker", new string[] { "Riverbui", "Pruz" }));
        modules.Add(new MakerModule("Minesweeper", new string[] { "samfundev" }));
        modules.Add(new MakerModule("Modern Cipher", new string[] { "TheFe" }));
        modules.Add(new MakerModule("Module Homework", new string[] { "Kritzy" }));
        modules.Add(new MakerModule("Module Maze", new string[] { "Riverbui", "Blananas2" }));
        modules.Add(new MakerModule("Modules Against Humanity", new string[] { "Flamanis", "catnip" }));
        modules.Add(new MakerModule("Modulo", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Monsplode Trading Cards", new string[] { "clutterArranger", "Grybo" }));
        modules.Add(new MakerModule("Monsplode, Fight!", new string[] { "lutterArranger" }));
        modules.Add(new MakerModule("Morse Buttons", new string[] { "TheThirdMan" }));
        modules.Add(new MakerModule("Morse War", new string[] { "Brandon", "Flamanis" }));
        modules.Add(new MakerModule("Morse-A-Maze", new string[] { "CaitSith2" }));
        modules.Add(new MakerModule("Morsematics", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Mortal Kombat", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Mouse in the Maze", new string[] { "Konqi" }));
        modules.Add(new MakerModule("Murder", new string[] { "Asimir" }));
        modules.Add(new MakerModule("Mystic Square", new string[] { "Konqi" }));
        modules.Add(new MakerModule("Neutralization", new string[] { "SL7205", "Nanthelas" }));
        modules.Add(new MakerModule("Nonogram", new string[] { "Elias", "KingBranBran" }));
        modules.Add(new MakerModule("Number Nimbleness", new string[] { "Elias", "KingBranBran" }));
        modules.Add(new MakerModule("Number Pad", new string[] { "mitterdoo" }));
        modules.Add(new MakerModule("Numbers", new string[] { "Waluigi" }));
        modules.Add(new MakerModule("Odd One Out", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Only Connect", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Orientation Cube", new string[] { "Perky" }));
        modules.Add(new MakerModule("Painting", new string[] { "Bashly", "Hendruid" }));
        modules.Add(new MakerModule("Party Time", new string[] { "KingSlendy" }));
        modules.Add(new MakerModule("Passport Control", new string[] { "Crystlack" }));
        modules.Add(new MakerModule("Pattern Cube", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Periodic Table", new string[] { "MrSpekCraft" }));
        modules.Add(new MakerModule("Perplexing Wires", new string[] { "Timwi", "Quinn Wuest" }));
        modules.Add(new MakerModule("Perspective Pegs", new string[] { "Spare Wizard" }));
        modules.Add(new MakerModule("Piano Keys", new string[] { "Bashly" }));
        modules.Add(new MakerModule("Pie", new string[] { "KingBranBran" }));
        modules.Add(new MakerModule("Pigpen Rotations", new string[] { "noahcoolboy" }));
        modules.Add(new MakerModule("Planets", new string[] { "MrMelon", "KingSlendy" }));
        modules.Add(new MakerModule("Playfair Cipher", new string[] { "Maca", "Groover" }));
        modules.Add(new MakerModule("Plumbing", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Poetry", new string[] { "clutterArranger" }));
        modules.Add(new MakerModule("Point of Order", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Poker", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Polyhedral Maze", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Press X", new string[] { "TGCJules" }));
        modules.Add(new MakerModule("Probing", new string[] { "Perky" }));
        modules.Add(new MakerModule("Purgatory", new string[] { "red031000", "3D", "cerulean" }));
        modules.Add(new MakerModule("Question Mark", new string[] { "DVD", "Panoptes (Xel)" }));
        modules.Add(new MakerModule("Quintuples", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Quiz Buzz", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Radiator", new string[] { "red031000", "Inova" }));
        modules.Add(new MakerModule("Regular Crazy Talk", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Resistors", new string[] { "Onyxite" }));
        modules.Add(new MakerModule("Retirement", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Reverse Morse", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Rhythms", new string[] { "Trainzack" }));
        modules.Add(new MakerModule("Rock-Paper-Scissors-Lizard-Spock", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Roman Art", new string[] { "eXish" }));
        modules.Add(new MakerModule("Round Keypad", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Rubik's Clock", new string[] { "Groover" }));
        modules.Add(new MakerModule("Rubik's Cube", new string[] { "Timwi", "Freelancer1025" }));
        modules.Add(new MakerModule("S.E.T.", new string[] { "Timwi", "Zawu" }));
        modules.Add(new MakerModule("Safety Safe", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Schlag den Bomb", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Scripting", new string[] { "Kritzy" }));
        modules.Add(new MakerModule("Sea Shells", new string[] { "Asimir" }));
        modules.Add(new MakerModule("Semaphore", new string[] { "Bashly" }));
        modules.Add(new MakerModule("Seven Wires", new string[] { "Atomik", "ryaninator81" }));
        modules.Add(new MakerModule("Shape Shift", new string[] { "Asimir" }));
        modules.Add(new MakerModule("Shapes and Bombs", new string[] { "KingSlendy" }));
        modules.Add(new MakerModule("Shikaku", new string[] { "Groover" }));
        modules.Add(new MakerModule("Signals", new string[] { "mkmk" }));
        modules.Add(new MakerModule("Silly Slots", new string[] { "Perky" }));
        modules.Add(new MakerModule("Simon Samples", new string[] { "Groover" }));
        modules.Add(new MakerModule("Simon Scrambles", new string[] { "noahcoolboy" }));
        modules.Add(new MakerModule("Simon Screams", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Simon Sends", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Simon Shrieks", new string[] { "Timwi", "IFBeetle" }));
        modules.Add(new MakerModule("Simon Sings", new string[] { "Timwi", "MarioXTurn" }));
        modules.Add(new MakerModule("Simon Sounds", new string[] { "Goofy" }));
        modules.Add(new MakerModule("Simon Speaks", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Simon Spins", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Simon States", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Simon Stops", new string[] { "JerryEris" }));
        modules.Add(new MakerModule("Simon's Stages", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Simon's Star", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Sink", new string[] { "McNiko67" }));
        modules.Add(new MakerModule("Skewed Slots", new string[] { "samfundev" }));
        modules.Add(new MakerModule("Skinny Wires", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Skyrim", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Snooker", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Sonic & Knuckles", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Sonic the Hedgehog", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Souvenir", new string[] { "Timwi", "Andrio Celos" }));
        modules.Add(new MakerModule("Spinning Buttons", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Splitting the Loot", new string[] { "Marksam", "Elias" }));
        modules.Add(new MakerModule("Square Button", new string[] { "Hexicube" }));
        modules.Add(new MakerModule("Stack'em", new string[] { "Goofy" }));
        modules.Add(new MakerModule("Street Fighter", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Subscribe to Pewdiepie", new string[] { "EpicToast" }));
        modules.Add(new MakerModule("Subways", new string[] { "AAces" }));
        modules.Add(new MakerModule("Sueet Wall", new string[] { "KingSlendy" }));
        modules.Add(new MakerModule("Superlogic", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Switches", new string[] { "Brian Fetter" }));
        modules.Add(new MakerModule("Symbol Cycle", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Symbolic Coordinates", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Symbolic Password", new string[] { "ZekNikZ" }));
        modules.Add(new MakerModule("SYNC-125 [3]", new string[] { "DVD" }));
        modules.Add(new MakerModule("Synchronization", new string[] { "samfundev", "TheAuthorOfOZ" }));
        modules.Add(new MakerModule("Synonyms", new string[] { "TGCJules", "limbojim" }));
        modules.Add(new MakerModule("T-Words", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Tangrams", new string[] { "Bashly" }));
        modules.Add(new MakerModule("Tap Code", new string[] { "KingBranBran" }));
        modules.Add(new MakerModule("Tax Returns", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Ten-Button Color Code", new string[] { "KingSlendy", "Lumbud84" }));
        modules.Add(new MakerModule("Tennis", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Terraria Quiz", new string[] { "LeGeND", "TasThiluna" }));
        modules.Add(new MakerModule("Text Field", new string[] { "SL7205" }));
        modules.Add(new MakerModule("The Bulb", new string[] { "Timwi" }));
        modules.Add(new MakerModule("The Clock", new string[] { "Timwi", "TheAuthorOfOZ" }));
        modules.Add(new MakerModule("The Code", new string[] { "LeGeND" }));
        modules.Add(new MakerModule("The Crystal Maze", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Cube", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Digit", new string[] { "Marksam", "tandyCake" }));
        modules.Add(new MakerModule("The Festive Jukebox", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Gamepad", new string[] {"samfundev", "theFIZZYnator" }));
        modules.Add(new MakerModule("The Giant's Drink", new string[] { "TheThirdMan" }));
        modules.Add(new MakerModule("The Hangover", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Hexabutton", new string[] { "EpicToast", "ryaninator81" }));
        modules.Add(new MakerModule("The Hypercube", new string[] { "Timwi" }));
        modules.Add(new MakerModule("The iPhone", new string[] {"Royal_Flu$h" }));
        modules.Add(new MakerModule("The Jack-O'-Lantern", new string[] { "Royal_Flu$h", "LeGeND" }));
        modules.Add(new MakerModule("The Jewel Vault", new string[] { "Royal_Flu$h", "MarioXTurn" }));
        modules.Add(new MakerModule("The Jukebox", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Labyrinth", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The London Underground", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Moon", new string[] { "Royal_Flu$h", "Pruz" }));
        modules.Add(new MakerModule("The Necronomicon", new string[] { "TheThirdMan" }));
        modules.Add(new MakerModule("The Number", new string[] { "red031000", "Felix" }));
        modules.Add(new MakerModule("The Number Cipher", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Plunger Button", new string[] {"Royal_Flu$h" }));
        modules.Add(new MakerModule("The Radio", new string[] { "Kritzy" }));
        modules.Add(new MakerModule("The Screw", new string[] { "SL7205", "Quinn Wuest" }));
        modules.Add(new MakerModule("The Sphere", new string[] {"Royal_Flu$h" }));
        modules.Add(new MakerModule("The Stare", new string[] { "DVD" }));
        modules.Add(new MakerModule("The Stock Market", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Stopwatch", new string[] { "Royal_Flu$h", "SpaceScrew" }));
        modules.Add(new MakerModule("The Sun", new string[] { "Royal_Flu$h", "Pruz" }));
        modules.Add(new MakerModule("The Swan", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Switch", new string[] { "McNiko67" }));
        modules.Add(new MakerModule("The Time Keeper", new string[] { "AAces" }));
        modules.Add(new MakerModule("The Triangle", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("The Troll", new string[] {"Royal_Flu$h" }));
        modules.Add(new MakerModule("The Wire", new string[] {"Royal_Flu$h" }));
        modules.Add(new MakerModule("The Witness", new string[] { "VFlyer", "bmo22xd" }));
        modules.Add(new MakerModule("Third Base", new string[] { "Asimir" }));
        modules.Add(new MakerModule("Tic Tac Toe", new string[] { "Timwi", "Moon" }));
        modules.Add(new MakerModule("Timezone", new string[] { "federan" }));
        modules.Add(new MakerModule("Timing is Everything", new string[] { "Blananas2", "TheRealWitch" }));
        modules.Add(new MakerModule("Triangle Buttons", new string[] { "hockeygoalie78", "Pruz" }));
        modules.Add(new MakerModule("Turn the Key", new string[] { "Perky" }));
        modules.Add(new MakerModule("Turn the Keys", new string[] { "Perky" }));
        modules.Add(new MakerModule("Turtle Robot", new string[] { "Groover" }));
        modules.Add(new MakerModule("Two Bits", new string[] { "kaneb" }));
        modules.Add(new MakerModule("Unfair Cipher", new string[] { "Maca" }));
        modules.Add(new MakerModule("Unrelated Anagrams", new string[] { "DVD" }));
        modules.Add(new MakerModule("USA Maze", new string[] { "Riverbui", "Blananas2" }));
        modules.Add(new MakerModule("Valves", new string[] { "KingBranBran" }));
        modules.Add(new MakerModule("Varicolored Squares", new string[] { "ZekNikZ" }));
        modules.Add(new MakerModule("Vexillology", new string[] { "MrSpekCraft" }));
        modules.Add(new MakerModule("Visual Impairment", new string[] { "KingBranBran" }));
        modules.Add(new MakerModule("Waste Management", new string[] { "red031000", "Inova", "AppleSlice" }));
        modules.Add(new MakerModule("Wavetapping", new string[] { "KingSlendy", "Lumbud84" }));
        modules.Add(new MakerModule("Web Design", new string[] { "SL7205", "theFIZZYnator" }));
        modules.Add(new MakerModule("Westeros", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Wire Placement", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Wire Spaghetti", new string[] { "Royal_Flu$h" }));
        modules.Add(new MakerModule("Word Search", new string[] { "Timwi", "TheAuthorOfOZ" }));
        modules.Add(new MakerModule("X-Ray", new string[] { "Timwi" }));
        modules.Add(new MakerModule("X01", new string[] { "SillyPuppy" }));
        modules.Add(new MakerModule("Yahtzee", new string[] { "Timwi" }));
        modules.Add(new MakerModule("Zoni", new string[] { "LeGeND" }));
        modules.Add(new MakerModule("Zoo", new string[] { "Timwi" }));
    }

    public void OnDestory()
    {
        JsonReader.Reset();
    }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} followed by '1,2,3, or 4' to select one of the buttons from top to bottom.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
        Command = Command.ToUpper().Trim();
        yield return null;
        switch (Command)
        {
            case "1":
                buttons[0].Selectable.OnInteract();
                yield break;
            case "2":
                buttons[1].Selectable.OnInteract();
                yield break;
            case "3":
                buttons[2].Selectable.OnInteract();
                yield break;
            case "4":
                buttons[3].Selectable.OnInteract();
                yield break;
            default:
                yield return string.Format("sendtochaterror Invalid command");
                yield break;
        }
   }

   void TwitchHandleForcedSolve () { //Void so that autosolvers go to it first instead of potentially striking due to running out of time.
      StartCoroutine(HandleAutosolver());
   }

   IEnumerator HandleAutosolver () {
        while (true)
        {
            while(!needyActive) yield return null;
            buttons.First(button => button.Text.text == correctAnswer).Selectable.OnInteract();
        }
    }
}
