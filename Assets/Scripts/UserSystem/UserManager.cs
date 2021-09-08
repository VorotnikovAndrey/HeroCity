using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Content;
using Economies;
using Events;
using Gameplay;
using Gameplay.Locations.Models;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

namespace UserSystem
{
    public class UserManager : EventBehavior
    {
        public UserModel CurrentUser { get; private set; }

        public UserManager()
        {
            Load();

            EventAggregator.Add<ResourceModifiedEvent>(x => Save());
        }

        public void Load()
        {
            if (File.Exists(SaveUtils.UserModelPath))
            {
                var serializedData = File.ReadAllText(SaveUtils.UserModelPath);
                JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                CurrentUser = JsonConvert.DeserializeObject<UserModel>(serializedData, settings);
                Debug.Log($"User loaded {SaveUtils.UserModelPath.AddColorTag(Color.yellow)}".AddColorTag(Color.green));
            }
            else
            {
                CreateUser();
                Save();
            }
        }

        public void Save()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(SaveUtils.UserModelPath, JsonConvert.SerializeObject(CurrentUser, settings));
            Debug.Log($"User saved {SaveUtils.UserModelPath.AddColorTag(Color.yellow)}".AddColorTag(Color.green));
        }

        public void CreateUser()
        {
            var defaultResources = new Dictionary<ResourceType, int>();

            foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            {
                defaultResources[type] = GetValueFromResourceEconomy(type);
            }

            CurrentUser = new UserModel(defaultResources);
            CurrentUser.SetTime(new TimeSpan(0, 12, 0, 0));
            CurrentUser.SetLocation(ContentProvider.LocationsEconomy.Data.First().Id);

            var locationModel = new LocationModel
            {
                LocationId = CurrentUser.LocationId
            };

            CurrentUser.Locations.Add(locationModel.LocationId, locationModel);

            Debug.Log($"User created {SaveUtils.UserModelPath.AddColorTag(Color.yellow)}".AddColorTag(Color.green));
        }

        private int GetValueFromResourceEconomy(ResourceType type)
        {
            ResourcesEconomy economy = ContentProvider.ResourcesEconomy;
            ResourcesData data = economy.Data.FirstOrDefault(x => x.Type == type);
            if (data != null)
            {
                return data.Value;
            }

            Debug.LogError($"Default value for {type.AddColorTag(Color.yellow)} is not found!".AddColorTag(Color.red));
            return 0;
        }
    }
}