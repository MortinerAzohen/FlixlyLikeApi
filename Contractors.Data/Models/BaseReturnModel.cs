using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contractors.Data.Models
{
    public class BaseReturnModel<T>
    {
        public string ErrorMessage { get; set; }
        public string AdditionalInformation { get; set; }
        public bool IsCorrect { get; set; }
        public T Model { get; set; }

    }
}
