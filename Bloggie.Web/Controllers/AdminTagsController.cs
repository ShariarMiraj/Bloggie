using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.viewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
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

            await tagRepository.AddAsync(tag);
             

            return RedirectToAction("List");
        }




        [HttpGet]
        [ActionName("List")]
        public async Task <IActionResult> List()
        {
            //USe Dbcontext to read the Tags
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task <IActionResult> Edit(Guid id)
        {
           var tag = await tagRepository.GetAsync(id);


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

            

            var updatedTag = await tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                //showing error notification    
            }
            else
            {
                //showing error notification 
            }




            return RedirectToAction("Edit", new {id = editTagRequst.Id });



        }

        [HttpPost]
        public async Task <IActionResult> Delete(EditTagRequst editTagRequst)
        {
            
             var deleteTag = await tagRepository.DeleteAsync(editTagRequst.Id);
            if (deleteTag != null)
            {    
                //Showing succes notification 
                return RedirectToAction("List");
            }

            //showing an error notification
            return RedirectToAction("Eidt", new {id =editTagRequst.Id});

        }
         
    }
}
