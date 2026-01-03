using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using UnityEngine; // For PlayerPrefs and JsonUtility
using Microsoft.CSharp; // For dynamic compilation
namespace HorrorEngine
{
    public static class CGameEventManager
    {
        private static readonly Dictionary<string, CGameEvent> eventDictionary = new Dictionary<string, CGameEvent>();
        private static readonly Dictionary<string, object> genericEventDictionary = new Dictionary<string, object>();

        public static void Subscribe(string eventName, Action listener)
        {
            if (!eventDictionary.TryGetValue(eventName, out var gameEvent))
            {
                gameEvent = new CGameEvent();
                eventDictionary[eventName] = gameEvent;
            }
            gameEvent.Subscribe(listener);
        }

        public static void Unsubscribe(string eventName, Action listener)
        {
            if (eventDictionary.TryGetValue(eventName, out var gameEvent))
            {
                gameEvent.Unsubscribe(listener);
            }
        }

        public static void Publish(string eventName)
        {
            if (eventDictionary.TryGetValue(eventName, out var gameEvent))
            {
                gameEvent.Publish();
            }
        }

        public static void Subscribe<T>(string eventName, Action<T> listener)
        {
            if (!genericEventDictionary.TryGetValue(eventName, out var obj))
            {
                var gameEvent = new CGameEvent<T>();
                genericEventDictionary[eventName] = gameEvent;
                gameEvent.Subscribe(listener);
            }
            else if (obj is CGameEvent<T> gameEvent)
            {
                gameEvent.Subscribe(listener);
            }
        }

        public static void Unsubscribe<T>(string eventName, Action<T> listener)
        {
            if (genericEventDictionary.TryGetValue(eventName, out var obj) && obj is CGameEvent<T> gameEvent)
            {
                gameEvent.Unsubscribe(listener);
            }
        }

        public static void Publish<T>(string eventName, T eventData)
        {
            if (genericEventDictionary.TryGetValue(eventName, out var obj) && obj is CGameEvent<T> gameEvent)
            {
                gameEvent.Publish(eventData);
            }
        }

        /// <summary>
        /// Registers all static events defined in CGameEvents into the eventDictionary or genericEventDictionary.
        /// </summary>
        public static void RegisterStaticEvents()
        {
            var fields = typeof(CGameEvents).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var eventName = field.Name;
                var eventValue = field.GetValue(null);

                if (eventValue is CGameEvent gameEvent)
                {
                    if (!eventDictionary.ContainsKey(eventName))
                    {
                        eventDictionary[eventName] = gameEvent;
                    }
                }
                else if (eventValue != null && eventValue.GetType().IsGenericType &&
                         eventValue.GetType().GetGenericTypeDefinition() == typeof(CGameEvent<>))
                {
                    if (!genericEventDictionary.ContainsKey(eventName))
                    {
                        genericEventDictionary[eventName] = eventValue;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the current state of the event dictionaries to a JSON file.
        /// </summary>
        public static void SaveEvents(string filePath)
        {
            var eventData = new
            {
                Events = eventDictionary.Keys,
                GenericEvents = genericEventDictionary.Keys
            };

            var json = JsonUtility.ToJson(eventData);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Loads the state of the event dictionaries from a JSON file.
        /// </summary>
        public static void LoadEvents(string filePath)
        {
            if (!File.Exists(filePath)) return;

            var json = File.ReadAllText(filePath);
            var eventData = JsonUtility.FromJson<EventData>(json);

            foreach (var eventName in eventData.Events)
            {
                if (!eventDictionary.ContainsKey(eventName))
                {
                    eventDictionary[eventName] = new CGameEvent();
                }
            }

            foreach (var eventName in eventData.GenericEvents)
            {
                if (!genericEventDictionary.ContainsKey(eventName))
                {
                    var eventType = typeof(CGameEvent<>).MakeGenericType(typeof(object));
                    genericEventDictionary[eventName] = Activator.CreateInstance(eventType);
                }
            }
        }

        /// <summary>
        /// Purges all saved and in-memory event data.
        /// </summary>
        public static void PurgeEvents(string filePath)
        {
            // Clear in-memory dictionaries
            eventDictionary.Clear();
            genericEventDictionary.Clear();

            // Delete the persisted file if it exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        /// <summary>
        /// Saves the current state of the event dictionaries to PlayerPrefs.
        /// </summary>
        public static void SaveEventsToPlayerPrefs()
        {
            var eventData = new
            {
                Events = eventDictionary.Keys,
                GenericEvents = genericEventDictionary.Keys
            };

            var json = JsonUtility.ToJson(eventData);
            PlayerPrefs.SetString("SavedEvents", json);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Loads the state of the event dictionaries from PlayerPrefs.
        /// </summary>
        public static void LoadEventsFromPlayerPrefs()
        {
            if (!PlayerPrefs.HasKey("SavedEvents")) return;

            var json = PlayerPrefs.GetString("SavedEvents");
            var eventData = JsonUtility.FromJson<EventData>(json);

            foreach (var eventName in eventData.Events)
            {
                if (!eventDictionary.ContainsKey(eventName))
                {
                    eventDictionary[eventName] = new CGameEvent();
                }
            }

            foreach (var eventName in eventData.GenericEvents)
            {
                if (!genericEventDictionary.ContainsKey(eventName))
                {
                    var eventType = typeof(CGameEvent<>).MakeGenericType(typeof(object));
                    genericEventDictionary[eventName] = Activator.CreateInstance(eventType);
                }
            }
        }

        /// <summary>
        /// Purges all saved and in-memory event data from PlayerPrefs.
        /// </summary>
        public static void PurgeEventsFromPlayerPrefs()
        {
            // Clear in-memory dictionaries
            eventDictionary.Clear();
            genericEventDictionary.Clear();

            // Remove the persisted data from PlayerPrefs
            if (PlayerPrefs.HasKey("SavedEvents"))
            {
                PlayerPrefs.DeleteKey("SavedEvents");
                PlayerPrefs.Save();
            }
        }
    }

    /// <summary>
    /// Strongly-typed class for event data serialization/deserialization.
    /// </summary>
    [Serializable]
    public class EventData
    {
        public List<string> Events = new List<string>();
        public List<string> GenericEvents = new List<string>();
    }
}
