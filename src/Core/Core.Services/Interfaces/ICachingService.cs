using Core.SharedKernel.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces;
public interface ICachingService
{
    T? GetItem<T>(string cacheKey);
    T SetItem<T>(string cacheKey, T item);
}
