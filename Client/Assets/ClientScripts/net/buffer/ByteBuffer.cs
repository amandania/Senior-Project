using System;
using System.Collections.Generic;
using System.Text;

public class ByteBuffer : IDisposable
{
    List<byte> buff;
    byte[] readBuff;
    int readPos;
    bool buffUpdate = false;

    public ByteBuffer()
    {
        buff = new List<byte>(); // Intitialize buff
        readPos = 0; // Set readPos to 0
    }

    public int GetReadPos()
    {
        return readPos; // Return the position where we are reading from in the byte array
    }

    public byte[] ToArray()
    {
        return buff.ToArray(); // Return a byte array of buff
    }

    public int Count()
    {
        return buff.Count; // Return the length of buff
    }

    public int Length()
    {
        return Count() - readPos; // Return the remaining length (unread)
    }

    public void Clear()
    {
        buff.Clear(); // Clear buff
        readPos = 0; // Reset readPos
    }

    #region"Write Data"
    public void WriteByte(byte _input)
    {
        buff.Add(_input); // Add _input to buff
        buffUpdate = true;
    }

    public void WriteBytes(byte[] _input)
    {
        buff.AddRange(_input); // Add _input to buff
        buffUpdate = true;
    }

    public void WriteShort(short _input)
    {
        buff.AddRange(BitConverter.GetBytes(_input)); // Convert short to bytes and add them to buff
        buffUpdate = true;
    }

    public void WriteInteger(int _input)
    {
        buff.AddRange(BitConverter.GetBytes(_input)); // Convert int to bytes and add them to buff
        buffUpdate = true;
    }

    public void WriteFloat(float _input)
    {
        buff.AddRange(BitConverter.GetBytes(_input)); // Convert float to bytes and add them to buff
        buffUpdate = true;
    }

    public void WriteString(string _input)
    {
        buff.AddRange(BitConverter.GetBytes(_input.Length)); // Convert the length of the string (_input.Length) to bytes and add them to buff
        buff.AddRange(Encoding.ASCII.GetBytes(_input)); // Convert string to bytes and add them to buff
        buffUpdate = true;
    }

    public void WriteBool(bool _input)
    {
        buff.AddRange(BitConverter.GetBytes(_input)); // Convert bool to bytes and add them to buff
        buffUpdate = true;
    }

    public void WriteLong(long _input) // Untested
    {
        buff.AddRange(BitConverter.GetBytes(_input)); // Convert long to bytes and add them to buff
        buffUpdate = true;
    }
    #endregion

    #region "Read Data"
    public string ReadString(bool _peek = true)
    {
        int length = ReadInteger(true); // Get the length of the string
        if (buffUpdate)
        {
            readBuff = buff.ToArray();
            buffUpdate = false;
        }

        string ret = Encoding.ASCII.GetString(readBuff, readPos, length); // Convert the bytes to a string
        if (_peek & buff.Count > readPos)
        {
            // If _peek is true and there are unread bytes
            if (ret.Length > 0)
            {
                // If the string length is > 0
                readPos += length; // Increase readPos by the length of the string
            }
        }
        return ret; // Return the string
    }

    public byte ReadByte(bool _peek = true)
    {
        if (buff.Count > readPos)
        {
            // If there are unread bytes
            if (buffUpdate)
            {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            byte ret = readBuff[readPos]; // Get the byte at readPos' position
            if (_peek & buff.Count > readPos)
            {
                // If _peek is true and there are unread bytes
                readPos += 1; // Increase readPos by 1
            }
            return ret; // Return the byte
        }
        else
        {
            throw new Exception("Byte Buffer Past Limit!");
        }
    }

    public byte[] ReadBytes(int _length, bool _peek = true)
    {
        if (buffUpdate)
        {
            readBuff = buff.ToArray();
            buffUpdate = false;
        }

        byte[] ret = buff.GetRange(readPos, _length).ToArray(); // Get the bytes at readPos' position with a range of _length
        if (_peek)
        {
            // If _peek is true
            readPos += _length; // Increase readPos by _length
        }
        return ret; // Return the bytes
    }

    public float ReadFloat(bool _peek = true)
    {
        if (buff.Count > readPos)
        {
            // If there are unread bytes
            if (buffUpdate)
            {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            float ret = BitConverter.ToSingle(readBuff, readPos); // Convert the bytes to a float
            if (_peek & buff.Count > readPos)
            {
                // If _peek is true and there are unread bytes
                readPos += 4; // Increase readPos by 4
            }
            return ret; // Return the float
        }
        else
        {
            throw new Exception("Byte Buffer is Past its Limit!");
        }
    }

    public int ReadInteger(bool _peek = true)
    {
        if (buff.Count > readPos)
        {
            // If there are unread bytes
            if (buffUpdate)
            {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            int ret = BitConverter.ToInt32(readBuff, readPos); // Convert the bytes to an int
            if (_peek & buff.Count > readPos)
            {
                // If _peek is true and there are unread bytes
                readPos += 4; // Increase readPos by 4
            }
            return ret; // Return the int
        }
        else
        {
            throw new Exception("Byte Buffer is Past its Limit!");
        }
    }

    public bool ReadBool(bool _peek = true)
    {
        if (buff.Count > readPos)
        {
            // If there are unread bytes
            if (buffUpdate)
            {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            bool ret = BitConverter.ToBoolean(readBuff, readPos); // Convert the bytes to a bool
            if (_peek & buff.Count > readPos)
            {
                // If _peek is true and there are unread bytes
                readPos += 1; // Increase readPos by 1
            }
            return ret; // Return the bool
        }
        else
        {
            throw new Exception("Byte Buffer is Past its Limit!");
        }
    }

    public long ReadLong(bool _peek = true) // Untested
    {
        if (buff.Count > readPos)
        {
            // If there are unread bytes
            if (buffUpdate)
            {
                readBuff = buff.ToArray();
                buffUpdate = false;
            }

            long ret = BitConverter.ToInt64(readBuff, readPos); // Convert the bytes to a long
            if (_peek & buff.Count > readPos)
            {
                // If _peek is true and there are unread bytes
                readPos += 8; // Increase readPos by 8
            }
            return ret; // Return the long
        }
        else
        {
            throw new Exception("Byte Buffer is Past its Limit!");
        }
    }
    #endregion

    private bool disposedValue = false;

    // IDisposable
    protected virtual void Dispose(bool _disposing)
    {
        if (!this.disposedValue)
        {
            if (_disposing)
            {
                buff.Clear();
            }

            readPos = 0;
        }
        this.disposedValue = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}