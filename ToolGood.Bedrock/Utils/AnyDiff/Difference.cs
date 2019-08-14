﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ToolGood.AnyDiff
{
    /// <summary>
    /// Calculate the difference between two objects
    /// </summary>
    public class Difference
    {
        private readonly Dictionary<Type, Func<object, object, object>> _differenceByType = new Dictionary<Type, Func<object, object, object>> {
                { typeof(byte), (leftValue, rightValue) => (byte)rightValue - (byte)leftValue },
                { typeof(byte?), (leftValue, rightValue) => ((byte?)rightValue ?? 0) - ((byte?)leftValue ?? 0) },
                { typeof(sbyte), (leftValue, rightValue) => (sbyte)rightValue - (sbyte)leftValue },
                { typeof(sbyte?), (leftValue, rightValue) => ((sbyte?)rightValue ?? 0) - ((sbyte?)leftValue ?? 0) },
                { typeof(short), (leftValue, rightValue) => (short)rightValue - (short)leftValue },
                { typeof(short?), (leftValue, rightValue) => ((short?)rightValue ?? 0) - ((short?)leftValue ?? 0) },
                { typeof(ushort), (leftValue, rightValue) => (ushort)rightValue - (ushort)leftValue },
                { typeof(ushort?), (leftValue, rightValue) => ((ushort?)rightValue ?? 0) - ((ushort?)leftValue ?? 0) },
                { typeof(int), (leftValue, rightValue) => (int)rightValue - (int)leftValue },
                { typeof(int?), (leftValue, rightValue) => ((int?)rightValue ?? 0) - ((int?)leftValue ?? 0 )},
                { typeof(uint), (leftValue, rightValue) => (uint)rightValue - (uint)leftValue },
                { typeof(uint?), (leftValue, rightValue) => ((uint?)rightValue ?? 0) - ((uint?)leftValue ?? 0 )},
                { typeof(long), (leftValue, rightValue) => (long)rightValue - (long)leftValue },
                { typeof(long?), (leftValue, rightValue) => ((long?)rightValue ?? 0) - ((long?)leftValue ?? 0) },
                { typeof(ulong), (leftValue, rightValue) => (ulong)rightValue - (ulong)leftValue },
                { typeof(ulong?), (leftValue, rightValue) => ((ulong?)rightValue ?? 0) - ((ulong?)leftValue ?? 0) },
                { typeof(float), (leftValue, rightValue) => (float)rightValue - (float)leftValue },
                { typeof(float?), (leftValue, rightValue) => ((float?)rightValue ?? 0) - ((float?)leftValue ?? 0) },
                { typeof(double), (leftValue, rightValue) => (double)rightValue - (double)leftValue },
                { typeof(double?), (leftValue, rightValue) => ((double?)rightValue ?? 0) - ((double?)leftValue ?? 0) },
                { typeof(decimal), (leftValue, rightValue) => (decimal)rightValue - (decimal)leftValue },
                { typeof(decimal?), (leftValue, rightValue) => ((decimal?)rightValue ?? 0) - ((decimal?)leftValue ?? 0) },
                { typeof(TimeSpan), (leftValue, rightValue) => (TimeSpan)rightValue - (TimeSpan)leftValue },
                { typeof(TimeSpan?), (leftValue, rightValue) => ((TimeSpan?)rightValue ?? TimeSpan.Zero) - ((TimeSpan?)leftValue ?? TimeSpan.Zero) },
                { typeof(DateTime), (leftValue, rightValue) => (DateTime)rightValue - (DateTime)leftValue },
                { typeof(DateTime?), (leftValue, rightValue) => ((DateTime?)rightValue ?? new DateTime()) - ((DateTime?)leftValue ?? new DateTime()) },
                // these values are not swapped in order
                { typeof(Guid), (leftValue, rightValue) => { return CompareStrings(((Guid)leftValue).ToString(), ((Guid)rightValue).ToString()); } },
                { typeof(Guid?), (leftValue, rightValue) => { return CompareStrings(((Guid?)leftValue)?.ToString(), ((Guid?)rightValue)?.ToString()); } },
                { typeof(string), (leftValue, rightValue) => { return CompareStrings((string)leftValue, (string)rightValue); } },
            };

        public string Path { get; }
        public string Property { get; }
        public int? ArrayIndex { get; }
        public Type PropertyType { get; }
        public TypeConverter TypeConverter { get; }
        public object LeftValue { get; }
        public object RightValue { get; }
        public object Delta { get; }

        /// <summary>
        /// Calculate the difference between two objects
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="property"></param>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <param name="converter"></param>
        public Difference(Type propertyType, string property, string path, object leftValue, object rightValue, TypeConverter converter)
        {
            PropertyType = propertyType;
            if (Nullable.GetUnderlyingType(propertyType) == null && (leftValue == null || rightValue == null))
            {
                // type is not nullable, but we've been passed a nullable type. Convert the type to nullable
                // so we can perform diff checks on it properly
                PropertyType = GetNullableType(propertyType);
            }
            Path = path;
            Property = property;
            LeftValue = leftValue;
            RightValue = rightValue;
            TypeConverter = converter;
            var leftValueLocal = leftValue;
            var rightValueLocal = rightValue;

            // if a type converter is specified, convert the type before performing a Diff evaluation
            if (TypeConverter != null)
            {
                if (TypeConverter.CanConvertFrom(PropertyType))
                {
                    if (leftValueLocal != null)
                    {
                        leftValueLocal = TypeConverter.ConvertFrom(leftValueLocal);
                        PropertyType = leftValueLocal.GetType();
                    }
                    if (rightValueLocal != null)
                    {
                        rightValueLocal = TypeConverter.ConvertFrom(rightValueLocal);
                        PropertyType = rightValueLocal.GetType();
                    }
                }
            }
            Delta = CreateDelta(leftValueLocal, rightValueLocal);
        }

        /// <summary>
        /// Calculate the difference between two objects
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="property"></param>
        /// <param name="path"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <param name="converter"></param>
        public Difference(Type propertyType, string property, string path, int arrayIndex, object leftValue, object rightValue) : this(propertyType, property, path, leftValue, rightValue, null)
        {
            ArrayIndex = arrayIndex;
        }

        /// <summary>
        /// Calculate the difference between two objects
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="property"></param>
        /// <param name="path"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <param name="converter"></param>
        public Difference(Type propertyType, string property, string path, int arrayIndex, object leftValue, object rightValue, TypeConverter converter) : this(propertyType, property, path, leftValue, rightValue, converter)
        {
            ArrayIndex = arrayIndex;
        }

        /// <summary>
        /// Compute the delta between two objects
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        private object CreateDelta(object leftValue, object rightValue)
        {
            object returnVal = null;

            if (_differenceByType.ContainsKey(PropertyType))
                returnVal = _differenceByType[PropertyType](leftValue, rightValue);

            return returnVal;
        }

        /// <summary>
        /// Get a list of the types supported by the Difference operation
        /// </summary>
        /// <returns></returns>
        public List<Type> GetSupportedTypes()
        {
            return _differenceByType.Keys.ToList();
        }

        public override string ToString()
        {
            return Path;
        }

        private static object CompareStrings(string leftValue, string rightValue)
        {
            if (leftValue == null)
                return rightValue;
            if (rightValue == null)
                return leftValue;

            // we will opt to use word comparison for comparing strings
            var wordDifferences = DifferenceWords.DiffWords(leftValue, rightValue);

            return wordDifferences;
        }

        private static Type GetNullableType(Type type)
        {
            // Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable.
            var typeLocal = Nullable.GetUnderlyingType(type) ?? type;
            if (typeLocal.IsValueType)
                return typeof(Nullable<>).MakeGenericType(typeLocal);
            else
                return typeLocal;
        }
    }
}
