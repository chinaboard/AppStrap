using System.Collections.Concurrent;
namespace AppStrap
{
    public class AppStrapList
    {
        public ConcurrentDictionary<string, AppStrap> List { get; private set; }
        public static AppStrapList Instance { get; private set; }
        public AppStrapList()
        {
            this.List = new ConcurrentDictionary<string, AppStrap>();
        }

        static AppStrapList()
        {
            Instance = new AppStrapList();
        }

        public AppStrap GetOrAdd(string key, AppStrap value)
        {
            if (this.List.ContainsKey(key))
                Utils.Preconditions.CheckNull(this.List[key], key);
            return this.List.GetOrAdd(key, value);
        }


    }
}
