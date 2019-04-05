using System;
using System.Collections.Generic;
using SerializedType = UnityEngine.BoundsInt;

namespace BeauData
{
    public abstract partial class Serializer
    {
        static private void Serialize_BoundsInt(ref SerializedType ioData, Serializer ioSerializer)
        {
            UnityEngine.Vector3Int min = ioData.min, max = ioData.max;

            ioSerializer.Serialize("min", ref min, FieldOptions.PreferAttribute);
            ioSerializer.Serialize("max", ref max, FieldOptions.PreferAttribute);

            if (ioSerializer.IsWriting)
            {
                ioData.SetMinMax(min, max);
            }
        }

        public void Serialize(string inKey, ref SerializedType ioData, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<SerializedType>(inKey, ref ioData, inOptions, Serialize_BoundsInt);
        }

        public void Serialize(string inKey, ref SerializedType ioData, SerializedType inDefault, FieldOptions inOptions = FieldOptions.None)
        {
            DoStruct<SerializedType>(inKey, ref ioData, inDefault, inOptions, Serialize_BoundsInt);
        }

        public void Array(string inKey, ref List<SerializedType> ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<SerializedType>(inKey, ref ioArray, inOptions, Serialize_BoundsInt);
        }

        public void Array(string inKey, ref SerializedType[] ioArray, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructArray<SerializedType>(inKey, ref ioArray, inOptions, Serialize_BoundsInt);
        }

        public void Set(string inKey, ref HashSet<SerializedType> ioSet, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructSet<SerializedType>(inKey, ref ioSet, inOptions, Serialize_BoundsInt);
        }

        public void Map(string inKey, ref Dictionary<string, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<SerializedType>(inKey, ref ioMap, inOptions, Serialize_BoundsInt);
        }

        public void Map(string inKey, ref Dictionary<int, SerializedType> ioMap, FieldOptions inOptions = FieldOptions.None)
        {
            DoStructMap<SerializedType>(inKey, ref ioMap, inOptions, Serialize_BoundsInt);
        }
    }
}