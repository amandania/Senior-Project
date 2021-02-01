using DotNetty.Buffers;

public interface IDataSender
{
    void SendDataTo(Player index, IByteBuffer data);
    void SendDataToAll(IByteBuffer data);
    void SendDataToAllBut(Player index, IByteBuffer data);
}