using System;
using System.Collections.Generic;

namespace Nop.Core.Domain.Blogs
{
    /// <summary>
    /// 博客扩展类
    /// </summary>
    public static class BlogExtensions
    {
        /// <summary>
        /// 解析标签
        /// </summary>
        /// <param name="blogPost"></param>
        /// <returns></returns>
        public static string[] ParseTags(this BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException("blogPost");

            var parsedTags = new List<string>();
            if (!string.IsNullOrEmpty(blogPost.Tags))
            {
                string[] tags2 = blogPost.Tags.Split(new [] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tag2 in tags2)
                {
                    var tmp = tag2.Trim();
                    if (!string.IsNullOrEmpty(tmp))
                        parsedTags.Add(tmp);
                }
            }
            return parsedTags.ToArray();
        }
    }
}
