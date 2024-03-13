using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.ValidateRules
{
    public class CustomRequiiredValidation : BaseValidateAtrribute
    {
        public CustomRequiiredValidation(string message) : base(message)
        {
        }

        public override (bool, string?) DoValidate(object oValue)
        {
            return oValue == null ? (false, Message) : (true, "");
        }
    }
}
