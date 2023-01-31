using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analizer.FileHelper
{
    class UnsuportedFileTypeException: Exception
    {
        public UnsuportedFileTypeException(string fileType) : base("Files of type \"" + fileType + "\" not accepted at the tyme") { }
        public UnsuportedFileTypeException() : base() { }
    }
}
