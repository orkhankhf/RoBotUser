using Data.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;

namespace Services.Implementations
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<T> _repository;

        public GenericService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = _unitOfWork.GetRepository<T>(); // Assuming you have a method to get generic repository instances
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.FindAsync(predicate);
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.SingleOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(); // Ensure this method is implemented in your repository or handled in the UnitOfWork.
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync(); // Ensure this method is implemented in your repository or handled in the UnitOfWork.
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync(); // Ensure this method is implemented in your repository or handled in the UnitOfWork.
        }

        public async Task RemoveAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                _repository.Remove(entity);
                await _unitOfWork.SaveChangesAsync(); // Ensure this method is implemented in your repository or handled in the UnitOfWork.
            }
        }

        public async Task RemoveAsync(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(); // Ensure this method is implemented in your repository or handled in the UnitOfWork.
        }

        public async Task RemoveAllAsync()
        {
            _repository.RemoveAll();
            await _unitOfWork.SaveChangesAsync(); // Ensure this method is implemented in your repository or handled in the UnitOfWork.
        }
    }
}
