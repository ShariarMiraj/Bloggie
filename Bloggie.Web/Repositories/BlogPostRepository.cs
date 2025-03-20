using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly BloggieDbContext bloggieDbContext;

        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogpost)
        {
            await bloggieDbContext.AddAsync(blogpost);
            await bloggieDbContext.SaveChangesAsync();
            return blogpost;
        }

        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlog = await bloggieDbContext.BlogPosts.FindAsync(id);
            if (existingBlog != null)
            {
                bloggieDbContext.BlogPosts.Remove(existingBlog);
                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;

        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
           return  await bloggieDbContext.BlogPosts.Include( x=> x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await bloggieDbContext.BlogPosts.Include(x=> x.Tags).FirstOrDefaultAsync(x => x.Id ==id);
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogpost)
        {
             var existingBlog = await bloggieDbContext.BlogPosts.Include(x => x.Tags).
                FirstOrDefaultAsync(x => x.Id == blogpost.Id);


            if (existingBlog != null)
            {
                existingBlog.Id = blogpost.Id;
                existingBlog.Heading = blogpost.Heading;
                existingBlog.PageTittle = blogpost.PageTittle;
                existingBlog.Author = blogpost.Author;
                existingBlog.Content = blogpost.Content;
                existingBlog.ShortDescription = blogpost.ShortDescription;
                existingBlog.FeaturedImageUrl = blogpost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogpost.UrlHandle;
                existingBlog.PublishedDate = blogpost.PublishedDate;    
                existingBlog.Visible = blogpost.Visible;
                existingBlog.Tags = blogpost.Tags;

                await bloggieDbContext.SaveChangesAsync();
                return existingBlog;

            }

            return null;

        }
    }
}
