using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace WebClientOrder.Infra.Map
{
    public static class MongoConfigure
    {
        public static void Configure()
        {
            EntitesMap.Configure();

            BsonDefaults.GuidRepresentation = GuidRepresentation.CSharpLegacy;

            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("Conventions", pack, t => true);
        }
    }
}
