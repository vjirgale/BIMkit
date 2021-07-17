using DbmsApi.API;
using MathPackage;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace RuleAPI.Models
{
    [BsonKnownTypes(typeof(PropertyCheckBool), typeof(PropertyCheckString), typeof(PropertyCheckNum))]
    public abstract class PropertyCheck
    {
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PCType PCType { get; set; }

        public PropertyCheck(string name)
        {
            Name = name;
        }

        public abstract string String();
        public abstract PropertyCheck Copy();
    }
    public class PropertyCheckBool : PropertyCheck
    {
        public bool Value { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OperatorBool Operation { get; set; }
        public PropertyCheckBool(string name, OperatorBool operation, bool value) : base(name)
        {
            PCType = PCType.BOOL;
            Value = value;
            Operation = operation;
        }

        public double CheckProperty(PropertyBool property)
        {
            if (property.Name == Name)
            {
                if (Operation == OperatorBool.EQUAL)
                {
                    return property.Value == Value ? 1.0 : 0.0;
                }
                else
                {
                    return property.Value != Value ? 1.0 : 0.0;
                }
            }
            return 0.0;
        }

        public override string String()
        {
            return Name + " " + Operation + " " + Value;
        }

        public override PropertyCheck Copy()
        {
            return new PropertyCheckBool(this.Name, this.Operation, this.Value);
        }
    }
    public class PropertyCheckString : PropertyCheck
    {
        public string Value { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public OperatorString Operation { get; set; }
        public PropertyCheckString(string name, OperatorString operation, string value) : base(name)
        {
            PCType = PCType.STRING;
            Value = value;
            Operation = operation;
        }

        public double CheckProperty(PropertyString property)
        {
            if (property.Name == Name)
            {
                if (Operation == OperatorString.EQUAL)
                {
                    return property.Value == Value ? 1.0 : 0.0;
                }
                if (Operation == OperatorString.NOT_EQUAL)
                {
                    return property.Value != Value ? 1.0 : 0.0;
                }
                if (Operation == OperatorString.CONTAINS)
                {
                    return property.Value.ToUpper().Contains(Value.ToUpper()) ? 1.0 : 0.0;
                }
            }
            return 0.0;
        }

        public override string String()
        {
            return Name + " " + Operation + " " + Value;
        }

        public override PropertyCheck Copy()
        {
            return new PropertyCheckString(this.Name, this.Operation, this.Value);
        }
    }
    public class PropertyCheckNum : PropertyCheck
    {
        public static double ALPHA = 2.0;
        double roundingHelper = 0.00001;

        [JsonConverter(typeof(StringEnumConverter))]
        public OperatorNum Operation { get; set; }
        public double Value { get; private set; }
        public double ValueInStandardUnit { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Unit ValueUnit { get; private set; }
        public PropertyCheckNum(string name, OperatorNum operation, double value, Unit valueUnit) : base(name)
        {
            PCType = PCType.NUM;
            Operation = operation;
            Value = value;
            ValueUnit = valueUnit;
            ValueInStandardUnit = Utils.ChangeUnitToMeterOrDeg(Value, ValueUnit);
        }

        public void SetNewValue(double value)
        {
            Value = value;
            ValueInStandardUnit = Utils.ChangeUnitToMeterOrDeg(Value, ValueUnit);
        }

        public void SetNewUnit(Unit unit)
        {
            ValueUnit = unit;
            ValueInStandardUnit = Utils.ChangeUnitToMeterOrDeg(Value, ValueUnit);
        }

        public double CheckProperty(PropertyNum property)
        {
            if (property.Name == Name)
            {
                double val = (ValueInStandardUnit == 0.0) ? roundingHelper : ValueInStandardUnit;
                switch (Operation)
                {
                    case (OperatorNum.EQUAL):
                        if (property.Value < val)
                        {
                            return Math.Pow((property.Value / val), ALPHA);
                        }
                        if (property.Value > val)
                        {
                            return Math.Pow((val / property.Value), ALPHA);
                        }
                        return 1.0;
                    case (OperatorNum.GREATER_THAN):
                        if (property.Value <= val)
                        {
                            return Math.Pow((property.Value / val), ALPHA);
                        }
                        return 1.0;
                    case (OperatorNum.GREATER_THAN_OR_EQUAL):
                        if (property.Value < val)
                        {
                            return Math.Pow((property.Value / val), ALPHA);
                        }
                        return 1.0;
                    case (OperatorNum.LESS_THAN):
                        if (property.Value >= val)
                        {
                            return Math.Pow((val / property.Value), ALPHA);
                        }
                        return 1.0;
                    case (OperatorNum.LESS_THAN_OR_EQUAL):
                        if (property.Value > val)
                        {
                            return Math.Pow((val / property.Value), ALPHA);
                        }
                        return 1.0;
                    case (OperatorNum.NOT_EQUAL):
                        return property.Value != val ? 1.0 : 0.0;
                }
            }
            return 0.0;
        }

        public override string String()
        {
            //return Name + " " + Operation + " " + ValueMeter.ToString("0.##") + "M";
            return Name + " " + Operation + " " + Value.ToString("0.##") + ValueUnit;
        }

        public override PropertyCheck Copy()
        {
            return new PropertyCheckNum(this.Name, this.Operation, this.Value, this.ValueUnit);
        }
    }
}