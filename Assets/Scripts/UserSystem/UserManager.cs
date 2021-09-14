using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Content;
using Economies;
using Events;
using Gameplay;
using Gameplay.Building;
using Gameplay.Building.Models;
using Gameplay.Locations.Models;
using Newtonsoft.Json;
using ResourceSystem;
using Source;
using UnityEngine;
using Utils;
using Zenject;

namespace UserSystem
{
    public class UserManager : EventBehavior
    {
        private const int AutoSaveInterval = 5;
        private int _autoSaveSecondCount;

        public UserModel CurrentUser { get; private set; }

        public UserManager()
        {
            Load();

            EventAggregator.Add<ResourceModifiedEvent>(x => Save(true));

            TimeTicker timeTicker = ProjectContext.Instance.Container.Resolve<TimeTicker>();
            timeTicker.OnSecondTick += AutoSave;
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

        public void Save(bool force = false)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(SaveUtils.UserModelPath, JsonConvert.SerializeObject(CurrentUser, settings));
            _autoSaveSecondCount = 0;

            if (force)
            {
                return;
            }

            Debug.Log($"User saved {SaveUtils.UserModelPath.AddColorTag(Color.yellow)}".AddColorTag(Color.green));
        }

        public void CreateUser()
        {
            var defaultResources = new Dictionary<ResourceType, int>();

            foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            {
                defaultResources[type] = GetValueFromResourceEconomy(type);
            }

            CurrentUser = new UserModel(defaultResources)
            {
                Time = new TimeSpan(0, 12, 0, 0),
                CurrentLocationId = ContentProvider.Economies.LocationsEconomy.Data.First().Id,
                Locations = new Dictionary<string, LocationModel>(),
                Improvement = new List<string>()
            };

            var locationModel = new LocationModel
            {
                LocationId = CurrentUser.CurrentLocationId,
                Buildings = new Dictionary<string, BuildingModel>()
            };

            CurrentUser.Locations.Add(locationModel.LocationId, locationModel);

            foreach (var building in ContentProvider.Economies.BuildingsEconomy.Data)
            {
                CurrentUser.CurrentLocation.Buildings.Add(building.Id, new BuildingModel
                {
                    Id = building.Id,
                    Type = building.Type,
                    Stage = new EventVariable<int>(),
                    State = new EventVariable<BuildingState>(building.State)
                });
            }

            Debug.Log($"User created {SaveUtils.UserModelPath.AddColorTag(Color.yellow)}".AddColorTag(Color.green));
        }

        private int GetValueFromResourceEconomy(ResourceType type)
        {
            ResourcesEconomy economy = ContentProvider.Economies.ResourcesEconomy;
            ResourcesData data = economy.Data.FirstOrDefault(x => x.Type == type);
            if (data != null)
            {
                return data.Value;
            }

            Debug.LogError($"Default value for {type.AddColorTag(Color.yellow)} is not found!".AddColorTag(Color.red));
            return 0;
        }

        private void AutoSave()
        {
            _autoSaveSecondCount++;

            if (_autoSaveSecondCount != AutoSaveInterval)
            {
                return;
            }

            Save(true);
        }
    }
}