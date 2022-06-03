using SearchingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SearchingLibrary.Extensions {
    public static class EnumerableExtensions {
        public static IEnumerable<T> SearchBy<T, TId>(this IEnumerable<T> enumerable) where T : AbstractModel<TId> { 

            return enumerable;
        }
    }
}
