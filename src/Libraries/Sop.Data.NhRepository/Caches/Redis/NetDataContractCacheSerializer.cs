using System.Runtime.Serialization;

namespace Sop.Data.NhRepositories.Caches.Redis
{
    /// <summary>
    /// 
    /// </summary>
    public class NetDataContractCacheSerializer : XmlRedisCacheSerializerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override XmlObjectSerializer CreateSerializer()
        {
            //  var serializer = new NetDataContractSerializer();
            var serializer = new DataContractSerializer(this.GetType());

            return serializer;
        }

    }
}
