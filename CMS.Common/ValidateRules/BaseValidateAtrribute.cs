using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Common.ValidateRules
{
    public abstract class BaseValidateAtrribute : Attribute
    {
        protected string Message { get; set; }

        public BaseValidateAtrribute(string message)
        {
            Message = message;
        }

        public abstract (bool, string?) DoValidate(object oValue);
    }
}
