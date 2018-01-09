namespace MovieCatalog.Tests.Mocks
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Routing;
    using Moq;

    public static class AjaxRequestMock
    {
        public static void InjectAjaxRequest(this Controller controller, bool isAjaxRequest)
        {
            Mock<HttpRequest> httpRequest = new Mock<HttpRequest>();
            httpRequest
                .Setup(x => x.Headers["X-Requested-With"])
                .Returns(isAjaxRequest ? "XMLHttpRequest" : string.Empty);

            Mock<HttpContext> httpContext = new Mock<HttpContext>();
            httpContext
                .Setup(x => x.Request)
                .Returns(httpRequest.Object);

            controller.ControllerContext = new ControllerContext(
                new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor())
            );
        }
    }
}
