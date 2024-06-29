namespace FakeSpoon.Wikipedia.Mirror.Domain.Wikipedia.Models;

public class WikiLink
{
        public WikiLink(string plainText)
        {
                var content = plainText[2..(plainText.Length - 2)];
                var pipeIndex = content.IndexOf('|');
                
                if (pipeIndex == -1)
                {
                        Target = content;
                        Text = content;
                        return;
                }

                Target = content[..pipeIndex];
                Text = content[(pipeIndex + 1)..];

        }
        
        public string Target { get; set; }
        public string? Text { get; set; } = null;

        public override string ToString()
        {
                if (Target == Text)
                {
                        return $"[[{Target}]]";
                }

                return $"[[{Target}|{Text}]]";
        }
}