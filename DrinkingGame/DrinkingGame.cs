using Parkitect.UI;
using System;
using System.Linq;
using UnityEngine;

namespace DrinkingGame
{
    public class DrinkingGame : MonoBehaviour
    {
        private bool _drawGUI = true;

        private event EventHandler _syncHandle;

        private readonly DrinkingSettings _settings;

        public DrinkingGame()
        {
            _settings = Main.configuration.Settings;
        }

        #region MonoBehaviour lifecycle events

        private void Start()
        {
            var em = EventManager.Instance;

            em.OnGuestEnteredAttraction += GuestEnteredAttractionHandler;
            em.OnGuestEntertained += GuestEntertainedHandler;
            em.OnGuestLeftPark += GuestLeftParkHandler;
            em.OnPersonDied += PersonDiedHandler;
            em.OnBalloonPopped += BalloonPoppedHandler;
            em.OnToiletUsed += ToiletUsedHandler;
            em.OnNewThought += NewThoughtHandler;
            em.OnBalloonKnockedAway += BalloonKnockedAwayHandler;
        }


        private void Update()
        {
            var handle = _syncHandle;

            if (handle != null)
            {
                handle(this, new EventArgs());
                _syncHandle = null;
            }

            if (Input.GetKeyDown(KeyCode.T)) _drawGUI = !_drawGUI;
        }

        private void OnDestroy()
        {
            var em = EventManager.Instance;

            em.OnGuestEnteredAttraction -= GuestEnteredAttractionHandler;
            em.OnGuestEntertained -= GuestEntertainedHandler;
            em.OnGuestLeftPark -= GuestLeftParkHandler;
            em.OnPersonDied -= PersonDiedHandler;
            em.OnBalloonPopped -= BalloonPoppedHandler;
            em.OnToiletUsed -= ToiletUsedHandler;
            em.OnNewThought -= NewThoughtHandler;
            em.OnBalloonKnockedAway -= BalloonKnockedAwayHandler;
        }

        #endregion

        #region Event Handlers

        private void GuestEnteredAttractionHandler(Guest guest, Attraction attraction)
        {
            if (_settings.GuestEnteredAttraction?.Enabled == true && guest.isFavorite)
            {
                AddNotification(GetName(guest), $"Entered {attraction.getCustomizedName()}", _settings.GuestEnteredAttraction.MaxDrinks);
            }
        }

        private void GuestEntertainedHandler(Guest entertained)
        {
            if (_settings.GuestEntertained?.Enabled == true && entertained.isFavorite)
            {
                AddNotification(GetName(entertained), $"Entertained lol", _settings.GuestEntertained.MaxDrinks);
            }
        }

        private void GuestLeftParkHandler(Guest guest)
        {
            if (_settings.GuestLeftPark?.Enabled == true && guest.isFavorite)
            {
                AddNotification(GetName(guest), $"LEFT THE PARK!!!", _settings.GuestLeftPark.MaxDrinks);
            }
        }

        private void PersonDiedHandler(Person person, DeathReason reason)
        {
            if (_settings.PersonDied?.Enabled == true)
            {
                if (person.isFavorite)
                {
                    AddNotification(GetName(person), $"Died by {(reason == DeathReason.DROWNED ? "drowning" : "coaster crash")}!!", _settings.PersonDied.MaxDrinks);
                }
                else
                {
                    AddNotification("Guest Died", null, 25);
                }
            }
        }

        private void BalloonPoppedHandler(Balloon balloon)
        {
            if (_settings.BalloonPopped?.Enabled == true)
            {
                AddNotification($"Balloon Popped", null, 3);
            }
        }

        private void ToiletUsedHandler(Person person)
        {
            if (_settings.ToiletUsed?.Enabled == true && person.isFavorite)
            {
                AddNotification(GetName(person), $"Used the Toilet", _settings.ToiletUsed.MaxDrinks);
            }
        }

        private void NewThoughtHandler(Person person)
        {
            if (_settings.NewThought.Enabled == true && person.isFavorite)
            {
                var lastThought = person.thoughts.OrderByDescending(x => x.timeStamp).FirstOrDefault();
                AddNotification(GetName(person), $"New Thought: {lastThought.text}", _settings.NewThought.MaxDrinks);
            }
        }

        private void BalloonKnockedAwayHandler(Balloon balloon, Person person)
        {
            if (_settings.BalloonKnockedAway.Enabled == true && person.isFavorite)
            {
                AddNotification(GetName(person), $"Lost a Balloon...", _settings.BalloonKnockedAway.MaxDrinks);
            }
        }

        #endregion

        private void AddNotification(string header, string text = null, int maxDrinks = 0)
        {
            if (string.IsNullOrWhiteSpace(header))
                return;
            var drinkTxt = GetDrinkString(maxDrinks);

            if (drinkTxt != null)
                header = $"{header} {drinkTxt}";

            Notification notification = text != null
                                    ? new Notification(header, text, Notification.Type.DEFAULT)
                                    : new Notification(header, Notification.Type.DEFAULT);

            NotificationBar.Instance.addNotification(notification);
        }

        private string GetDrinkString(int maxDrinks)
        {
            if (maxDrinks > 0)
            {
                var rand = new System.Random();
                var i = rand.Next(1, maxDrinks);

                return $"{i} DRINKS";
            }
            return null;
        }

        private string GetName(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.nickname) == false)
                return person.nickname;
            return person.getName();
        }
    }
}
