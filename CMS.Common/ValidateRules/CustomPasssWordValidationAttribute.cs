using CMS.Common.ValidateRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Zhaoxi.Manage.Common.ValidateRules
{
    public class CustomPasssWordValidationAttribute : BaseValidateAtrribute
    {
        public CustomPasssWordValidationAttribute(string message) : base(message)
        {

        }

        public override (bool, string?) DoValidate(object oValue)
        {
            if (oValue == null)
            {
                return (false, "A validate password is needed here.");
            }
            string reg = @"^\d+$";
            string value = oValue.ToString();
            Regex regex = new Regex(reg);
            return regex.IsMatch(value) ? (true, "") : (false, Message);
        }
    }
}
