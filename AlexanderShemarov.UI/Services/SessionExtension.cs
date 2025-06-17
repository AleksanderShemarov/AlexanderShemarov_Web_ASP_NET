using System.Text.Json;

namespace AlexanderShemarov.UI.Services
{
    public static class SessionExtension
    {
        public static void Set<T>(this ISession session, string key, T trainItem)
        {
            var serializedItem = JsonSerializer.Serialize(trainItem);
            session.SetString(key, serializedItem);
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var trainItem = session.GetString(key);
            return trainItem == null
                ? Activator.CreateInstance<T>()
                : JsonSerializer.Deserialize<T>(trainItem);
        }
    }
}
