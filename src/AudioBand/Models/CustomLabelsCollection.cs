using System.Collections.Generic;

namespace AudioBand.Models
{
    internal class CustomLabelsCollection : ModelBase
    {
        public List<CustomLabel> CustomLabels { get; set; } = new List<CustomLabel>{new CustomLabel()};
    }
}
