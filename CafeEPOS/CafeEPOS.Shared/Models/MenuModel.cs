﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CafeEPOS.Shared.Models
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public decimal price { get; set; }
        public int archived { get; set; }
    }
}
