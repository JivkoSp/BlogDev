using Data.Models;
using Data.Models.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Data
{
    internal sealed class BlogApiJsonDirectAccess : IBlogApi
    {
        private readonly BlogApiJsonDirectAccessSetting _settings;

        public BlogApiJsonDirectAccess(IOptions<BlogApiJsonDirectAccessSetting> option)
        {
            _settings = option.Value;
            ManageDataPaths();
        }

        private void ManageDataPaths()
        {
            CreateDirectoryIfNotExists(_settings.DataPath);
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.BlogPostsFolder}");
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.CategoriesFolder}");
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.TagsFolder}");
            CreateDirectoryIfNotExists($@"{_settings.DataPath}\{_settings.CommentsFolder}");
        }

        private static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private async Task<List<T>> LoadAsync<T>(string folder)
        {
            var list = new List<T>();

            foreach (var f in Directory.GetFiles($@"{_settings.DataPath}\{folder}"))
            {
                var json = await File.ReadAllTextAsync(f);

                var blogPost = JsonSerializer.Deserialize<T>(json);

                if (blogPost is not null)
                {
                    list.Add(blogPost);
                }
            }

            return list;
        }

        private async Task SaveAsync<T>(string folder, string filename, T item)
        {
            var filepath = $@"{_settings.DataPath}\{folder}\{filename}.json";

            await File.WriteAllTextAsync(filepath, JsonSerializer.Serialize<T>(item));
        }

        private Task DeleteAsync(string folder, string filename)
        {
            var filepath = $@"{_settings.DataPath}\{folder}\{filename}.json";

            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }

            return Task.CompletedTask;
        }

        public async Task<int> GetBlogPostCountAsync()
        {
            var list = await LoadAsync<BlogPost>(_settings.BlogPostsFolder);

            return list.Count;
        }

        public async Task<List<BlogPost>> GetBlogPostsAsync(int numberofposts, int startindex)
        {
            var list = await LoadAsync<BlogPost>(_settings.BlogPostsFolder);

            return list.Skip(startindex).Take(numberofposts).ToList();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await LoadAsync<Category>(_settings.CategoriesFolder);
        }

        public async Task<List<Tag>> GetTagsAsync()
        {
            return await LoadAsync<Tag>(_settings.TagsFolder);
        }

        public Task<List<Comment>> GetCommentsAsync(string blogPostId)
        {
            throw new NotImplementedException();
        }

        public async Task<BlogPost?> GetBlogPostAsync(string id)
        {
            var list = await LoadAsync<BlogPost>(_settings.BlogPostsFolder);

            return list.FirstOrDefault(bp => bp.Id == id);
        }

        public async Task<Category?> GetCategoryAsync(string id)
        {
            var list = await LoadAsync<Category>(_settings.CategoriesFolder);

            return list.FirstOrDefault(c => c.Id == id);
        }

        public Task<Tag?> GetTagAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost?> SaveBlogPostAsync(BlogPost item)
        {
            throw new NotImplementedException();
        }

        public Task<Category?> SaveCategoryAsync(Category item)
        {
            throw new NotImplementedException();
        }

        public Task<Tag?> SaveTagAsync(Tag item)
        {
            throw new NotImplementedException();
        }

        public Task<Comment?> SaveCommentAsync(Comment item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBlogPostAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCategoryAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTagAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCommentAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
