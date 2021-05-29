using MongoDB.Bson.Serialization;
using WebClientOrder.Domain.Entities;

namespace WebClientOrder.Infra.Map
{
    public class EntitesMap
    {
        public static void Configure()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Entity)))
            {
                BsonClassMap.RegisterClassMap<Entity>(map =>
               {
                   map.AutoMap();
                   map.SetIsRootClass(true);
                   map.SetIgnoreExtraElements(true);
                   map.SetIgnoreExtraElementsIsInherited(true);
                   map.AddKnownType(typeof(Product));
                   map.AddKnownType(typeof(Client));
                   map.AddKnownType(typeof(Order));
                   map.AddKnownType(typeof(Product));
               });

            }
        }
    }
}
