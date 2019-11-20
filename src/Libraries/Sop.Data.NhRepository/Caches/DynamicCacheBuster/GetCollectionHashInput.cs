namespace Sop.Data.NhRepositories.Caches.DynamicCacheBuster
{
    using Collection = global::NHibernate.Mapping.Collection;

    public delegate object GetCollectionHashInput(Collection collection);
}
