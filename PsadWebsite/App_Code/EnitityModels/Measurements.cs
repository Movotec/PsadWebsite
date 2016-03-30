namespace PsadWebsite.App_Code.EnitityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Measurements
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Measurements()
        {
            MeasurementData = new HashSet<MeasurementData>();
        }

        public int RecID { get; set; }

        public Guid PsadGuid { get; set; }

        public Guid? PatientGuid { get; set; }

        public Guid? OperatorGuid { get; set; }

        [StringLength(50)]
        public string MeasureMode { get; set; }

        [StringLength(50)]
        public string Limb { get; set; }

        [StringLength(50)]
        public string Orientation { get; set; }

        public double? MeasureTime { get; set; }

        [StringLength(50)]
        public string Comport { get; set; }

        public double? FibulaLength { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment { get; set; }

        public int? FirmWare1 { get; set; }

        public int? FirmWare2 { get; set; }

        public int? FirmWare3 { get; set; }

        public DateTime MeasureDateTime { get; set; }

        [Key]
        public Guid MeasureGuid { get; set; }

        public double? MasterLength1 { get; set; }

        public double? MasterLength2 { get; set; }

        public double? StartAngle { get; set; }

        public double? MinAngle { get; set; }

        public double? MaxAngle { get; set; }

        public double? Rom { get; set; }

        public double? MaxAngularVelocity { get; set; }

        public double? MinAngularVelocity { get; set; }

        public double? MaxAcceleration { get; set; }

        public double? MinForce { get; set; }

        public double? MaxForce { get; set; }

        public double? MinXForce { get; set; }

        public double? MaxXForce { get; set; }

        public double? MinYForce { get; set; }

        public double? MaxYForce { get; set; }

        public double? Stiffness1 { get; set; }

        public double? Stiffness2 { get; set; }

        public double? Stiffness3 { get; set; }

        public double? Stiffness4 { get; set; }

        public double? Stiffness5 { get; set; }

        public double? EMG1Activity { get; set; }

        public double? EMG2Activity { get; set; }

        public bool? Batt0 { get; set; }

        public bool? Batt1 { get; set; }

        public bool? Batt2 { get; set; }

        public bool? Led0 { get; set; }

        public bool? Led1 { get; set; }

        public bool? Led2 { get; set; }

        public bool? Led3 { get; set; }

        public bool? Led4 { get; set; }

        public double? Acc1XOffset { get; set; }

        public double? Acc1XGain { get; set; }

        public double? Acc1YOffset { get; set; }

        public double? Acc1YGain { get; set; }

        public double? Acc1ZOffset { get; set; }

        public double? Acc1ZGain { get; set; }

        public double? Acc2XOffset { get; set; }

        public double? Acc2XGain { get; set; }

        public double? Acc2YOffset { get; set; }

        public double? Acc2YGain { get; set; }

        public double? Acc2ZOffset { get; set; }

        public double? Acc2ZGain { get; set; }

        public double? GyroXOffset { get; set; }

        public double? GyroXGain { get; set; }

        public double? GyroYOffset { get; set; }

        public double? GyroYGain { get; set; }

        public double? GyroZOffset { get; set; }

        public double? GyroZGain { get; set; }

        public double? Emg1Offset { get; set; }

        public double? Emg1Gain { get; set; }

        public double? Emg2Offset { get; set; }

        public double? Emg2Gain { get; set; }

        public double? Straingauge1Offset { get; set; }

        public double? Straingauge1Gain { get; set; }

        public double? Straingauge2Offset { get; set; }

        public double? Straingauge2Gain { get; set; }

        public double? Straingauge3Offset { get; set; }

        public double? Straingauge3Gain { get; set; }

        public double? Straingauge4Offset { get; set; }

        public double? Straingauge4Gain { get; set; }

        public double? PsadWeight { get; set; }

        public double? PsadAcc12 { get; set; }

        public double? PsadAcc1h { get; set; }

        public double? PsadAcc2h { get; set; }

        public double? Psadh { get; set; }

        public double? AccGRange { get; set; }

        public double? GyroDegreePrSec { get; set; }

        public double? SPS { get; set; }

        public int? Emg1Pad { get; set; }

        public int? Emg2Pad { get; set; }

        public int? BatteryLow { get; set; }

        public int? BatteryHigh { get; set; }

        public DateTime? LastCalibrationDateAndTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeasurementData> MeasurementData { get; set; }
    }
}
