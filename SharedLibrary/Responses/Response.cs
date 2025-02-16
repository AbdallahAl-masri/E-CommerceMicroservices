using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Responses
{
    public class Response
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = null!;
    }
}
