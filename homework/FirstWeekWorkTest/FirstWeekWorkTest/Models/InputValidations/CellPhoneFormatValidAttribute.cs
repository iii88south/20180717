using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FirstWeekWorkTest.Models.InputValidations
{
    public class CellPhoneFormatValidAttribute:DataTypeAttribute
    {
        public CellPhoneFormatValidAttribute():base(DataType.Text)
        {
            ErrorMessage = "請輸入正確的電話號碼格式(ex:09xx-xxxxxx)";
        }

        // Over Ride Valid
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            string str = (string)value;
            if (str == String.Empty)
            {
                return true;
            }
            return isFormatCorrect(str);
        }

        public bool isFormatCorrect(string inputCellPhone)
        {          
            string pattern = @"[0-9]{4}\-[0-9]{6}"; // 規則字串
            var result=Regex.IsMatch(inputCellPhone, pattern);
            return result;
        }

    }
}