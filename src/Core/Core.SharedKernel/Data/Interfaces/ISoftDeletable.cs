using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.Data.Interfaces;

public interface ISoftDeletable
{
    public string? DeletedBy { get; }

    public DateTime? DeletedAt { get; }
}
