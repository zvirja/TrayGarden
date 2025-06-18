using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.RuntimeSettings.Provider;

[UsedImplicitly]
public class SettingsStorage : ISettingsStorage
{
  public SettingsStorage()
  {
    FileName = "RuntimeSettings.xml";
    UseLocalFolder = true;
    EnableDebuggingTraces = false;
  }

  public virtual bool EnableDebuggingTraces { get; set; }

  public virtual string FileName { get; set; }

  public virtual bool UseLocalFolder { get; set; }

  protected IObjectFactory ContainerFactory { get; set; }

  protected IContainer ResolvedRootContainer { get; set; }

  public virtual IContainer GetRootContainer()
  {
    return ResolvedRootContainer;
  }

  [UsedImplicitly]
  public void Initialize(IObjectFactory containerFactory)
  {
    Assert.ArgumentNotNull(containerFactory, "containerFactory");
    ContainerFactory = containerFactory;
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
    {
      return false;
    }
    bool result = SerializeToStorageFile(rootBucket);
    return result;
  }

  protected virtual Bucket BuildBucketFromContainer(IContainer rootContainer)
  {
    if (rootContainer == null)
    {
      return null;
    }
    var bucket = new Bucket();
    bucket.Name = rootContainer.Name;
    bucket.Settings =
      rootContainer.GetPresentStringSettingNames().Select(x => new StringStringPair(x, rootContainer.GetStringSetting(x))).ToList();
    bucket.InnerBuckets =
      rootContainer.GetPresentSubContainerNames()
        .Select(x => BuildBucketFromContainer(rootContainer.GetNamedSubContainer(x)))
        .ToList();
    return bucket;
  }

  protected virtual IContainer BuildContainerFromBucket(Bucket rootBucket)
  {
    Dictionary<string, string> settings = rootBucket.Settings.ToDictionary(
      settingPair => settingPair.Key,
      settingPair => settingPair.Value);
    var subcontainers = rootBucket.InnerBuckets.Select(BuildContainerFromBucket).ToList();
    var newContainer = ContainerFactory.GetPurelyNewObject() as IContainer;
    Assert.IsNotNull(newContainer, "Wrong container factory");
    newContainer.InitializeFromCollections(rootBucket.Name, settings, subcontainers);
    return newContainer;
  }

  protected virtual Bucket DeserializeStorageFile(string fileName)
  {
    try
    {
      if (!File.Exists(fileName))
      {
        return new Bucket();
      }
      using (var streamReader = new StreamReader(fileName))
      {
        XmlSerializer serializer = GetBucketXmlSerializer();
        Bucket resultObject = serializer.Deserialize(streamReader) as Bucket;
        if (EnableDebuggingTraces)
        {
          TraceDebugStreamContent(streamReader.BaseStream, "ReadOp");
        }
        return resultObject;
      }
    }
    catch (Exception ex)
    {
      File.Delete(fileName);
      Log.Warn("Failed to deserialize setting storage file {0}".FormatWith(fileName), this, ex);
      return null;
    }
  }

  protected virtual XmlSerializer GetBucketXmlSerializer()
  {
    return new XmlSerializer(typeof(Bucket));
  }

  protected virtual string GetFilePath()
  {
    string folderName = null;
    if (!UseLocalFolder)
    {
      folderName = Settings.ApplicationDataFolderName;
      if (!folderName.IsNullOrEmpty())
      {
        folderName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);
      }
    }
    if (folderName.IsNullOrEmpty())
    {
      folderName = DirectoryHelper.CurrentDirectory;
    }
    Assert.IsNotNullOrEmpty(folderName, "Folder name shouldn't be unresolved");
    return Path.Combine(folderName, FileName);
  }

  protected virtual bool SerializeToStorageFile(Bucket bucket)
  {
    string filePath = GetFilePath();
    try
    {
      var parentDirectory = Directory.GetParent(filePath);
      if (!parentDirectory.Exists)
      {
        Directory.CreateDirectory(parentDirectory.FullName);
      }
      using (var streamWriter = new StreamWriter(filePath, false))
      {
        XmlSerializer serializer = GetBucketXmlSerializer();
        serializer.Serialize(streamWriter, bucket);
        if (EnableDebuggingTraces)
        {
          using (var memStream = new MemoryStream())
          {
            serializer.Serialize(memStream, bucket);
            TraceDebugStreamContent(memStream, "SaveOp");
          }
        }
      }
      return true;
    }
    catch (Exception ex)
    {
      Log.Error("Failed to save settings to file", ex, this);
      return false;
    }
  }

  protected virtual void TraceDebugStreamContent(Stream streamToTrace, string fileNameSuffix)
  {
    try
    {
      string debugTracesDirectory = Path.Combine(DirectoryHelper.CurrentDirectory, "SettingsStorageDebugTraces");
      Directory.CreateDirectory(debugTracesDirectory);
      string traceFileName = "{0} --- {1}.txt".FormatWith(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff"), fileNameSuffix);
      using (FileStream traceFileStream = File.Create(Path.Combine(debugTracesDirectory, traceFileName)))
      {
        streamToTrace.Seek(0, SeekOrigin.Begin);
        streamToTrace.CopyTo(traceFileStream);

        var streamWriter = new StreamWriter(traceFileStream);
        streamWriter.WriteLine();
        streamWriter.WriteLine();
        streamWriter.WriteLine();
        streamWriter.WriteLine("Current stack trace:");
        streamWriter.WriteLine(Environment.StackTrace);
      }
    }
    catch (Exception ex)
    {
      Log.Error("Enable to perform trace logging for SettingsStorage", ex, this);
    }
  }
}