using com.tvd12.ezyfoxserver.client.entity;

namespace EUN.Entity
{
    public class RoomGameObject
    {
        public int ObjectId { get; private set; }
        public int OwnerId { get; set; }
        public string PrefabPath { get; set; }
        public object SynchronizationData { get; set; }
        public object InitializeData { get; set; }

        public RoomGameObject(EzyArray ezyArray)
        {
            ObjectId = ezyArray.get<int>(0);
            OwnerId = ezyArray.get<int>(1);
            PrefabPath = ezyArray.get<string>(2);
            SynchronizationData = ezyArray.get<object>(3);
            InitializeData = ezyArray.get<object>(4);
        }
    }
}