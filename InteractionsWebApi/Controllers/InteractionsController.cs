namespace InteractionsWebApi.Controllers
{
    using System.Web.Http;
    using InteractionsWebApi.Models;
    using System.Collections.Generic;
    using System.Linq;

    using global::Models;

    public class InteractionsController : ApiController
    {
        // GET: api/Interactions
        public IEnumerable<InteractionsModel> Get()
        {
            return WebApiApplication.InteractionsModels;
        }

        // GET: api/Interactions/5
        public InteractionsModel Get(string id)
        {
            return WebApiApplication.InteractionsModels.First(m => m.Id.Equals(id));
        }

        // POST: api/Interactions
        public void Post([FromBody]InteractionsModel value)
        {
            WebApiApplication.InteractionsModels.Add(value);
        }

        // PUT: api/Interactions/5
        public void Put(string id, [FromBody]InteractionsModel newModel)
        {
            var oldModel = this.Get(id);
            oldModel.Delivered = newModel.Delivered;
            oldModel.Sent = newModel.Sent;
        }

        // DELETE: api/Interactions/5
        public void Delete(string id)
        {
            var modelToRemove = this.Get(id);
            WebApiApplication.InteractionsModels.Remove(modelToRemove);
        }

        [HttpGet]
        [Route("api/Interactions/reset")]
        public void Reset()
        {
            WebApiApplication.InteractionsModels.Clear();
        }

        [HttpPut]
        [Route("api/Interactions/upsert")]
        public void Upsert([FromBody] InteractionsModel newModel)
        {
            var existingModel = WebApiApplication.InteractionsModels.FirstOrDefault(m => m.Id.Equals(newModel.Id));
            if (existingModel == null)
            {
                WebApiApplication.InteractionsModels.Add(newModel);
            }
            else
            {
                existingModel.Sent = newModel.Sent;
                existingModel.Delivered = newModel.Delivered;
            }
        }
    }
}
