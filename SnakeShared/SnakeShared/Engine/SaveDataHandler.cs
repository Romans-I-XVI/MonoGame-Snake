using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace Snake
{
    public static class SaveDataHandler
    {
        private static readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
        public static bool EnableCaching = true;

        public static async void SaveData(string data, string filename = "savedata.json")
        {
            if (EnableCaching)
            {
                lock(_cache)
                {
                    _cache[filename] = data;
                }
            }

            Action save_delegate = delegate
            {
#if XBOX_LIVE
                XboxLiveConnectedStorage.Save(data, filename);
#elif PLAYSTATION4
                PS4SaveDataHandler.Save(data, filename);
#else
  #if NETFX_CORE
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
  #else
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
  #endif

                try
                {
                    using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filename, FileMode.Create, isoStore))
                    {
                        using (StreamWriter writer = new StreamWriter(isoStream))
                        {
                            writer.WriteLine(data);
                            Debug.WriteLine("You have written to the file.");
                        }
                    }
                }
                catch
                {
                    Debug.WriteLine("Something went wrong writing to the file");
                }
#endif
            };
            await Task.Run(save_delegate);
        }

        public static string LoadData(string filename = "savedata.json")
        {
            if (EnableCaching)
            {
                lock(_cache)
                {
                    if (_cache.ContainsKey(filename) && !string.IsNullOrWhiteSpace(_cache[filename]))
                    {
                        Debug.WriteLine("SaveDataHandler :: Returning previously loaded or saved file data - " + _cache[filename]);
                        return _cache[filename];
                    }
                }
            }

#if XBOX_LIVE
            var loadTask = XboxLiveConnectedStorage.Load(filename);
            loadTask.Wait(5000);
            string data = loadTask.Result;
            if (EnableCaching)
            {
                lock (_cache)
                {
                    _cache[filename] = data;
                }
            }

            return data;
#elif PLAYSTATION4
            string data = PS4SaveDataHandler.Load(filename);
            if (EnableCaching)
            {
                lock (_cache)
                {
                    _cache[filename] = data;
                }
            }

            return data;
#else
#if NETFX_CORE
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();
#else
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
#endif

            if (isoStore.FileExists(filename))
            {
                using (IsolatedStorageFileStream isoStream = new IsolatedStorageFileStream(filename, FileMode.Open, isoStore))
                {
                    using (StreamReader reader = new StreamReader(isoStream))
                    {
                        Debug.WriteLine("Reading contents:");
                        string data = reader.ReadToEnd();
                        if (EnableCaching)
                        {
                            lock (_cache)
                            {
                                _cache[filename] = data;
                            }
                        }

                        return data;
                    }
                }
            }
            return null;
#endif

        }

        public static void ResetCache()
        {
            Debug.WriteLine("SaveDataHandler :: Clearing Cached Data");
            lock (_cache)
            {
                _cache.Clear();
            }
        }
    }
}
