namespace RuleAPI.Models
{
    public enum OperatorNum { GREATER_THAN, GREATER_THAN_OR_EQUAL, EQUAL, LESS_THAN, LESS_THAN_OR_EQUAL, NOT_EQUAL }
    public enum OperatorBool { EQUAL, NOT_EQUAL }
    public enum OperatorString { EQUAL, NOT_EQUAL, CONTAINS }
    public enum ErrorLevel { Error = 2, Warning = 1, Recommended = 0 }
    public enum Negation { MUST_HAVE, MUST_NOT_HAVE }
    public enum OccurrenceRule { ALL, ANY, NONE }
    public enum LogicalOperator { AND, OR, XOR }
    public enum PCType { BOOL, STRING, NUM }
}
