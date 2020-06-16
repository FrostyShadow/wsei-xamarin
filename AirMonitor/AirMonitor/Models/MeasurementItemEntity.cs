using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;

namespace AirMonitor.Models
{
    public class MeasurementItemEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public string Values { get; set; }
        public string Indexes { get; set; }
        public string Standards { get; set; }

        public MeasurementItemEntity()
        {
            
        }

        public MeasurementItemEntity(AveragedValues averagedValues)
        {
            FromDateTime = averagedValues.FromDateTime;
            TillDateTime = averagedValues.TillDateTime;
            Values = JsonConvert.SerializeObject(averagedValues.Values);
            Indexes = JsonConvert.SerializeObject(averagedValues.Indexes);
            Standards = JsonConvert.SerializeObject(averagedValues.Standards);
        }
    }
}