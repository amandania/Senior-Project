using DotNetty.Buffers;
using System.Threading.Tasks;

/// <summary>
/// This interface is used to setup all our incoming packet types to be executed appropiately. Check the manual for the tree diagram for inheritance.
/// All packets inheirt from this class and call the executable function to read the buffer message in order.
/// </summary>
public interface IIncomingPackets
{
    /// <summary>
    /// The packet Identifier
    /// </summary>
    IncomingPackets PacketType { get; }

    /// <summary>
    /// This function will read the buffer messsage based on our packet defintion
    /// </summary>
    /// <param name="a_player">Player who is executing the packet</param>
    /// <param name="a_buffer">The buffer message containing packet info.</param>
    /// <returns></returns>
    Task ExecutePacket(Player a_player, IByteBuffer a_buffer);

}
