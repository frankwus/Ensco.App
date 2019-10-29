using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensco.Irma.Data.Enums
{
    public class CAPAStatus
    {
        private readonly string value;

        private CAPAStatus(string value)
        {
            this.value = value;
        }
        public string Value { get; set; }


        public static CAPAStatus Open => new CAPAStatus("Open");
        public static CAPAStatus PendingImplementation => new CAPAStatus("Pending Implementation");
        public static CAPAStatus PendingReview => new CAPAStatus("Pending Review");
        public static CAPAStatus PendingVerification => new CAPAStatus("Pending Verification");
        public static CAPAStatus Closed => new CAPAStatus("Closed");


        public override string ToString()
        {
            return Value;
        }
    }
}
