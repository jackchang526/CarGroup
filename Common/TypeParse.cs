﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BitAuto.CarChannel.Common
{
    public class TypeParse
    {
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length <= 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }


        public static bool IsDouble(object Expression)
        {
            if (Expression != null)
            {
                return Regex.IsMatch(Expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(Expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object Expression, int defValue)
        {
            int result;
            if (!int.TryParse((Expression ?? "").ToString(), out result))
            {
                return defValue;
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// 将对象转换为Decimal类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Decimal StrToDecimal(object Expression, Decimal defValue)
        {
            Decimal result;
            if (!Decimal.TryParse((Expression ?? "").ToString(), out result))
            {
                return defValue;
            }
            else
            {
                return result;
            }
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue.ToString(), @"^([-]|[0-9])[0-9]*(\.\d*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }


        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }

        /// <summary>
        /// 将对象转换为DateTime类型
        /// </summary>
        /// <param name="Expression"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime StrToDateTime(object Expression, DateTime defaultValue)
        {
            DateTime dt;
            if (DateTime.TryParse(Expression == null ? "" : Expression.ToString(), out dt))
            {
                return dt;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将字符串数组转换为整型数组
        /// </summary>
        /// <param name="s">字符串数组</param>
        /// <returns></returns>
        public static int[] StrArrToIntArr(string[] s)
        {
            return Array.ConvertAll<string, int>(s, a => int.Parse(a));
        }

        ///// <summary>
        ///// 反射遍历类属性并赋值
        ///// </summary>
        ///// <param name="typeofTarget"></param>
        ///// <param name="typeofSource"></param>
        ///// <param name="objTarget"></param>
        ///// <param name="objSource"></param>
        //public static void SwitchTypeByPropertyName(Type typeofTarget, Type typeofSource, object objTarget, object objSource)
        //{
        //    if (typeofTarget == null || typeofSource == null || objTarget == null || objSource == null) return;

        //    foreach (var pTarget in typeofTarget.GetProperties())
        //    {
        //        PropertyInfo pSource = typeofSource.GetProperty(pTarget.Name);
        //        if (pSource != null)
        //        {
        //            object pSourceValue = pSource.GetValue(objSource);
        //            pTarget.SetValue(objTarget, pSourceValue);
        //        }
        //    }
        //}

    }
}