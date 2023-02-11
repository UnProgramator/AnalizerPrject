namespace DRSTool.Extractor.DataExtraction.Utils;

class FieldChecker
{
    public bool checkCondition(object value, string condition)
    {
        if (value == null)
            return false;
        if (value is string)
            return checkString((string)value, condition);
        if (value is int)
            return checkNumeric((int)value, condition);
        if (value is double)
            return checkNumeric((double)value, condition);

        return false;
    }

    protected bool checkNumeric<T>(T value, string condition)
    {
        return false;
    }

    protected bool checkString(string value, string condition)
    {

        return false;
    }


    protected const string EQ = "=";
    protected const string GT = ">";
    protected const string GE = ">=";
    protected const string LT = "<";
    protected const string LE = "<=";
    protected const string IN = "in";
    protected const string BE = "between";
    protected const string NO = "!";
}
