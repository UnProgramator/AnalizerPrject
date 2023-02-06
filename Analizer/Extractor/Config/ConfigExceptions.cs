namespace DRSTool.Extractor.Config;

class InvalidConfigException: Exception
{
    public InvalidConfigException(string msg) : base(msg) { }
    public InvalidConfigException() : base() { }
}

class InvalidConfigOutputException : Exception
{
    public InvalidConfigOutputException(string msg) : base(msg) { }
    public InvalidConfigOutputException() : base() { }
}

class InvalidConfigInputException : Exception
{
    public InvalidConfigInputException(string msg) : base(msg) { }
    public InvalidConfigInputException() : base() { }
}

class InvalidConfigDefaultInputException : Exception
{
    public InvalidConfigDefaultInputException(string msg) : base(msg) { }
    public InvalidConfigDefaultInputException() : base() { }
}
