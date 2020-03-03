﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class DDLData
    {
        //This is the class used to load up our DropDownList
        public int valueField { get; set; }
        public string displayField { get; set; }

        public DDLData()
        {

        }

        public DDLData(int valuefield, string displayfield)
        {
            valueField = valuefield;
            displayField = displayfield;
        }
    }
}