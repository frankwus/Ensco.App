using Ensco.Irma.Oap.Common.Extensions;

namespace Ensco.App.Areas.Oap.Models
{
    public class KeyValueModel
    {
        /// <summary>
        /// Names of fields are important as these are used in javacript to get the comment boxes
        /// </summary>
        public string Id { get; set; }

        // <summary>
        /// Names of fields are important as these are used in javacript to get the comment boxes
        /// </summary>
        public string Value { get; set; } 

        public string DisplayValue { get; set; }

        public KeyValueModel()
        {

        }

        public KeyValueModel(string id, string value, string displayValue = "")
        {
            Id = id;
            Value = value;
            DisplayValue = (string.IsNullOrEmpty(displayValue)?Id: displayValue).Translate();
        }
    }
}