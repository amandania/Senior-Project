using Engine.Net;

namespace Engine.Interfaces
{
				public interface ICombatController
				{
								void Attack(Character attacker, Character damage);
								void HandleDamage(Character attacker, Character target);
				}
}
