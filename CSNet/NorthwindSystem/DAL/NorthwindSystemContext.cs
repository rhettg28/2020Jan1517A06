﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using NorthwindSystem.Entities;
using System.Data.Entity;
#endregion

namespace NorthwindSystem.DAL
{
    //restrict access to this class to ONLY other classes within this library : access
    //  privilidge of internal
    //security measure
    
    //  connect to EntityFrameWork by inheriting DbContext
    internal class NorthwindSystemContext:DbContext
    {
        //you will need a constructor that passes the connection string name to EntityFramework
        //  via the inherited class DbContext
        public NorthwindSystemContext():base("NWDB")
        {

        }

        //properties to interact with EntityFramework these properties represent the whole table
        //  and all data of the SQL database
        public DbSet<Product> Products { get; set; }
    }
}
