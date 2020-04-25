using Newtonsoft.Json;
using Parkitect.UI;
using System.Runtime.Serialization;

namespace DrinkingGame
{
    public class DrinkingSettings : SerializedRawObject
    {
        public DrinkingSettings()
        {
            GuestEnteredAttraction = new PropSettings();
            GuestEntertained = new PropSettings();
            GuestLeftPark = new PropSettings();
            PersonDied = new PropSettings();
            BalloonPopped = new PropSettings();
            ToiletUsed = new PropSettings();
            NewThought = new PropSettings();
            BalloonKnockedAway = new PropSettings();
        }
        [Serialized] public PropSettings GuestEnteredAttraction { get; set; }
        [Serialized] public PropSettings GuestEntertained { get; set; }
        [Serialized] public PropSettings GuestLeftPark { get; set; }
        [Serialized] public PropSettings PersonDied { get; set; }
        [Serialized] public PropSettings BalloonPopped { get; set; }
        [Serialized] public PropSettings ToiletUsed { get; set; }
        [Serialized] public PropSettings NewThought { get; set; }
        [Serialized] public PropSettings BalloonKnockedAway { get; set; }
    }

    public class PropSettings
    {
        public PropSettings()
        {
            Enabled = true;
            Drinks = "5";
        }
        [Serialized] public bool Enabled { get; set; }
        [Serialized] public string Drinks { get; set; }

        [IgnoreDataMember]
        public int MaxDrinks {
            get
            {
                if (string.IsNullOrWhiteSpace(Drinks) == false)
                {
                    int.TryParse(Drinks, out int result);
                    return result;
                }
                return 0;
            }
        }
    }
}
