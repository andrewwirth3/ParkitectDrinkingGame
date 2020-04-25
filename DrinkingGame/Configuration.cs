using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace DrinkingGame
{
    public class Configruation
    {
        private readonly string path;

        public Configruation()
        {
            path = FilePaths.getFolderPath("atw_drinkinggame.config");

            Settings = new DrinkingSettings();
            Debug.Log(path);
        }

        public DrinkingSettings Settings { get; set; }
        public void Load()
        {
            try
            {
                if (File.Exists(path))
                {
                    string jsonString;
                    using (var reader = new StreamReader(path))
                    {
                        jsonString = reader.ReadToEnd();
                        reader.Close();
                    }
                    Settings = JsonConvert.DeserializeObject<DrinkingSettings>(jsonString);
                }
            }
            catch (Exception e)
            {
                Debug.Log("Couldn't properly load settings file! " + e);
                if (e.InnerException != null)
                    Debug.Log("Inner Exception: " + e.InnerException);
            }
        }

        public void Save()
        {
            var context = new SerializationContext(SerializationContext.Context.Savegame);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                using (var writer = new StreamWriter(stream))
                {
                    var json = JsonConvert.SerializeObject(Settings);
                    writer.WriteLine(json);
                }
            }
        }

        public void DrawGUI()
        {
            GUILayout.Label("Andy Testing");

            GUILayout.BeginVertical();
            CreatePropGroup(Settings.GuestEnteredAttraction, "Guest Entered Attraction");
            CreatePropGroup(Settings.GuestEntertained, "Guest Entertained");
            CreatePropGroup(Settings.GuestLeftPark, "Guest Left Park");
            CreatePropGroup(Settings.PersonDied, "Person Died");
            CreatePropGroup(Settings.BalloonPopped, "Balloon Popped");
            CreatePropGroup(Settings.ToiletUsed, "Toilet Used");
            CreatePropGroup(Settings.NewThought, "New Thought");
            CreatePropGroup(Settings.BalloonKnockedAway, "Balloon Knocked Away");
            GUILayout.EndVertical();
        }

        private void CreatePropGroup(PropSettings prop, string title)
        {
            GUILayout.BeginHorizontal();
            prop.Enabled = GUILayout.Toggle(prop.Enabled, title);
            prop.Drinks = GUILayout.TextField(prop.Drinks);
            GUILayout.EndHorizontal();
            GUILayout.Space(20);
        }

    }
}
