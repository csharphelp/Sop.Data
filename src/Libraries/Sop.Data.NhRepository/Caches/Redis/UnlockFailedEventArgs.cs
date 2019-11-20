namespace Sop.Data.NhRepositories.Caches.Redis
{
    public class UnlockFailedEventArgs
    {
        public string RegionName { get; private set; }
        public object Key { get; private set; }
        public string LockKey { get; private set; }
        public string LockValue { get; private set; }

        internal UnlockFailedEventArgs(string regionName, object key, string lockKey, string lockValue)
        {
            this.RegionName = regionName;
            this.Key = key;
            this.LockKey = lockKey;
            this.LockValue = lockValue;
        }
    }
}
