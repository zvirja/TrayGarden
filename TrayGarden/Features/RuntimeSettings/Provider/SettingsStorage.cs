using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using TrayGarden.Configuration;
using TrayGarden.Helpers;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Features.RuntimeSettings.Provider
{
    public class SettingsStorage : ISettingsStorage
    {
        protected IContainer ResolvedRootContainer { get; set; }

        public virtual string FileName { get; set; }

        public virtual bool UseLocalFolder { get; set; }


        public SettingsStorage()
        {
            FileName = "runtimeSettings.xml";
            UseLocalFolder = true;
        }


        public virtual IContainer GetRootContainer()
        {
            return ResolvedRootContainer;
        }

        public virtual void LoadSettings()
        {
            string storageFilePath = GetFilePath();
            Bucket rootBucket = DeserializeStorageFile(storageFilePath) ?? new Bucket();
            var rootContainer = BuildContainerFromBucket(rootBucket);
            ResolvedRootContainer = rootContainer;
        }


        public virtual bool SaveSettings()
        {
            Bucket rootBucket = BuildBucketFromContainer(ResolvedRootContainer);
            if (rootBucket == null)
                return false;
            bool result = SerializeToStorageFile(rootBucket);
            return result;
        }

        protected virtual string GetFilePath()
        {
            string folderName = null;
            if (!UseLocalFolder)
            {
                folderName = Settings.ApplicationDataFolderName;
                if (!folderName.IsNullOrEmpty())
                {
                    folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                              folderName);
                }
            }
            if (folderName.IsNullOrEmpty())
                folderName = Directory.GetCurrentDirectory();
            return Path.Combine(folderName, FileName);
        }

        protected virtual IContainer BuildContainerFromBucket(Bucket rootBucket)
        {
            Dictionary<string, string> settings = rootBucket.Settings.ToDictionary(settingPair => settingPair.Key,
                                                                                   settingPair => settingPair.Value);
            var subcontainers = rootBucket.InnerBuckets.Select(BuildContainerFromBucket).ToList();
            var newContainer = HatcherGuide<IContainer>.CreateNewInstance();
            newContainer.InitializeFromCollections(rootBucket.Name, settings, subcontainers);
            return newContainer;
        }

        protected virtual Bucket BuildBucketFromContainer(IContainer rootContainer)
        {
            if (rootContainer == null)
                return null;
            var bucket = new Bucket();
            bucket.Name = rootContainer.Name;
            bucket.Settings =
                rootContainer.GetPresentStringSettingNames()
                             .Select(x => new StringStringPair(x, rootContainer.GetStringSetting(x)))
                             .ToList();
            bucket.InnerBuckets =
                rootContainer.GetPresentSubContainerNames()
                             .Select(x => BuildBucketFromContainer(rootContainer.GetNamedSubContainer(x)))
                             .ToList();
            return bucket;
        }

        protected virtual Bucket DeserializeStorageFile(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                    return new Bucket();
                using (var streamReader = new StreamReader(fileName))
                {
                    XmlSerializer serializer = GetBucketXmlSerializer();
                    Bucket resultObject = serializer.Deserialize(streamReader) as Bucket;
                    return resultObject;
                }
            }
            catch
            {
                File.Delete(fileName);
                return null;
            }
        }


        protected virtual bool SerializeToStorageFile(Bucket bucket)
        {
            string filePath = GetFilePath();
            try
            {
                var parentDirectory = Directory.GetParent(filePath);
                if (!parentDirectory.Exists)
                    Directory.CreateDirectory(parentDirectory.FullName);
                using (var streamWriter = new StreamWriter(filePath, false))
                {
                    XmlSerializer serializer = GetBucketXmlSerializer();
                    serializer.Serialize(streamWriter, bucket);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected virtual XmlSerializer GetBucketXmlSerializer()
        {
            return new XmlSerializer(typeof (Bucket));
        }
    }
}