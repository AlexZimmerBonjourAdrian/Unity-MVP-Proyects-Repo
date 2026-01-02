namespace ECS.Core
{
    public interface ISystem
    {
        void Initialize(World world);
        void Update(float deltaTime);
        void Shutdown();
    }
}

