namespace Hoff.Server.Web.Helpers
{
    internal static class ExtentionMethods
    {
        internal static string Inject(this Resources.StringResources template, string content, string tag = "{content}")
        {
            // get the real string from the resources
            string realTemplate = Resources.GetString(template);

            // Get the place to inject
            int index = realTemplate.IndexOf(tag);

            // Split the template without the tag included.
            string start = realTemplate.Substring(0, index); // The template before the tag.
            string end = realTemplate.Substring(index + tag.Length); // The template after the tag.

            //inject 
            string injected = start + content + end;

            // Return the new string with the injection added.
            return injected;
        }
    }
}