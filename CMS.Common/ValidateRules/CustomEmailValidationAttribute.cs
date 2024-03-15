using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CMS.Common.ValidateRules
{
    public class CustomEmailValidationAttribute : BaseValidateAtrribute
    {
        public CustomEmailValidationAttribute(string message) : base(message)
        {
        }
        public override (bool, string?) DoValidate(object oValue)
        {
            if (oValue == null)
            {
                return (false, "A Email is needed here.");
            }
            string reg = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            string value = oValue.ToString();
            Regex regex = new Regex(reg);
            return regex.IsMatch(value) ? (true, "") : (false, Message);
        }
    }
}
