using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NumberConverter : MonoBehaviour
{
    static string[] units = {"", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sep", "Oct", "Non", "Dec",
        "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n",
        "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
        "aa", "bb", "cc", "dd", "ee", "ff", "gg", "hh", "ii", "jj", "kk", "ll", "mm", "nn",
        "oo", "pp", "qq", "rr", "ss", "tt", "uu", "vv", "ww", "xx", "yy", "zz"};

    #region Operation
    public static string minusBtwUnit(string having, string minusVal)
    {
        int havingIdx;
        int minusValIdx;

        string tmpStr = Regex.Replace(having, "[^a-zA-Z]", "");
        if(!tmpStr.Equals(""))
            having = having.Replace(tmpStr, "");
        havingIdx = FindIdx(tmpStr);

        tmpStr = Regex.Replace(minusVal, "[^a-zA-Z]", "");
        if(!tmpStr.Equals(""))
            minusVal = minusVal.Replace(tmpStr, "");
        minusValIdx = FindIdx(tmpStr);

        double d1 = double.Parse(having);
        double d2 = double.Parse(minusVal);
        double result = 0;

        if(havingIdx > minusValIdx)
        {
            result = (d1 * Mathf.Pow(1000, havingIdx - minusValIdx)) - d2;
        }
        else if(havingIdx == minusValIdx)
        {
            if(d1 < d2)
            {
                // minus 불가
                result = -1;
            }
            else
                result = d1 - d2;
        }
        else
        {
            // minus 불가
            result = -1;
        }

        if (result < 0)
            return "";

        return numTounit(result, units[minusValIdx]);
    }
    public static string plusBtwUnit(string having, string plusVal)
    {
        int havingIdx;
        int plusValIdx;

        string tmpStr = Regex.Replace(having, "[^a-zA-Z]", "");
        if (!tmpStr.Equals(""))
            having = having.Replace(tmpStr, "");
        havingIdx = FindIdx(tmpStr);

        tmpStr = Regex.Replace(plusVal, "[^a-zA-Z]", "");
        if (!tmpStr.Equals(""))
            plusVal = plusVal.Replace(tmpStr, "");
        plusValIdx = FindIdx(tmpStr);

        double d1 = double.Parse(having);
        double d2 = double.Parse(plusVal);
        double result = 0;

        if (havingIdx > plusValIdx)
        {
            result = (d1 * Mathf.Pow(1000, havingIdx - plusValIdx)) + d2;
            return numTounit(result, units[plusValIdx]);
        }
        else if (havingIdx == plusValIdx)
        {
            result = d1 + d2;
            return numTounit(result, units[havingIdx]);
        }
        else
        {
            result = d1 + (d2 * Mathf.Pow(1000, plusValIdx - havingIdx));
            return numTounit(result, units[havingIdx]);
        }
    }
    public static string multipleBtwUnit(string having, float multipleVal)
    {
        int havingIdx;

        string tmpStr = Regex.Replace(having, "[^a-zA-Z]", "");
        if (!tmpStr.Equals(""))
            having = having.Replace(tmpStr, "");
        havingIdx = FindIdx(tmpStr);

        double d1 = double.Parse(having);
        double result = d1 * multipleVal;
        if (result < 1)
        {
            // idx 줄이는 과정.. while문 돌려서... 1보다 커질 때까지..
            while (result < 1)
            {
                result *= 1000;
                havingIdx--;
                if (havingIdx == 0)
                    break;
            }
        }
        if (result < 1) // 결과 자체가 원래 1보다 작은 경우. 0으로 감안한다.
        {
            return numTounit(0, units[0]);
        }

        return numTounit(result, units[havingIdx]);
    }
    #endregion

    public static string numTounit(double num, string critunit)
    {
        if (num == 0) return "0";

        int unitIdx = FindIdx(critunit);
        while (true)
        {
            if (num / 1000 < 1) break;
            num = num / 1000;
            unitIdx++;
        }
        if (unitIdx == 0)
            num = (int)num;

        return num + units[unitIdx];
    }

    // 화면에 깔끔하게 나타낼 수 있도록, 소수점 2번째 자리 이하 수를 지워 return해주는 함수
    public static string ToDisplayNum(string num)
    {
        string tmpStr = Regex.Replace(num, "[^a-zA-Z]", "");
        if (!tmpStr.Equals(""))
            num = num.Replace(tmpStr, "");

        double d1 = double.Parse(num);
        string returnVal;
        if (tmpStr.Equals(""))
            returnVal = string.Format("{0:0.#}", d1);
        else
            returnVal = string.Format("{0:f2}", d1) + tmpStr;
        return returnVal;
    }

    static int FindIdx(string unit)
    {
        for(int i = 0; i < units.Length; i++)
        {
            if (unit.Equals(units[i]))
                return i;
        }
        return 0;
    }
}
