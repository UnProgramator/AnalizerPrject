namespace DRSTool.CommonModels.Exceptions;

class EntityUsedButNotDeclaredException : Exception
{
    public EntityUsedButNotDeclaredException() : base() { }
    public EntityUsedButNotDeclaredException(string message) : base(message) { }
}
