using System.Linq;
using UnityEngine;

namespace DrinkingGame
{
    public class Main : IMod, IModSettings
    {
        public static DrinkingGame instance;
        public static Configruation configuration;

        public string Name => "Drinking Game";

        public string Description => "Tracks Favorite Guests and Alerts how many drinks";

        public string Identifier => "DrinkingGame_ATW";

        public string Path
        {
            get { return ModManager.Instance.getModEntries().First(x => x.mod == this).path; }
        }

        public void onEnabled()
        {
            if (configuration == null)
            {
                configuration = new Configruation();
                configuration.Load();
            }

            var go = new GameObject(Name);
            instance = go.AddComponent<DrinkingGame>();
        }

        public void onDisabled()
        {
            if (instance != null)
            {
                Object.Destroy(instance.gameObject);
                instance = null;
            }
        }

        public void onDrawSettingsUI()
        {
            configuration.DrawGUI();
        }

        public void onSettingsClosed()
        {
            configuration.Save();
        }

        public void onSettingsOpened()
        {
            if (configuration == null)
                configuration = new Configruation();
            configuration.Load();
        }
    }
}
