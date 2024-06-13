using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using WexAssessmentApi.Interfaces;

namespace WexAssessmentApi.Data
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        private readonly ConcurrentDictionary<int, T> _repository = new ConcurrentDictionary<int, T>();

        /// <summary>
        /// Gets all objects in the repository.
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(this._repository.Values.AsEnumerable());
        }

        /// <summary>
        /// Gets object of type T from the repository by key. 
        /// is thrown.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">When no object is found, it throws InvalidOperationException.</exception>
        public Task<T> GetByIdAsync(int id)
        {
            if (this._repository.TryGetValue(id, out T? result))
            {
                return Task.FromResult(result);
            }
            else
            {
                throw new InvalidOperationException($"No object with key {id} found");
            }
        }

        /// <summary>
        /// Add an object into the repository.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">When an object with key is found, it throws InvalidOperationException.</exception>
        public Task AddAsync(T entity)
        {
            return Task.Run(() =>
            {
                int key = PreAddObject(entity);
                if (this._repository.ContainsKey(key))
                {
                    throw new InvalidOperationException($"Object with key {key} already exists");
                }
                else
                {
                    this._repository[key] = entity;
                }
            });
        }

        /// <summary>
        /// Updates object in the repository based on entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public abstract Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes object with key value of id from the repository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If no object with key id is found, it throws InvalidOperationException.</exception>
        public Task DeleteAsync(int id)
        {
            return Task.Run(() =>
            {
                if (this._repository.Remove(id, out T? result) == false)
                {
                    throw new InvalidOperationException($"No object with key {id} found");
                }
            });
        }

        protected abstract int PreAddObject(T entity);
    }
}
