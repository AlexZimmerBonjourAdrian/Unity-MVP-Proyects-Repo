using ECS.Core;
using ECS.Components.Anger;

namespace ECS.Systems
{
    public class AngerSystem : ISystem
    {
        private World world;
        private Entity playerEntity;

        public void Initialize(World world)
        {
            this.world = world;
        }

        public void Update(float deltaTime)
        {
            // Verificar si el jugador explotó de ira
            if (playerEntity.Id != 0 && HasExploded())
            {
                var anger = world.GetComponent<AngerComponent>(playerEntity);
                if (anger != null && anger.HasExploded)
                {
                    // Notificar GameManager para Game Over
                    if (Managers.GameManager.Instance != null)
                    {
                        Managers.GameManager.Instance.GameOver();
                    }
                }
            }
        }

        public void Shutdown()
        {
        }

        public void SetPlayerEntity(Entity entity)
        {
            playerEntity = entity;
        }

        public void IncreaseAnger(float amount)
        {
            if (world == null || playerEntity.Id == 0) return;

            var anger = world.GetComponent<AngerComponent>(playerEntity);
            if (anger == null) return;

            anger.CurrentAnger += amount;
            
            // Asegurar que no exceda el máximo
            if (anger.CurrentAnger > anger.MaxAnger)
            {
                anger.CurrentAnger = anger.MaxAnger;
                anger.HasExploded = true;
            }
        }

        public void DecreaseAnger(float amount)
        {
            if (world == null || playerEntity.Id == 0) return;

            var anger = world.GetComponent<AngerComponent>(playerEntity);
            if (anger == null) return;

            anger.CurrentAnger -= amount;
            
            // Asegurar que no sea negativo
            if (anger.CurrentAnger < 0f)
            {
                anger.CurrentAnger = 0f;
            }
        }

        public float GetCurrentAnger()
        {
            if (world == null || playerEntity.Id == 0) return 0f;

            var anger = world.GetComponent<AngerComponent>(playerEntity);
            if (anger == null) return 0f;

            return anger.CurrentAnger;
        }

        public bool HasExploded()
        {
            if (world == null || playerEntity.Id == 0) return false;

            var anger = world.GetComponent<AngerComponent>(playerEntity);
            if (anger == null) return false;

            return anger.HasExploded;
        }

        public string GetAngerWarning()
        {
            if (world == null || playerEntity.Id == 0) return "";

            var anger = world.GetComponent<AngerComponent>(playerEntity);
            if (anger == null) return "";

            float percentage = anger.GetAngerPercentage();
            
            if (percentage >= 90f)
            {
                return "¡CUIDADO! Una más y explotas";
            }
            else if (percentage >= 75f)
            {
                return "¡Respira! Estás al límite";
            }
            else if (percentage >= 50f)
            {
                return "Mantén la calma...";
            }
            
            return "";
        }
    }
}

