﻿/*
 * Copyright (C) 2017 - 2020. Autumn Beauchesne. All rights reserved.
 * Author:  Autumn Beauchesne
 * Date:    3 May 2017
 * 
 * File:    TypeUtility.cs
 * Purpose: Manages type aliases.
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace BeauData
{
    /// <summary>
    /// Manages type aliases.
    /// </summary>
    static public class TypeUtility
    {
        static private readonly Dictionary<Type, string> s_TypeToAlias = new Dictionary<Type, string>();
        static private readonly Dictionary<string, Type> s_AliasToType = new Dictionary<string, Type>();
        static private readonly Dictionary<string, Type> s_RenamedTypes = new Dictionary<string, Type>();

        static private readonly Dictionary<Type, ConstructorInfo> s_ConstructorsSystem = new Dictionary<Type, ConstructorInfo>();
        static private readonly Dictionary<Type, Delegate> s_TypeSerializers = new Dictionary<Type, Delegate>();

        public delegate void TypeSerializerDelegate<T>(ref T ioObject, Serializer ioSerializer);

        static internal Type NameToType(string inName)
        {
            Type type;
            if (!s_AliasToType.TryGetValue(inName, out type) && !s_RenamedTypes.TryGetValue(inName, out type))
                type = Type.GetType(inName);
            return type;
        }

        static internal string TypeToName(Type inType)
        {
            string name;
            if (!s_TypeToAlias.TryGetValue(inType, out name))
                name = inType.AssemblyQualifiedName;
            return name;
        }

        static internal TypeSerializerDelegate<T> CustomSerializer<T>()
        {
            Delegate serializer;
            s_TypeSerializers.TryGetValue(typeof(T), out serializer);
            return (TypeSerializerDelegate<T>) serializer;
        }

        static internal object Instantiate(Type inType, Serializer inSerializer)
        {
            if (inType.IsValueType)
            {
                return Activator.CreateInstance(inType);
            }

            Type typeKey = inType;
            ConstructorInfo constructor;
            if (!s_ConstructorsSystem.TryGetValue(typeKey, out constructor))
                constructor = s_ConstructorsSystem[typeKey] = inType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                UnityEngine.Debug.LogError("[BeauData] Could not find empty constructor for type '" + inType.FullName + "'");
                return null;
            }
            
            return constructor.Invoke(null);
        }

        #region Alias Scanning

        // static private bool s_ScanCount = false;
        // static private readonly object s_ScanLock = new object();

        // static public void ScanAssembly()
        // {

        // }

        #endregion // Alias Scanning

        /// <summary>
        /// Registers a type alias, for use when serializing subclasses.
        /// </summary>
        static public void RegisterAlias(Type inType, string inAliasName)
        {
            s_AliasToType[inAliasName] = inType;
            s_TypeToAlias[inType] = inAliasName;
        }

        /// <summary>
        /// Registers a type alias, for use when serializing subclasses.
        /// </summary>
        static public void RegisterAlias<T>(string inAliasName) where T : ISerializedObject
        {
            Type type = typeof(T);
            s_AliasToType[inAliasName] = type;
            s_TypeToAlias[type] = inAliasName;
        }

        /// <summary>
        /// Registers a type rename, for use when deserializing subclasses.
        /// </summary>
        static public void RegisterRename(Type inType, string inRenameName)
        {
            s_RenamedTypes[inRenameName] = inType;
        }

        /// <summary>
        /// Registers a type rename, for use when deserializing subclasses.
        /// </summary>
        static public void RegisterRename<T>(string inRenameName) where T : ISerializedObject
        {
            Type type = typeof(T);
            s_RenamedTypes[inRenameName] = type;
        }

        /// <summary>
        /// Registers a type serialization function.
        /// </summary>
        static public void RegisterSerializer<T>(TypeSerializerDelegate<T> inSerializeFunction)
        {
            Type type = typeof(T);
            s_TypeSerializers[type] = inSerializeFunction;
        }
    }
}