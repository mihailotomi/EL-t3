namespace EL_t3.Core.Exceptions;

public class EntityNotFoundException : Exception
{
    private string _message;

    public EntityNotFoundException(string name, object identifier)
    {
        this._message = String.Format("Entity {0} is not found with identifier {1}", name, identifier.ToString());
    }

    public override string Message
    {
        get
        {
            return _message;
        }
    }
}
