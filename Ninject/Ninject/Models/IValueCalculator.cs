using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninject.Models
{
    interface IValueCalculator
    {
        decimal ValueProducts (IEnumerable<Product> products)
    }
}
