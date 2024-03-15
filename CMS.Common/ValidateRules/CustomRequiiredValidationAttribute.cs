using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.ValidateRules
{
    public class CustomRequiiredValidationAttribute : BaseValidateAtrribute
    {
        public CustomRequiiredValidationAttribute(string message) : base(message)
        {
        }

        public override (bool, string?) DoValidate(object oValue)
        {
            return oValue == null || oValue?.ToString()?.Length == 0 ? (false, Message) : (true, "");
        }
    }
}
