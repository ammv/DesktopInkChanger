namespace ShellLinkChanger
{
    public class ShellLinkData
    {
        public ShellLinkData(string path, string arguments)
        {
            Path = path;
            Arguments = arguments;
        }

        public string Path { get; }
        public string Arguments { get; }
    }
}
