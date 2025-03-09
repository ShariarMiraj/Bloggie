using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.viewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult SubmitTag(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };


            bloggieDbContext.Tags.Add(tag); 
            bloggieDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            //USe Dbcontext to read the Tags
            var tags = bloggieDbContext.Tags.ToList();

            return View(tags);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var tag = bloggieDbContext.Tags.Find(id);


            if (tag != null)
            {
                var editTagRequst = new EditTagRequst
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName=tag.DisplayName,
                };

                return View(editTagRequst);
            }



            return View(null);
        }


        [HttpPost]
        public IActionResult Edit(EditTagRequst editTagRequst)
        {
            var tag = new Tag
            {
                Id = editTagRequst.Id,
                Name = editTagRequst.Name,
                DisplayName = editTagRequst.DisplayName,
            };

            var existingTag = bloggieDbContext.Tags.Find(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //SAVE CHANGS
                bloggieDbContext.SaveChanges();

                //Showing successs notificcation
                return RedirectToAction("Edit", new { id = editTagRequst.Id });
            }

            //showing error notification 

            //showing error notification 

            return RedirectToAction("Edit", new {id = editTagRequst.Id });



        }

        [HttpPost]
        public IActionResult Delete(EditTagRequst editTagRequst)
        {
            
            var tag= bloggieDbContext.Tags.Find(editTagRequst.Id);

            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);
                bloggieDbContext.SaveChanges();

                //showing a successs notifiaction 
                return RedirectToAction("List");
            }

            //showing an error notification
            return RedirectToAction("Eidt", new {id =editTagRequst.Id});

        }
         
    }
}
