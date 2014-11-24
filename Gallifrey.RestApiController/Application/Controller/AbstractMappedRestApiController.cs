using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Gallifrey.Persistence.Application.Persistence;
using Gallifrey.RestApi.Application.Domain.Model;
using Gallifrey.RestApi.Application.Extension;
using Gallifrey.RestApi.Application.Validation;

namespace Gallifrey.RestApi.Application.Controller
{
    public abstract class AbstractMappedRestApiController<TModel, TIdentityType, TRepository, TMappedModel> :
        ApiController
        where TRepository : IDatabaseRepository<TModel, TIdentityType>
        where TModel : class
        where TMappedModel : class
    {
        private readonly TRepository _respository;

        private readonly ValidationStrategyFactory<TMappedModel> _validation;

        protected AbstractMappedRestApiController(TRepository respository,
            IEnumerable<IValidationStrategy> validationStrategies)
        {
            _respository = respository;
            //Usefull for serialization of REST API
            _respository.DisableProxyAndLazyLoading();

            _validation =
                new ValidationStrategyFactory<TMappedModel>(error => ModelState.AddModelError("Validation", error),
                    validationStrategies);
        }

        private void ValidateWithStrategy(TMappedModel model)
        {
            _validation.Validate(model);
        }

        // GET api/transactionapi
        public virtual IEnumerable<TMappedModel> Get()
        {
            return _respository.GetAllFiltered().MapEnumerableFromTo<TModel, TMappedModel>();
        }

        // GET api/transactionapi/5
        [HttpGet]
        public virtual ResponseContainer<TMappedModel> Get(TIdentityType id)
        {
            return new ResponseContainer<TMappedModel>(_respository.Find(id).MapTo<TMappedModel>());
        }

        // POST api/transactionapi
        [HttpPost]
        public virtual HttpResponseMessage Post([FromBody] TMappedModel value)
        {
            try
            {
                ValidateWithStrategy(value);

                if (!ModelState.IsValid)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value.MapTo<TModel>());
                _respository.Save();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // PUT api/transactionapi/5
        public virtual HttpResponseMessage Put(int id, [FromBody] TMappedModel value)
        {
            try
            {
                ValidateWithStrategy(value);

                if (!ModelState.IsValid)
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                _respository.InsertOrUpdate(value.MapTo<TModel>());
                _respository.Save();
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        // DELETE api/transactionapi/5
        public virtual HttpResponseMessage Delete(TIdentityType id)
        {
            _respository.Delete(id);
            _respository.Save();

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}