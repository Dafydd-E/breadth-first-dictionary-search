namespace Application.Writers
{
    /// <summary>
    /// Defines the methods required for a class to be a writer.
    /// </summary>
    /// <typeparam name="T">The type of the item to be written.</typeparam>
    public interface IWriter<T>
    {
        /// <summary>
        /// Writes the specified item to the target location.
        /// </summary>
        /// <param name="item">The item to write to the target.</param>
        void Write(T item);
    }
}
