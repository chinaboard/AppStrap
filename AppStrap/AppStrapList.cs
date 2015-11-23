using System.Collections.Concurrent;
namespace AppStrap
{
    public class AppStrapList
    {
        public ConcurrentDictionary<string, AppStrap> List { get; private set; }
        public static AppStrapList Instance { get; private set; }
        public AppStrapList()
        {
            List = new ConcurrentDictionary<string, AppStrap>();
        }

        static AppStrapList()
        {
            Instance = new AppStrapList();
        }

        public AppStrap GetOrAdd(string key, AppStrap value)
        {
            if (List.ContainsKey(key))
                Utils.Preconditions.CheckNull(List[key], key);
            return List.GetOrAdd(key, value);
        }


    }
}
