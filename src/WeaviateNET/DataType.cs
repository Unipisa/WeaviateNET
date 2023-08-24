using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeaviateNET
{
    public class WeaviateDataType
    {
        public const string Text = "text";
        public const string TextArray = "text[]";
        public const string Int = "int";
        public const string IntArray = "int[]";
        public const string Boolean = "boolean";
        public const string BooleanArray = "boolean[]";
        public const string Number = "number";
        public const string NumberArray = "number[]";
        public const string Date = "date";
        public const string DateArray = "date[]";
        public const string Uuid = "uuid";
        public const string UuidArray = "uuid[]";
        public const string GeoCoordinates = "geoCoordinates";
        public const string PhoneNumber = "phoneNumber";
        public const string Blob = "blob";

        public static readonly string[] CoreDataTypeNames = new string[]
        {
            "text", "text[]", "int", "int[]", "boolean", "boolean[]",
            "number", "number[]", "date", "date[]", "uuid", "uuid[]",
            "geoCoordinates", "phoneNumber", "blob"
        };

        public static bool isCoreType(string type)
        {
            return CoreDataTypeNames.Contains(type);
        }

        public static bool isCoreType<T>()
        {
            var type = typeof(T);
            switch (type)
            {
                case Type when type == typeof(string):
                case Type when type == typeof(string[]):
                case Type when type == typeof(int):
                case Type when type == typeof(long):
                case Type when type == typeof(int[]):
                case Type when type == typeof(long[]):
                case Type when type == typeof(bool):
                case Type when type == typeof(bool[]):
                case Type when type == typeof(double):
                case Type when type == typeof(double[]):
                case Type when type == typeof(DateTime):
                case Type when type == typeof(DateTime?):
                case Type when type == typeof(DateTime[]):
                case Type when type == typeof(Guid):
                case Type when type == typeof(Guid[]):
                case Type when type == typeof(GeoCoordinates):
                case Type when type == typeof(PhoneNumber):
                case Type when type == typeof(byte[]):
                    return true;
                default:
                    return false;
            }

        }

        public static string MapType(Type type)
        {
            string? typename = null;
            switch (type)
            {
                case Type when type == typeof(string):
                    typename = "text";
                    break;
                case Type when type == typeof(string[]):
                    typename = "text[]";
                    break;
                case Type when type == typeof(int):
                case Type when type == typeof(long):
                    typename = "int";
                    break;
                case Type when type == typeof(int[]):
                case Type when type == typeof(long[]):
                    typename = "int[]";
                    break;
                case Type when type == typeof(bool):
                    typename = "boolean";
                    break;
                case Type when type == typeof(bool[]):
                    typename = "boolean[]";
                    break;
                case Type when type == typeof(double):
                    typename = "number";
                    break;
                case Type when type == typeof(double[]):
                    typename = "number[]";
                    break;
                case Type when type == typeof(DateTime):
                case Type when type == typeof(DateTime?):
                    typename = "date";
                    break;
                case Type when type == typeof(DateTime[]):
                    typename = "date[]";
                    break;
                case Type when type == typeof(Guid):
                    typename = "uuid";
                    break;
                case Type when type == typeof(Guid[]):
                    typename = "uuid[]";
                    break;
                case Type when type == typeof(GeoCoordinates):
                    typename = "geoCoordinates";
                    break;
                case Type when type == typeof(PhoneNumber):
                    typename = "phoneNumber";
                    break;
                // Blob is a string with B64 encoding
                case Type when type == typeof(byte[]):
                    typename = "blob";
                    break;
                default:
                    throw new Exception($"Unsupported type '{type.Name}'");
            }
            return typename;
        }

        public static string SearchValueType<T>()
        {
            var type = typeof(T);
            switch (type)
            {
                case Type when type == typeof(string):
                    return "valueText";
                case Type when type == typeof(int):
                case Type when type == typeof(long):
                    return "valueInt";
                case Type when type == typeof(bool):
                    return "valueBoolean";
                case Type when type == typeof(double):
                    return "valueNumber";
                case Type when type == typeof(DateTime):
                case Type when type == typeof(DateTime?):
                    return "valueDate";
                default:
                    throw new Exception("Unsupported Query type");
            }

        }

        public static string MapType<T>()
        {
            return MapType(typeof(T));
        }
    }
}
