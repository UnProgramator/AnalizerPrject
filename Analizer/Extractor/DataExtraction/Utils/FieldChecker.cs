using Newtonsoft.Json.Serialization;

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
        //if (value is double)
        //    return checkNumeric((double)value, condition);

        return false;
    }

    protected bool checkNumeric(int value, string condition)
    { 
        bool negate = false;
        bool compRez = false;

        (var cond, var ops) = getOps(condition);

        if (cond.StartsWith(NO))
        {
            negate = true;
            cond = cond.Substring(1);
        }

        if (cond.Equals(BE))
        {
            if(ops is not Array)
                throw new Exception();
            int left, right;
            left = int.Parse(((string[])ops)[0]);
            right = int.Parse(((string[])ops)[1]);
            compRez = value >= left && value <= right;
        }
        else if (cond.Equals(IN))
        {
            if (ops is not string[])
                throw new Exception();
            foreach (var op in (string[])ops)
            {
                if (value.Equals(int.Parse(op)))
                {
                    compRez = true;
                    break;
                }
            }
        }
        else
        {

            int op = int.Parse((string)ops);
            switch (cond)
            {
                case EQ:
                    compRez = op == value;
                    break;
                case GT:
                    compRez = value > op;
                    break;
                case GE:
                    compRez = value >= op;
                    break;
                case LT:
                    compRez = value < op;
                    break;
                case LE:
                    compRez = value <= op;
                    break;
            }
        }

        return negate ^ compRez;
    }

    protected bool checkString(string value, string condition)
    {
        bool negate = false;
        bool compRez = false;

        (var cond, var ops) = getOps(condition);

        if (cond.StartsWith(NO))
        {
            negate = true;
            cond = cond.Substring(1);
        }

        if (cond.Equals(EQ))
        {
            if (ops is string)
                compRez = value.Equals(ops);
            else
                throw new Exception($"For Equal comparison (\"=\") the operator must be a string, not an {ops.GetType()}");
        }
        else if (cond.Equals(IN))
        {
            if(ops is string[])
            {
                foreach(var op in (string[])ops)
                {
                    if (op.Equals(value))
                    {
                        compRez = true;
                        break;
                    }
                }
            }
        }
        else
            throw new Exception($"Operator{cond} cannot be aplied on string");

        return negate ^ compRez;
    }

    protected (string,object) getOps(string conditions)
    {
        string operations = "";
        object operands = "";


        operations = conditions.Substring(0, conditions.IndexOf(' ')).Trim();

        switch (operations)
        {
            case IN:
            case NO + IN:
            case BE:
            case NO + BE:
                var stIdx = conditions.IndexOf('(') + 1; //starting index of the operands, next index after "("
                var len = conditions.LastIndexOf(')') - stIdx; // the length between the two "()", without the parathesis
                var opsL = conditions.Substring(stIdx, len).Trim();
                operands = opsL.Split(',').Select(e => e.Trim()).ToArray(); //split then trim
                break;
            default:
                operands = conditions.Substring(conditions.IndexOf(' ')).Trim();
                break;
        }

        return (operations, operands);
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
