namespace MovieCatalog.Web.Infrastructure.Filters
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// This action filter validates the model state only when:
    /// - the action receives model as a parameter
    /// - the model contains 'model' in its name
    /// - the model doesn't have additional logic in the action
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Controller controller = context.Controller as Controller;

                if (controller == null)
                {
                    return;
                }

                object model = context.ActionArguments
                    .FirstOrDefault(a => a.Key.ToLower().Contains("model"))
                    .Value;

                if (model == null)
                {
                    return;
                }

                context.Result = controller.View(model);
            }

            base.OnActionExecuting(context);
        }
    }
}
