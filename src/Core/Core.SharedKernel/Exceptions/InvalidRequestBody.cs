using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.Exceptions;
public class InvalidRequestBodyException : Exception
{
    public required string[] Errors { get; set; }
}
