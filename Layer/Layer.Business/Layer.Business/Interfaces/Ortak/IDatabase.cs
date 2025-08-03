using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Layer.Data;

namespace Layer.Business.Interfaces.Ortak
{
    public interface IDatabase
    {
        DbTeknoKurEntities Context { get; set; }
    }
}
