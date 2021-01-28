using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.Compression;
using System.IO;

public class UmaConstants : MonoBehaviour
{
    public static UmaConstants instance;

    private void Awake()
    {
        instance = new UmaConstants();
    }

    public const int MAX_PLAYERS = 100;

    [Serializable]
    public enum UMASLOT
    {
        HAIR = 1,
        HAIRADD = 2,
        HEAD = 3,
        EYE = 4,
        MOUTH = 5,
        TORSO = 6,
        HANDS = 7,
        LEGS = 8,
        FEET = 9,
        S_EAR = 10,
        S_NOSE = 11,
        S_MOUTH = 12,
        S_HEAD = 13,
        S_EYE = 14,
        EYELASH = 15,
        A_FEET = 21,
        A_LEGS = 22,
        A_TORSO = 23,
        A_ARMS = 24,
        A_SHOULDER = 25,
        A_HANDS = 26
    }
    public enum UMASLOT_GROUP
    {
        NOTHING = 0,
        SKIN = 1,
        HAIR = 2,
        EYE = 3,
        ARMOR = 4,
        UPPER_ARMOR = 5,
        LOWER_ARMOR = 6
    }

    public class RaceNtoN
    {
        public static Dictionary<string, int> racedic = new Dictionary<string, int>()
        {
            {"HumanMale", 0},
            {"HumanFemale", 1},
            {"ElfMale", 2},
            {"ElfFemale", 3}
        };
    }




    public string Compress(byte[] buffer)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var DeflateStream = new DeflateStream(memoryStream, CompressionMode.Compress, true))
            {
                DeflateStream.Write(buffer, 0, buffer.Length);
            }
            memoryStream.Position = 0;

            var compressdedData = new byte[memoryStream.Length];
            memoryStream.Read(compressdedData, 0, compressdedData.Length);

            var gZipBuffer = new byte[compressdedData.Length + 4];
            Buffer.BlockCopy(compressdedData, 0, gZipBuffer, 4, compressdedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }
    }

    public byte[] Decompress(string text)
    {
        byte[] gzipbuffer = Convert.FromBase64String(text);
        using (var memoryStream = new MemoryStream())
        {
            int dataLength = BitConverter.ToInt32(gzipbuffer, 0);
            memoryStream.Write(gzipbuffer, 4, gzipbuffer.Length - 4);

            var buffer = new byte[dataLength];
            memoryStream.Position = 0;
            using (var gZipStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
            {
                gZipStream.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
    }
}
