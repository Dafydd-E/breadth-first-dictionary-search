namespace Application.Searchers
{
    /// <summary>
    /// Defines the methods required to find a queue.
    /// </summary>
    /// <typeparam name="T">The type of the items that are being searched..</typeparam>
    public interface IPathFinder<T>
    {
        /// <summary>
        /// Discovers a path from the starting element to the finishing target.
        /// </summary>
        /// <param name="start">The strting item.</param>
        /// <param name="target">The finishing item.</param>
        /// <returns></returns>
        T FindPath(T start, T target);
    }
}
