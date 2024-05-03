using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.DAL.Repositories
{
    /// <summary>
    /// Интерфейс для объявления шаблона Repository.
    /// </summary>
    /// <typeparam name="Model"></typeparam>
    public interface IRepository<Model>  where Model : ModelBase
    {
        /// <summary>
        /// Возвращает элемент по <paramref name="id"/> элемента.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Возвращает элемент типа <typeparamref name="Model"/>.</returns>
        public Model Get(Guid id);

        /// <summary>
        /// Возвращает все элементы.
        /// </summary>
        /// <returns>Возвращает список элементов типа <typeparamref name="Model"/>.</returns>
        public IEnumerable<Model> Get()
        {
            return Get(x => true);
        }

        /// <summary>
        /// Возвращает элементы согласно условию.
        /// </summary>
        /// <param name="predicate">Условие.</param>
        /// <returns>Возвращает список элементов типа <typeparamref name="Model"/>, согласно условию.</returns>
        public IEnumerable<Model> Get(Func<Model, bool> predicate);

        /// <summary>
        /// Метод для создания элемента.
        /// </summary>
        /// <param name="item">Создаваемый элемент.</param>
        public void Create(Model item);

        /// <summary>
        /// Метод для обновления элемента.
        /// </summary>
        /// <param name="item">Обновляемый элемент.</param>
        public void Update(Model item);

        /// <summary>
        /// Метод для удаления по <paramref name="id"/> элемента.
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);
    }
}
