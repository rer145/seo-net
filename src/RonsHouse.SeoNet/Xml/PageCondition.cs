using System;

namespace RonsHouse.SeoNet.Xml
{
    public class PageCondition
    {
        public string ParameterName { get; set; }
        public ConditionOperator Operator { get; set; }
        public string Value { get; set; }
        public ConditionParameterType ParameterType { get; set; }

        public SeoValues SeoValues { get; set; }

        public PageCondition()
        {
            this.SeoValues = new SeoValues();
        }
    }
}