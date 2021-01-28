using Engine.Entity.npc;
using System.Threading.Tasks;

namespace Engine.Interfaces
{
    public interface INPCMovement
    {
        Task Process(NPC npc);
    }
}