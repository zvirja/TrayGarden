using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ClipboardChangerPlant.Shortening.Google
{
  public class SerializationHelper
  {
    public static T DeserializeObject<T>(Stream stream) where T : class
    {
      if (stream == null)
      {
        return null;
      }
      var serializer = new DataContractJsonSerializer(typeof(T));
      if (stream.CanSeek)
      {
        stream.Seek(0, SeekOrigin.Begin);
      }
      object result = serializer.ReadObject(stream);
      return result as T;
    }

    public static string SerializeToString<T>(T inputObj) where T : class
    {
      if (inputObj == null)
      {
        return null;
      }
      var serializer = new DataContractJsonSerializer(typeof(T));
      var ms = new MemoryStream();
      serializer.WriteObject(ms, inputObj);
      ms.Seek(0, SeekOrigin.Begin);
      return Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
    }
  }
}