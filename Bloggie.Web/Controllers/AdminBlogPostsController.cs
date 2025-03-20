using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.viewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;

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


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            //Retrieve the result fromt the repos
            var blogPost = await blogPostRepository.GetAsync(id);
            var tagDomainModel = await tagRepository.GetAllAsync();


            if (blogPost != null)
            {


                //map the domain model inoto the view model 

                var model = new EditBlogPostRequest
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTittle = blogPost.PageTittle,
                    Content = blogPost.Content,

                    Author = blogPost.Author,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    ShortDescription = blogPost.ShortDescription,
                    PublishedDate = blogPost.PublishedDate,
                    Visible = blogPost.Visible,

                    Tags = tagDomainModel.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }),
                    SelectedTags = blogPost.Tags.Select(x => x.Id.ToString()).ToArray()
                };

                return View(model);



            }


            //pass daataa to view
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            //map view model back to domain model 
            var blogPostDomainModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                PageTittle = editBlogPostRequest.PageTittle,
                Content = editBlogPostRequest.Content,
                Author = editBlogPostRequest.Author,
                ShortDescription=editBlogPostRequest.ShortDescription,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                Visible = editBlogPostRequest.Visible,
            };
            //map tags into domain model 
            var selectedTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);

                    if (foundTag != null)
                    {
                        selectedTags.Add(foundTag);
                    }
                }
            }
             blogPostDomainModel.Tags = selectedTags;


            //submit info to repos to update
            var updatedBlog =  await blogPostRepository.UpdateAsync(blogPostDomainModel);

            if (updatedBlog != null)
            {
                //show success notification 
                return RedirectToAction("Edit");
            }
            //show erro notification 
            return RedirectToAction("Edit");
        }


        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            //Talk to repos to delete the blog post and tag
            var deletedBlogPost = await blogPostRepository.DeleteAsync(editBlogPostRequest.Id);

            if (deletedBlogPost != null)
            {
                //show success notification 
                return RedirectToAction("List");
            }

            // errror notification      >>>    // passiing a object for Guid id 
            return RedirectToAction("Edit" , new { id = editBlogPostRequest.Id });


            //displaying the repos
        }
    
    }

  }

