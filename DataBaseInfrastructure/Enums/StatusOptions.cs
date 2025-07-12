using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseInfrastructure.Enums
{

    //todo при использовании Application их не возмжно исползьовать, но в dto они есть
    public enum StatusOptions
    {
        Pending,
        Denied,
        Approved,
        OnDeletion,
        Deleted
    }
}
