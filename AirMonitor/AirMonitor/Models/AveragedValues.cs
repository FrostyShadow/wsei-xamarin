using System;
using System.Collections.Generic;
using Java.Util;
using Newtonsoft.Json;
using SQLite;

namespace AirMonitor.Models
{
    public class AveragedValues
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public IList<Value> Values { get; set; }
        public IList<Index> Indexes { get; set; }
        public IList<Standard> Standards { get; set; }

        public AveragedValues()
        {
            
        }

        public AveragedValues(MeasurementItemEntity measurementItemEntity)
        {
            FromDateTime = measurementItemEntity.FromDateTime;
            TillDateTime = measurementItemEntity.TillDateTime;
            Values = JsonConvert.DeserializeObject<IList<Value>>(measurementItemEntity.Values);
            Indexes = JsonConvert.DeserializeObject<IList<Index>>(measurementItemEntity.Indexes);
            Standards = JsonConvert.DeserializeObject<IList<Standard>>(measurementItemEntity.Standards);
        }
    }
}