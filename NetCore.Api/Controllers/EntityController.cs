using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.Business.Services;

namespace NetCore.Api.Controllers
{
    public class EntityController<TModel, TModelData> : Controller
        where TModel : class
        where TModelData : class
    {
        protected readonly IEntityService<TModel, TModelData> _entityService;

        protected EntityController(IEntityService<TModel, TModelData> entityService)
        {
            _entityService = entityService;
        }

        protected async Task<IActionResult> GetAllAsync()
        {
            var result = await _entityService.GetAllAsync();
            return Ok(result);
        }

        protected async Task<IActionResult> GetAsync(long id)
        {
            var result = await _entityService.GetAsync(id);
            return Ok(result);
        }

        protected async Task<IActionResult> CreateAsync([FromBody]TModelData model)
        {
            var result = await _entityService.CreateAsync(model);
            return Ok(result);
        }

        protected async Task<IActionResult> UpdateAsync(long id, [FromBody]TModelData model)
        {
            var result = await _entityService.UpdateAsync(id, model);
            return Ok(result);
        }

        protected async Task<IActionResult> DeleteAsync(long id)
        {
            await _entityService.DeleteAsync(id);
            return Ok();
        }
    }

}