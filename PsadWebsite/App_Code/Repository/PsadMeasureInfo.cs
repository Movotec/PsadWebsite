using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsadWebsite.App_Code.Repository
{
    public class PsadMeasureInfo
    {
        public string RecID = "0";
        public Guid PsadGuid;
        public Guid PatientGuid;
        public Guid OperatorGuid;
        public Guid OrganisationGuid;

        public string PatientName;
        public string OperatorName;
        public string OrganisationName; // TBD From file or Firmware (PsadFWMovotecData)
        public string OperatorType; // Normal Advanced, Administrator

        public string MeasureMode = "Not set";   // Active Passive, ..
        public string Limb = "Not set";          // Ankle, Wrist...
        public string Orientation = "Not set";   // Left, Right
        public string MeasureTime = "10";
        public string Comport = "Com?";
    }
}