namespace WebStore.Domain
{
    public enum RepositoryResult: byte
    {
        Success = 0,
        Failed = 1,
        NotFound = 2,
    }
}
