using System;

namespace RonsHouse.SeoNet
{
    public enum ConditionOperator
    {
        Equals,
		NotEquals
    }

    public enum ConditionParameterType
    {
        QueryString,
        Session,
        Cache
    }
}
