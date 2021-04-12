namespace DbmsApi.API
{
    public enum PropertyType { BOOL, STRING, NUM }

    public abstract class Property
    {
        public string Name { get; private set; }
        public PropertyType Type { get; set; }

        public Property(string name)
        {
            Name = name;
        }

        public string GetValueString()
        {
            string returnString = "";
            if (this.GetType() == typeof(PropertyString))
            {
                returnString = (this as PropertyString).Value;
            }
            if (this.GetType() == typeof(PropertyBool))
            {
                returnString = (this as PropertyBool).Value.ToString();
            }
            if (this.GetType() == typeof(PropertyNum))
            {
                returnString = (this as PropertyNum).Value.ToString();
            }
            return returnString;
        }

        public string String(int tabCount = 0)
        {
            string tabs = "";
            for (int i = 0; i < tabCount; i++)
            {
                tabs += "\t";
            }
            return Name + ": " + tabs + GetValueString();
        }
    }
    public class PropertyString : Property
    {
        public string Value { get; private set; }

        public PropertyString(string name, string value) : base(name)
        {
            Value = value;
            Type = PropertyType.STRING;
        }
    }
    public class PropertyBool : Property
    {
        public bool Value { get; private set; }

        public PropertyBool(string name, bool value) : base(name)
        {
            Value = value;
            Type = PropertyType.BOOL;
        }
    }
    public class PropertyNum : Property
    {
        public double Value { get; private set; }

        public PropertyNum(string name, double value) : base(name)
        {
            Value = value;
            Type = PropertyType.NUM;
        }
    }
}