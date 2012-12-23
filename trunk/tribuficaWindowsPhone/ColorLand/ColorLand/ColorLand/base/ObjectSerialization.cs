using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;

namespace ColorLand
{
    class ObjectSerialization
    {
        public static IsolatedStorageFile GetUserStoreAsAppropriateForCurrentPlatform()
        {
#if WINDOWS
            return IsolatedStorageFile.GetUserStoreForDomain();
#else
        return IsolatedStorageFile.GetUserStoreForApplication();
#endif
        }

        public static void Save<T>(string fileName, T item)
        {
            using (IsolatedStorageFile storage = GetUserStoreAsAppropriateForCurrentPlatform())
            {
                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, storage))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(fileStream, item);
                }
            }
        }

        public static T Load<T>(string fileName)
        {
            using (IsolatedStorageFile storage = GetUserStoreAsAppropriateForCurrentPlatform())
            {
                using (IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, storage))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(fileStream);
                }
            }
        }
    }
}
