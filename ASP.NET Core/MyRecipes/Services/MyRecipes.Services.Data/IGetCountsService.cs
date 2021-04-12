namespace MyRecipes.Services.Data
{
using MyRecipes.Web.ViewModels.Home;
    public interface IGetCountsService
    {
        //1.Use view model

        IndexViewModel GetCount();
    }
}
