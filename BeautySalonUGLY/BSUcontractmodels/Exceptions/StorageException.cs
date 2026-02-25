public class StorageException : Exception
{
    public StorageException(Exception ex) :
        base($"< ! > Error while operation goes in storage: {ex.Message}", ex) { } 
}
