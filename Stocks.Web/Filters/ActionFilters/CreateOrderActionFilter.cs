using Microsoft.AspNetCore.Mvc.Filters;
using Stocks.Core.DTO;
using Stocks.Web.Controllers;
using Stocks.Web.Models;

namespace Stocks.Web.Filters.ActionFilters
{
    //Not the easiest way to implement filters but it demonstrates how to do it without using ServiceFilter or TypeFilter and still be able to pass parameters and inject services into the filter while being an attribute itself.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CreateOrderActionFilterAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        public int Order { get; set; } = 0;
        public bool IsReusable { get; set; } = false;
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var instance = serviceProvider.GetRequiredService<CreateOrderActionFilter>();
            instance.Logger = serviceProvider.GetRequiredService<ILogger<CreateOrderActionFilter>>();
            return instance;
        }
    }
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public ILogger<CreateOrderActionFilter>? Logger;
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController controller && context.ActionArguments.TryGetValue("orderRequest", out var value))
            {
                var request = value as IOrderRequest;
                if (request != null)
                {
                    request.DateAndTimeOfOrder = DateTime.Now;
                    context.ModelState.Clear();
                    controller.TryValidateModel(request);
                    if (!context.ModelState.IsValid)
                    {
                        controller.ViewBag.Errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                        var stockTrade = new StockTrade() { StockName = request.StockName, Quantity = request.Quantity, StockSymbol = request.StockSymbol };
                        context.Result = controller.View(nameof(TradeController.Index), stockTrade);
                        return; //return here to prevent the action from executing thus short-circuit
                    }
                }
            }

            await next();

        }
    }
}
