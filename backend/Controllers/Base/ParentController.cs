using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.DAL.Repository.Base;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DEMOAPP.Controllers
{
    public abstract class ParentController<TModel, TRepository> : ControllerBase where TModel:class where TRepository: IRepository<TModel>
    {
        protected readonly TRepository Repository;

        public ParentController(TRepository repository)
        {
            this.Repository = repository;
        }

        [Route("[action]")]
        [HttpGet]
        public IEnumerable<TModel> GetAll()
        {
            return Repository.GetAll();
        }

        [Route("[action]")]
        [HttpPost]
        public void Add([FromBody] TModel item)
        {
            Repository.Add(item);
        }

    }
}