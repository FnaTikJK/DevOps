namespace DevOpsAPI;

public class PostResponse<T>
{
    public bool IsCreated { get; set; }
    public T Item { get; set; }
}