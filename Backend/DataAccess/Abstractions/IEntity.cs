using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstractions
{
    public interface IEntity<TKey> where TKey : IComparable<TKey>
    {
        public TKey Id { get; }
    }
}
