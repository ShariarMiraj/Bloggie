using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.viewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggie.Web.Controllers
{
    public class AdminBlogPostsController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //get tags from repository
            var tags = await tagRepository.GetAllAsync();


            var model = new AddBlogPostRrequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRrequest addBlogPostRrequest)
        {
            //Maping view model to domain model 
            var blogpost = new BlogPost
            {
                Heading = addBlogPostRrequest.Heading,
                PageTittle = addBlogPostRrequest.PageTittle,
                Content = addBlogPostRrequest.Content,
                ShortDescription = addBlogPostRrequest.ShortDescription,
                FeaturedImageUrl = addBlogPostRrequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRrequest.UrlHandle,
                PublishedDate = addBlogPostRrequest.PublishedDate,
                Author = addBlogPostRrequest.Author,
                Visible = addBlogPostRrequest.Visible,



            };

            // map tags from selected tags
            var selectedTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRrequest.SelectedTags)
            {
                var selectedTagIdAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagIdAsGuid);


                if (existingTag != null)
                {
                    selectedTags.Add(existingTag);
                }

            }
            //Maping tags back to domain model 
            blogpost.Tags = selectedTags;

            await blogPostRepository.AddAsync(blogpost);

            return RedirectToAction("Add");
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {
            //call the repository
            var  blogPost = await blogPostRepository.GetAllAsync();
            return View(blogPost);
        }
    
    }

  }

