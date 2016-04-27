using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PsadWebsite.App_Code
{
    public enum EData : byte
    {
        Patients = 1, Operators, Organisations, Measurements
    }

    // Swapped for Data enum
    //public enum Groups : byte
    //{
    //    Patients, Operators, Organisations
    //}
    
    public enum EGender : byte
    {
        Male = 1, Female
    }

    public enum EStatus : byte
    {
        Active = 1, Inactive, Pending
    }

    
}