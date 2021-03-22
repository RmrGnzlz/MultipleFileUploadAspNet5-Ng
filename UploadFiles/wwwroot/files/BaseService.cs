using Application.Models;
using Domain.Base;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Application.Base
{
    public abstract class BaseService<T> : IService<T> where T : BaseEntity
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IGenericRepository<T> _repository;
        public BaseService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public virtual IResponse<T> Add(T entity)
        {
            _repository.Add(entity);
            int result = _unitOfWork.Commit();
            return new Response<T>(
                mensaje: $"Registro guardado correctamente.{result}",
                entidad: entity
            );
        }
        public virtual IResponse<T> Update(T entity)
        {
            _repository.Edit(entity);
            _unitOfWork.Commit();
            return new Response<T>(
                mensaje: "Registro actualizado correctamente",
                entidad: entity
            );
        }

        public virtual IResponse<T> Get(Expression<Func<T, bool>> expression = null, string include = "", uint page = 0, uint size = 10)
        {
            return new Response<T>(
                mensaje: "Registros consultados",
                entidad: _repository.FindBy(expression, includeProperties: include).Skip((int)(page * size)).Take((int)size).ToList()
            );
        }

        public virtual T Find(int id)
        {
            return _repository.Find(id);
        }
    }
}
