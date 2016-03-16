using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsadWebsite.App_Code.Repository
{
    public class PsadMeasureResult
    {
        public DateTime MeasureDateTime;
        public Guid MeasureGuid;
        public string Comments = String.Empty;
        public double StartAngle;
        public double MinAngle;
        public double MaxAngle;
        public double Rom;
        public double MaxAngularVelocity;
        public double MinAngularVelocity;
        public double MaxAcceleration;
        public double MinForce;
        public double MaxForce;
        public double MinXForce;
        public double MaxXForce;
        public double MinYForce;
        public double MaxYForce;
        public double Stiffness1; //Stiffness Parameters??
        public double Stiffness2;
        public double Stiffness3;
        public double Stiffness4;
        public double Stiffness5;
        public bool Batt0;
        public bool Batt1;
        public bool Batt2;
        public bool Led0;
        public bool Led1;
        public bool Led2;
        public bool Led3;
        public bool Led4;
    }
}