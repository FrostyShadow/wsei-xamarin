using System;
using System.Collections.Generic;
using Java.Util;

namespace AirMonitor.Models
{
    public class AveragedValues
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public IList<Value> Values { get; set; }
        public IList<Index> Indexes { get; set; }
        public IList<Standard> Standards { get; set; }
    }
}