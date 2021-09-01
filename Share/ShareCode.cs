using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share
{
    public class ShareCode
    {
        public static string NormalizeType(Type TypeInput)
        {
            string res = "string";
            if (TypeInput == typeof(string))
            {
                res = "string";
            }
            else if (TypeInput == typeof(bool))
            {
                res = "bool";
            }
            else if (TypeInput == typeof(byte))
            {
                res = "byte";
            }
            else if (TypeInput == typeof(char))
            {
                res = "char";
            }
            else if (TypeInput == typeof(DateTime))
            {
                res = "DateTime";
            }
            else if (TypeInput == typeof(decimal))
            {
                res = "decimal";
            }
            else if (TypeInput == typeof(double))
            {
                res = "double";
            }
            else if (TypeInput == typeof(float))
            {
                res = "float";
            }
            else if (TypeInput == typeof(Guid))
            {
                res = "Guid";
            }
            else if (TypeInput == typeof(int))
            {
                res = "int";
            }
            else if (TypeInput == typeof(long))
            {
                res = "long";
            }
            else if (TypeInput == typeof(sbyte))
            {
                res = "sbyte";
            }
            else if (TypeInput == typeof(short))
            {
                res = "short";
            }
            else if (TypeInput == typeof(uint))
            {
                res = "uint";
            }
            else if (TypeInput == typeof(ulong))
            {
                res = "ulong";
            }
            else if (TypeInput == typeof(ushort))
            {
                res = "ushort";
            }
            //
            else if (TypeInput == typeof(bool?))
            {
                res = "bool";
            }
            else if (TypeInput == typeof(byte?))
            {
                res = "byte";
            }
            else if (TypeInput == typeof(char?))
            {
                res = "char";
            }
            else if (TypeInput == typeof(DateTime?))
            {
                res = "DateTime";
            }
            else if (TypeInput == typeof(decimal?))
            {
                res = "decimal";
            }
            else if (TypeInput == typeof(double?))
            {
                res = "double";
            }
            else if (TypeInput == typeof(float?))
            {
                res = "float";
            }
            else if (TypeInput == typeof(Guid?))
            {
                res = "Guid";
            }
            else if (TypeInput == typeof(int?))
            {
                res = "int";
            }
            else if (TypeInput == typeof(long?))
            {
                res = "long";
            }
            else if (TypeInput == typeof(sbyte?))
            {
                res = "sbyte";
            }
            else if (TypeInput == typeof(short?))
            {
                res = "short";
            }
            else if (TypeInput == typeof(uint?))
            {
                res = "uint";
            }
            else if (TypeInput == typeof(ulong?))
            {
                res = "ulong";
            }
            else if (TypeInput == typeof(ushort?))
            {
                res = "ushort";
            }
            return res;
        }

        public static bool IsNullable(Type TypeInput)
        {
            var flag = false;
            if (TypeInput == typeof(string))
            {
                flag = true;
            }
            if (TypeInput == typeof(bool?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(byte?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(char?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(DateTime?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(decimal?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(double?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(float?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(Guid?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(int?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(long?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(sbyte?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(short?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(uint?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(ulong?))
            {
                flag = true;
            }
            else if (TypeInput == typeof(ushort?))
            {
                flag = true;
            }

            return flag;
        }
    }
}
