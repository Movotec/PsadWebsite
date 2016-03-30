namespace PsadWebsite.App_Code.EnitityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MeasurementData")]
    public partial class MeasurementData
    {
        public int ID { get; set; }

        public Guid? MeasurementID { get; set; }

        public int? Switches { get; set; }

        public double? Clock { get; set; }

        public double? Acc1X { get; set; }

        public double? Acc1Y { get; set; }

        public double? Acc1Z { get; set; }

        public double? Acc2X { get; set; }

        public double? Acc2Y { get; set; }

        public double? Acc2Z { get; set; }

        public double? GyroX { get; set; }

        public double? GyroY { get; set; }

        public double? GyroZ { get; set; }

        public double? Emg1 { get; set; }

        public double? Emg2 { get; set; }

        public double? Straingauge1 { get; set; }

        public double? Straingauge2 { get; set; }

        public double? Straingauge3 { get; set; }

        public double? Straingauge4 { get; set; }

        public int? Error { get; set; }

        public virtual Measurements Measurements { get; set; }
    }
}
