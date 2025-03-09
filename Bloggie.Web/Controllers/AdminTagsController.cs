using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        //this is Add without Asynchronouns 
        /*[HttpPost]
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
        }*/


        //with Asynchronouns

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            //Mapping AddTagRequest to Tag domain model
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };


            await  bloggieDbContext.Tags.AddAsync(tag);
            await bloggieDbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }




        [HttpGet]
        [ActionName("List")]
        public async Task <IActionResult> List()
        {
            //USe Dbcontext to read the Tags
            var tags = await bloggieDbContext.Tags.ToListAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task <IActionResult> Edit(Guid id)
        {
            var tag = await bloggieDbContext.Tags.FindAsync(id);


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
        public async Task <IActionResult> Edit(EditTagRequst editTagRequst)
        {
            var tag = new Tag
            {
                Id = editTagRequst.Id,
                Name = editTagRequst.Name,
                DisplayName = editTagRequst.DisplayName,
            };

            var existingTag = await bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                //SAVE CHANGS
                await bloggieDbContext.SaveChangesAsync();

                //Showing successs notificcation
                return RedirectToAction("Edit", new { id = editTagRequst.Id });
            }

            //showing error notification 

            //showing error notification 

            return RedirectToAction("Edit", new {id = editTagRequst.Id });



        }

        [HttpPost]
        public async Task <IActionResult> Delete(EditTagRequst editTagRequst)
        {
            
            var tag= await bloggieDbContext.Tags.FindAsync(editTagRequst.Id);

            if (tag != null)
            {
                bloggieDbContext.Tags.Remove(tag);
                await bloggieDbContext.SaveChangesAsync();

                //showing a successs notifiaction 
                return RedirectToAction("List");
            }

            //showing an error notification
            return RedirectToAction("Eidt", new {id =editTagRequst.Id});

        }
         
    }
}
