namespace MyRecipes.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    public class ChatController : BaseController
    {

        [Authorize]
        public IActionResult Chat()
        {
            return this.View();
        }
    }
}
