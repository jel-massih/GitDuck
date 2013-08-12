using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitDuck
{
    public class IssueData
    {
        public string url { get; set; }
        public int number { get; set; }
        public string title { get; set; }
        public User user { get; set; }
        public List<Label> labels { get; set; }
        public string state { get; set; }
        public User assignee { get; set; }
        public object milestone { get; set; }
        public int comments { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string closed_at { get; set; }
        public PullRequest pull_request { get; set; }
        public string body { get; set; }
        public string repoInfo { get; set; }
    }

    public class Label
    {
        public string url { get; set; }
        public string name { get; set; }
        public string color { get; set; }
    }

    public class PullRequest
    {
        public object html_url { get; set; }
        public object diff_url { get; set; }
        public object patch_url { get; set; }
    }
}
