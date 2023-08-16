using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SharedKernel.Entities;
public abstract class BaseEntity
{
    public virtual int Id { get; set; }
    public virtual DateTime Created { get; set; }
}
